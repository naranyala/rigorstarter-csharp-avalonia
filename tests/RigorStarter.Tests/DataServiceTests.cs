using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Moq;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class DataServiceTests
{
    private readonly Mock<ISystemService> _systemServiceMock;
    private readonly ComponentRegistry _registry;
    private readonly DataService _service;

    public DataServiceTests()
    {
        _systemServiceMock = new Mock<ISystemService>();
        _systemServiceMock.Setup(s => s.GetNetworkSummary()).Returns("Network: OK");
        _systemServiceMock.Setup(s => s.GetDiskSummary()).Returns("Disk: 50% used");
        _systemServiceMock.Setup(s => s.GetSystemSummary()).Returns("System: Linux");
        _systemServiceMock.Setup(s => s.GetTopProcesses()).Returns("Processes: none");
        _systemServiceMock.Setup(s => s.GetMemorySummary()).Returns("Memory: 8GB/16GB");
        _systemServiceMock.Setup(s => s.GetCpuSummary()).Returns("CPU: 25%");

        _registry = new ComponentRegistry();
        // Add dummy modules for testing
        _registry.Register(
            new TestModule { Name = "Accordion", Category = ComponentCategory.Pinned }
        );
        _registry.Register(
            new TestModule { Name = "Drawer", Category = ComponentCategory.InDevelopment }
        );
        _registry.Register(
            new TestModule { Name = "CustomChart", Category = ComponentCategory.Archives }
        );

        _service = new DataService(_systemServiceMock.Object, _registry);
    }

    private class TestModule : IComponentModule
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "Test";
        public ComponentCategory Category { get; set; }
        public Type ViewType => typeof(object);
        public bool IsMockup { get; set; } = false;
    }

    [Fact]
    public void InitializeComponents_ShouldPopulateCollectionsFromRegistry()
    {
        // Arrange
        var searchItems = new ObservableCollection<SearchItemViewModel>();
        var pinned = new ObservableCollection<SearchItemViewModel>();
        var dev = new ObservableCollection<SearchItemViewModel>();
        var archives = new ObservableCollection<SearchItemViewModel>();
        var accordion = new ObservableCollection<AccordionItemViewModel>();

        // Act
        _service.InitializeComponents(searchItems, pinned, dev, archives, accordion);

        // Assert
        Assert.NotEmpty(searchItems);
        Assert.Contains(searchItems, i => i.Name == "Accordion");
        Assert.Contains(pinned, i => i.Name == "Accordion");
        Assert.Contains(searchItems, i => i.Name == "Drawer");
        Assert.Contains(dev, i => i.Name == "Drawer");
        Assert.Contains(searchItems, i => i.Name == "CustomChart");
        Assert.Contains(archives, i => i.Name == "CustomChart");
    }

    [Fact]
    public void InitializeComponents_ShouldRegisterUtilities()
    {
        // Arrange
        var searchItems = new ObservableCollection<SearchItemViewModel>();
        var collections = CreateEmptyCollections();

        // Act
        _service.InitializeComponents(
            searchItems,
            collections.pinned,
            collections.dev,
            collections.archives,
            collections.accordion
        );

        // Assert
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "Network Utility");
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "Disk Utility");
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "System Info Utility");
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "Process Utility");
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "Memory Utility");
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "CPU Utility");
    }

    [Fact]
    public void UtilityAction_WhenExecuted_ShouldUpdateExecutionResult()
    {
        // Arrange
        var searchItems = new ObservableCollection<SearchItemViewModel>();
        var collections = CreateEmptyCollections();
        _service.InitializeComponents(
            searchItems,
            collections.pinned,
            collections.dev,
            collections.archives,
            collections.accordion
        );

        var networkUtil = searchItems.First(i => i.Name == "Network Utility");
        Assert.NotNull(networkUtil.ExecuteAction);

        // Act
        networkUtil.ExecuteAction(networkUtil);

        // Assert
        Assert.NotNull(networkUtil.ExecutionResult);
        Assert.True(networkUtil.ExecutionResult.IsSuccess);
        Assert.Equal("Network: OK", networkUtil.ExecutionResult.Message);
    }

    [Fact]
    public void UtilityAction_WhenSystemServiceFails_ShouldStillUpdateResult()
    {
        // Arrange
        _systemServiceMock
            .Setup(s => s.GetNetworkSummary())
            .Throws(new InvalidOperationException("Network error"));

        var service = new DataService(_systemServiceMock.Object, _registry);
        var searchItems = new ObservableCollection<SearchItemViewModel>();
        var collections = CreateEmptyCollections();
        service.InitializeComponents(
            searchItems,
            collections.pinned,
            collections.dev,
            collections.archives,
            collections.accordion
        );

        var networkUtil = searchItems.First(i => i.Name == "Network Utility");

        // Act & Assert
        var ex = Record.Exception(() => networkUtil.ExecuteAction!(networkUtil));
        Assert.NotNull(ex);
        Assert.IsType<InvalidOperationException>(ex);
    }

    [Fact]
    public void AccordionItems_ShouldBePopulated()
    {
        // Arrange
        var searchItems = new ObservableCollection<SearchItemViewModel>();
        var collections = CreateEmptyCollections();

        // Act
        _service.InitializeComponents(
            searchItems,
            collections.pinned,
            collections.dev,
            collections.archives,
            collections.accordion
        );

        // Assert
        Assert.Equal(3, collections.accordion.Count);
        Assert.Contains(collections.accordion, a => a.Header == "Section 1" && a.IsExpanded);
        Assert.Contains(collections.accordion, a => a.Header == "Section 2" && !a.IsExpanded);
        Assert.Contains(collections.accordion, a => a.Header == "Section 3" && !a.IsExpanded);
    }

    private static (
        ObservableCollection<SearchItemViewModel> pinned,
        ObservableCollection<SearchItemViewModel> dev,
        ObservableCollection<SearchItemViewModel> archives,
        ObservableCollection<AccordionItemViewModel> accordion
    ) CreateEmptyCollections()
    {
        return (
            new ObservableCollection<SearchItemViewModel>(),
            new ObservableCollection<SearchItemViewModel>(),
            new ObservableCollection<SearchItemViewModel>(),
            new ObservableCollection<AccordionItemViewModel>()
        );
    }
}
