using System.Collections.Generic;
using UniLaunch.Core.Spec;
using UniLaunch.UI.CodeGeneration;

namespace UniLaunch.UI.ViewModels;

public static class EntityViewModelRegistryExtensions
{
    public static BaseEntityViewModel? ToViewModel(this INameable entity)
    {
        return EntityViewModelRegistry.Instance.Of(entity);
    }
    
    public static  List<BaseEntityViewModel?> ToViewModel<T>(this List<T> entity) where T : INameable
    {
        return EntityViewModelRegistry.Instance.Of(entity);
    }
}