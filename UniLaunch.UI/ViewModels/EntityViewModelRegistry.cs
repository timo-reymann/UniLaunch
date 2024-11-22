using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniLaunch.Core.Spec;
using UniLaunch.UI.CodeGeneration;

namespace UniLaunch.UI.ViewModels;

public class EntityViewModelRegistry
{
    private readonly Dictionary<Type, Type> _entityViewModelMapping = new();

    public static EntityViewModelRegistry Instance { get; } = new();

    private EntityViewModelRegistry()
    {
    }

    public EntityViewModelRegistry Register<TEntity, TViewModel>()
        where TEntity : INameable
        where TViewModel : BaseEntityViewModel
    {
        _entityViewModelMapping[typeof(TEntity)] = typeof(TViewModel);
        return this;
    }

    public BaseEntityViewModel? Of<TEntity>(TEntity model) where TEntity : INameable
    {
        var viewModelType = _entityViewModelMapping[model.GetType()];
        var viewModel = Activator.CreateInstance(viewModelType, [model]);
        return viewModel as BaseEntityViewModel;
    }

    public List<BaseEntityViewModel> Of<TEntity>(List<TEntity> models) where TEntity : INameable
    {
        var collection = new List<BaseEntityViewModel>();
        
        foreach (var model in models)
        {
            collection.Add(Of(model)!);
        }

        return collection;
    }
}