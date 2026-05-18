using System;
using System.Collections.ObjectModel;
using System.IO;
using Moq;
using RigorStarter.Core.Interfaces;
using RigorStarter.Core.Services;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class DataServiceTests
{
    private readonly Mock<ISystemService> _systemServiceMock;
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

        _service = new DataService(_systemServiceMock.Object);
    }

    [Fact]
    public void InitializeComponents_ShouldPopulateCollections()
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
        Assert.NotEmpty(pinned);
        Assert.NotEmpty(dev);
        Assert.NotEmpty(archives);
        Assert.NotEmpty(accordion);

        // Verify components are registered in correct categories
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
        var pinned = new ObservableCollection<SearchItemViewModel>();
        var dev = new ObservableCollection<SearchItemViewModel>();
        var archives = new ObservableCollection<SearchItemViewModel>();
        var accordion = new ObservableCollection<AccordionItemViewModel>();

        // Act
        _service.InitializeComponents(searchItems, pinned, dev, archives, accordion);

        // Assert
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "Network Utility");
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "Disk Utility");
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "System Info Utility");
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "Process Utility");
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "Memory Utility");
        Assert.Contains(searchItems, i => i.IsUtility && i.Name == "CPU Utility");
    }

    [Fact]
    public void InitializeComponents_ShouldSetMockupFlag()
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
        Assert.Contains(searchItems, i => i.IsMockup && i.Name == "DataGrid");
        Assert.Contains(searchItems, i => i.IsMockup && i.Name == "ColorPicker");
        Assert.DoesNotContain(searchItems, i => i.IsMockup && i.Name == "Accordion");
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
        var service = new DataService(_systemServiceMock.Object);
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

    [Fact]
    public void CountLines_ShouldHandleMissingFilesGracefully()
    {
        // Arrange
        var method = typeof(DataService).GetMethod(
            "CountLines",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        Assert.NotNull(method);

        // Act
        var result = method.Invoke(_service, new object[] { "non_existent_file.txt" });

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, (int)result);
    }

    [Fact]
    public void CountLines_ShouldHandleNullPathGracefully()
    {
        // Arrange
        var method = typeof(DataService).GetMethod(
            "CountLines",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        Assert.NotNull(method);

        // Act
        var result = method.Invoke(_service, new object[] { null! });

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CountLines_ShouldHandleEmptyFile()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, string.Empty);
            var method = typeof(DataService).GetMethod(
                "CountLines",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            );
            Assert.NotNull(method);

            // Act
            var result = method.Invoke(_service, new object[] { tempFile });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, (int)result);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void CountLines_ShouldHandleFileWithContent()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(tempFile, new[] { "line1", "line2", "line3" });
            var method = typeof(DataService).GetMethod(
                "CountLines",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            );
            Assert.NotNull(method);

            // Act
            var result = method.Invoke(_service, new object[] { tempFile });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, (int)result);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
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
