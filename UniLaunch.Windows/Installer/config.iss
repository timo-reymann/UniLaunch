#define AppName "UniLaunch"
#define AppVersion "0.0.0"
#define AppPublisher "Timo Reymann"
#define AppURL "https://github.com/timo-reymann/UniLaunch"
#define AppExeName "unilaunch.exe"
#define AppLicense "LICENSE"

#define InstallerLogo "UniLaunch.ico"
#define InstallerSourceExe "unilaunch.exe" 
#define InstallerOutputFolder "dist"

[Setup]
AppId={{9EFFF847-8B52-41FE-B825-178991860F12}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultDirName={autopf}\{#AppName}
DisableProgramGroupPage=yes
LicenseFile={#AppLicense}
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputBaseFilename=UniLaunch-Setup
SetupIconFile={#InstallerLogo}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
OutputDir={#InstallerOutputFolder}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#InstallerSourceExe}"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExeName}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Registry]
Root: HKCU; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "UniLaunch"; ValueData: "{app}\{#AppExeName} --autostart"; Flags: uninsdeletekey
