using System;
using System.Collections.ObjectModel;
using System.IO;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class DataServiceTests
{
    [Fact]
    public void InitializeComponents_ShouldPopulateCollections()
    {
        // Arrange
        var systemService = new SystemService();
        var service = new DataService(systemService);
        var searchItems = new ObservableCollection<SearchItemViewModel>();
        var pinned = new ObservableCollection<SearchItemViewModel>();
        var dev = new ObservableCollection<SearchItemViewModel>();
        var archives = new ObservableCollection<SearchItemViewModel>();
        var accordion = new ObservableCollection<AccordionItemViewModel>();

        // Act
        service.InitializeComponents(searchItems, pinned, dev, archives, accordion);

        // Assert
        Assert.NotEmpty(searchItems);
        Assert.NotEmpty(pinned);
        Assert.NotEmpty(dev);
        Assert.NotEmpty(archives);
        Assert.NotEmpty(accordion);

        // Verify components are registered
        Assert.Contains(searchItems, i => i.Name == "StatusBadge");
        Assert.Contains(searchItems, i => i.Name == "MetricCard");
        Assert.Contains(dev, i => i.Name == "StatusBadge");
        Assert.Contains(dev, i => i.Name == "MetricCard");
    }

    [Fact]
    public void CountLines_ShouldHandleMissingFilesGracefully()
    {
        // Arrange
        var systemService = new SystemService();
        var service = new DataService(systemService);
        var method = typeof(DataService).GetMethod(
            "CountLines",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );

        // Act
        var result = (int)method.Invoke(service, new object[] { "non_existent_file.txt" });

        // Assert
        Assert.Equal(0, result);
    }
}
