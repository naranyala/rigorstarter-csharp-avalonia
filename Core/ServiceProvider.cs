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
        var memoryService = new NativeMemoryService();
        var trayService = new TrayService();
        var dialogService = new DialogService();
        var notificationService = new NotificationService();
        var todoService = new TodoService();
        var todoServiceJson = new TodoServiceJson();

        _services[typeof(ISystemService)] = systemService;
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
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }
        throw new Exception($"Service of type {typeof(T).Name} not registered.");
    }
}
