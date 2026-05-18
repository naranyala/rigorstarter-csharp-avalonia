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

        var componentRegistry = new ComponentRegistry();
        componentRegistry.DiscoverModules();

        var dataService = new DataService(systemService, componentRegistry);
        var themeService = new ThemeService();
        var memoryService = new NativeMemoryService();
        var trayService = new TrayService();
        var dialogService = new DialogService();
        var notificationService = new NotificationService();
        var todoService = new TodoService();
        var todoServiceJson = new TodoServiceJson();

        _services[typeof(ISystemService)] = systemService;
        _services[typeof(ComponentRegistry)] = componentRegistry;
        _services[typeof(IDataService)] = dataService;
        _services[typeof(IThemeService)] = themeService;
        _services[typeof(INativeMemoryService)] = memoryService;
        _services[typeof(ITrayService)] = trayService;
        _services[typeof(IDialogService)] = dialogService;
        _services[typeof(INotificationService)] = notificationService;
        _services[typeof(ITodoService)] = todoService;

        // Register ViewModels
        _services[typeof(TodoListViewModel)] = new TodoListViewModel(todoService);
        _services[typeof(TodoListJsonViewModel)] = new TodoListJsonViewModel(todoServiceJson);
        _services[typeof(TreeViewDemoViewModel)] = new TreeViewDemoViewModel();
        _services[typeof(TableDataViewModel)] = new TableDataViewModel();
        _services[typeof(MarkdownDemoViewModel)] = new MarkdownDemoViewModel();

        _services[typeof(MainWindowViewModel)] = new MainWindowViewModel(
            dataService,
            themeService,
            trayService,
            dialogService,
            notificationService
        );
    }

    public static T GetService<T>()
    {
        return (T)GetService(typeof(T));
    }

    public static object GetService(Type type)
    {
        if (_services.TryGetValue(type, out var service))
        {
            return service;
        }
        throw new Exception($"Service of type {type.Name} not registered.");
    }
}
