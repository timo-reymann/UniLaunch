using System;
using ReactiveUI;
using UniLaunch.Core.Spec;

namespace UniLaunch.UI.CodeGeneration;

public abstract class BaseEntityViewModel : ReactiveObject, IAssociatedUserControl, INameable
{
    /// <inheritdoc />
    public abstract Type UserControl { get; }

    /// <inheritdoc />
    public abstract string Name { get; }
    
    public abstract object Model { get; }
}