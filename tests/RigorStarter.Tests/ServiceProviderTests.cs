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
    public void GetService_ShouldReturnRegisteredServices()
    {
        // Act & Assert
        Assert.IsAssignableFrom<MainWindowViewModel>(
            ServiceProvider.GetService<MainWindowViewModel>()
        );
        Assert.IsAssignableFrom<IDataService>(ServiceProvider.GetService<IDataService>());
        Assert.IsAssignableFrom<ISystemService>(ServiceProvider.GetService<ISystemService>());
    }

    [Fact]
    public void GetService_ShouldThrowExceptionForUnregisteredType()
    {
        // Act & Assert
        Assert.Throws<Exception>(() => ServiceProvider.GetService<Random>());
    }
}
