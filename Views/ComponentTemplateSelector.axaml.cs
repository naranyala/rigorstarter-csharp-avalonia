using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using RigorStarter.ViewModels;

namespace RigorStarter.Views;

public class ComponentTemplateSelector : IDataTemplate
{
    public Func<SearchItemViewModel, IDataTemplate>? TemplateResolver { get; set; }

    public Control? Build(object? param)
    {
        if (param is SearchItemViewModel searchItem && TemplateResolver != null)
        {
            var template = TemplateResolver(searchItem);
            if (template != null)
            {
                return template.Build(param);
            }
        }
        return null;
    }

    public bool Match(object? param)
    {
        return param is SearchItemViewModel;
    }
}
