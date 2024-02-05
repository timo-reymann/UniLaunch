using System;

namespace UniLaunch.UI.CodeGeneration;

/// <summary>
/// Add this annotation to an partial class, extending from ReactiveUI.ReactiveObject to generate a bindable property
/// for each property of the model
/// </summary>
/// <see cref="ReactiveUI.ReactiveObject"/>
/// <param name="modelType"></param>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class GenerateViewModelAttribute(Type modelType) : Attribute
{
    /// <summary>
    /// Type of the model
    /// </summary>
    public Type ModelType { get; set; } = modelType;
}