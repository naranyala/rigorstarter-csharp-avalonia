using System;
using System.Collections.Generic;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using RigorStarter.ViewModels;

namespace RigorStarter.Core;

public static class ServiceProvider
{
    private static readonly Dictionary<Type, object> _services = new();

    static ServiceProvider()
    {
        // Register Services (Singletons)
        var systemService = new SystemService();
        var dataService = new DataService(systemService);
        var themeService = new ThemeService();

        _services[typeof(ISystemService)] = systemService;
        _services[typeof(IDataService)] = dataService;
        _services[typeof(IThemeService)] = themeService;

        // Register ViewModels
        _services[typeof(MainWindowViewModel)] = new MainWindowViewModel(dataService, themeService);
    }

    public static T GetService<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }
        throw new Exception($"Service of type {typeof(T).Name} not registered.");
    }
}
