using System;
using System.Collections.ObjectModel;
using System.IO;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class DataServiceTests
{
    [Fact]
    public void InitializeComponents_ShouldPopulateCollections()
    {
        var service = new ViewModelDataService();
        var searchItems = new ObservableCollection<SearchItemViewModel>();
        var pinned = new ObservableCollection<SearchItemViewModel>();
        var dev = new ObservableCollection<SearchItemViewModel>();
        var archives = new ObservableCollection<SearchItemViewModel>();
        var accordion = new ObservableCollection<AccordionItemViewModel>();

        service.InitializeComponents(searchItems, pinned, dev, archives, accordion);

        Assert.NotEmpty(searchItems);
        Assert.NotEmpty(pinned);
        Assert.NotEmpty(dev);
        Assert.NotEmpty(archives);
        Assert.NotEmpty(accordion);
    }

    [Fact]
    public void CountLines_ShouldHandleMissingFilesGracefully()
    {
        // We use reflection to test the private CountLines method
        var service = new ViewModelDataService();
        var method = typeof(ViewModelDataService).GetMethod(
            "CountLines",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );

        var result = (int)method.Invoke(service, new object[] { "non_existent_file.txt" });

        Assert.Equal(0, result);
    }
}
