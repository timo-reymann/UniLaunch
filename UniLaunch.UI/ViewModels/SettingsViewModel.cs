using System;
using System.Linq;
using ReactiveUI;
using UniLaunch.Core.Autostart;
using UniLaunch.Core.ConnectivityCheck;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Configuration;

namespace UniLaunch.UI.ViewModels;

[GenerateViewModel(typeof(EditorConfiguration))]
public partial class SettingsViewModel
{
    public bool HasChangesForConnectivityCheckConfig { get; private set; }

    private TimeSpan? _connectivityCheckConfigurationTimeout;

    public TimeSpan? ConnectivityCheckConfigurationTimeout
    {
        get => _connectivityCheckConfigurationTimeout;
        set
        {
            if (FromConfig().TimeoutOrDefault() == value)
            {
                return;
            }

            FromConfig().Timeout = value;
            HasChangesForConnectivityCheckConfig = true;
            this.RaiseAndSetIfChanged(ref _connectivityCheckConfigurationTimeout, value);
        }
    }

    private Uri _connectivityCheckConfigurationEndpoint;

    public Uri ConnectivityCheckConfigurationEndpoint
    {
        get => _connectivityCheckConfigurationEndpoint;
        set
        {
            if (FromConfig().EndpointOrDefault() == value)
            {
                return;
            }
            
            FromConfig().Endpoint = value;
            HasChangesForConnectivityCheckConfig = true;
            this.RaiseAndSetIfChanged(ref _connectivityCheckConfigurationEndpoint, value);
        }
    }

    private ConnectivityCheckConfiguration FromConfig()
    {
        return UniLaunchEngine.Instance!.Configuration!.ConnectivityCheck;
    }

    partial void InitViewModel()
    {
        _connectivityCheckConfigurationTimeout = FromConfig().TimeoutOrDefault();
        _connectivityCheckConfigurationEndpoint = FromConfig().EndpointOrDefault();

        _propertiesToWatchForChanges = _propertiesToWatchForChanges
            .Append(nameof(ConnectivityCheckConfigurationTimeout))
            .ToArray();
    }
}