using System;
using RigorStarter.Core;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class ServiceProviderTests
{
    [Fact]
    public void GetService_ShouldReturnMainWindowViewModel()
    {
        var vm = ServiceProvider.GetService<MainWindowViewModel>();
        Assert.NotNull(vm);
        Assert.IsAssignableFrom<MainWindowViewModel>(vm);
    }

    [Fact]
    public void GetService_ShouldReturnAllRegisteredServices()
    {
        Assert.IsAssignableFrom<IDataService>(ServiceProvider.GetService<IDataService>());
        Assert.IsAssignableFrom<ISystemService>(ServiceProvider.GetService<ISystemService>());
        Assert.IsAssignableFrom<IThemeService>(ServiceProvider.GetService<IThemeService>());
        Assert.IsAssignableFrom<INativeMemoryService>(
            ServiceProvider.GetService<INativeMemoryService>()
        );
        Assert.IsAssignableFrom<ITrayService>(ServiceProvider.GetService<ITrayService>());
        Assert.IsAssignableFrom<IDialogService>(ServiceProvider.GetService<IDialogService>());
        Assert.IsAssignableFrom<INotificationService>(
            ServiceProvider.GetService<INotificationService>()
        );
    }

    [Fact]
    public void GetService_ShouldReturnSingletonInstance()
    {
        var first = ServiceProvider.GetService<ISystemService>();
        var second = ServiceProvider.GetService<ISystemService>();
        Assert.Same(first, second);

        var firstVm = ServiceProvider.GetService<MainWindowViewModel>();
        var secondVm = ServiceProvider.GetService<MainWindowViewModel>();
        Assert.Same(firstVm, secondVm);
    }

    [Fact]
    public void GetService_ShouldThrowExceptionForUnregisteredType()
    {
        Assert.Throws<Exception>(() => ServiceProvider.GetService<Random>());
    }

    [Fact]
    public void GetService_ShouldThrowExceptionForInterfaceWithoutImplementation()
    {
        Assert.Throws<Exception>(() => ServiceProvider.GetService<IDisposable>());
    }

    [Fact]
    public void MainWindowViewModel_ShouldHaveAllDependenciesInjected()
    {
        var vm = ServiceProvider.GetService<MainWindowViewModel>();
        Assert.NotNull(vm);

        // Verify view model is functional by checking initial state
        Assert.False(vm.IsSearchPanelOpen);
        Assert.NotNull(vm.SearchItems);
        Assert.NotEmpty(vm.SearchItems);
    }
}
