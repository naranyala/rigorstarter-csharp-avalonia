using System.Linq;
using RigorStarter.Core;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class ViewModelTests
{
    private MainWindowViewModel CreateVM() =>
        new MainWindowViewModel(
            ServiceProvider.GetService<Core.Interfaces.IDataService>(),
            ServiceProvider.GetService<Core.Interfaces.IThemeService>(),
            ServiceProvider.GetService<Core.Interfaces.ITrayService>(),
            ServiceProvider.GetService<Core.Interfaces.IDialogService>(),
            ServiceProvider.GetService<Core.Interfaces.INotificationService>()
        );

    [Fact]
    public void InitialState_ShouldBeCorrect()
    {
        var vm = CreateVM();
        Assert.False(vm.IsSearchPanelOpen);
        Assert.NotNull(vm.SearchItems);
        Assert.NotEmpty(vm.SearchItems);
        Assert.Null(vm.SelectedItem);
        Assert.False(vm.IsAnyItemSelected);
    }

    [Theory]
    [InlineData("accordion")]
    [InlineData("Card")]
    [InlineData("S")]
    [InlineData("!!!")]
    [InlineData("")]
    [InlineData("    ")] // Whitespace
    [InlineData("VeryLongSearchStringThatProbablyWontMatchAnythingInTheCurrentDataset123456789")] // Stress
    public void Search_ShouldFilterCorrectly(string query)
    {
        var vm = CreateVM();
        vm.SearchText = query;

        if (string.IsNullOrWhiteSpace(query) || query == "!!!" || query.Length > 50)
        {
            if (string.IsNullOrWhiteSpace(query))
                Assert.Equal(vm.SearchItems.Count, vm.FilteredItems.Count);
            else if (query == "!!!")
                Assert.Empty(vm.FilteredItems);
            else
                Assert.Empty(vm.FilteredItems);
        }
        else
        {
            Assert.True(
                vm.FilteredItems.All(i =>
                    i.Name.Contains(query, System.StringComparison.OrdinalIgnoreCase)
                    || i.Description.Contains(query, System.StringComparison.OrdinalIgnoreCase)
                )
            );
        }
    }

    [Fact]
    public void Search_WithCaseInsensitiveQuery_ShouldMatch()
    {
        var vm = CreateVM();
        vm.SearchText = "ACCORDION";
        Assert.NotEmpty(vm.FilteredItems);
        Assert.Contains(vm.FilteredItems, i => i.Name == "Accordion");
    }

    [Fact]
    public void Search_WithPartialName_ShouldMatch()
    {
        var vm = CreateVM();
        vm.SearchText = "Accord";
        Assert.NotEmpty(vm.FilteredItems);
        Assert.Contains(vm.FilteredItems, i => i.Name == "Accordion");
    }

    [Fact]
    public void Search_WithPartialDescription_ShouldMatch()
    {
        var vm = CreateVM();
        vm.SearchText = "collapsible";
        Assert.NotEmpty(vm.FilteredItems);
        Assert.Contains(vm.FilteredItems, i => i.Name == "Accordion");
    }

    [Fact]
    public void Selection_ShouldUpdateCorrectStateProperties()
    {
        // Arrange
        var vm = CreateVM();
        var accordion = vm.SearchItems.First(i => i.Name == "Accordion");

        // Act
        vm.SelectItemCommand.Execute(accordion);

        // Assert
        Assert.True(vm.IsAccordionSelected);
        Assert.False(vm.IsDrawerSelected);
        Assert.False(vm.IsUtilitySelected);
        Assert.True(vm.IsAnyItemSelected);
    }

    [Theory]
    [InlineData("StatusBadge", nameof(MainWindowViewModel.IsStatusBadgeSelected))]
    [InlineData("MetricCard", nameof(MainWindowViewModel.IsMetricCardSelected))]
    public void NewComponents_ShouldUpdateSelectionProperties(
        string componentName,
        string propertyName
    )
    {
        var vm = CreateVM();
        var item = vm.SearchItems.First(i => i.Name == componentName);

        vm.SelectItemCommand.Execute(item);

        var prop = typeof(MainWindowViewModel).GetProperty(propertyName);
        Assert.NotNull(prop);
        var value = (bool)prop.GetValue(vm)!;
        Assert.True(value);
    }

    [Fact]
    public void SelectingUtility_ShouldExecuteAction()
    {
        var vm = CreateVM();
        var networkUtil = vm.SearchItems.First(i => i.Name == "Network Utility");

        Assert.Null(networkUtil.ExecutionResult);

        vm.SelectItemCommand.Execute(networkUtil);

        // After selection, the utility action should have been executed
        Assert.NotNull(networkUtil.ExecutionResult);
        Assert.True(networkUtil.ExecutionResult.IsSuccess);
    }

    [Fact]
    public void SelectingUtility_ShouldCloseSearchPanel()
    {
        var vm = CreateVM();
        vm.ToggleSearchCommand.Execute(null);
        Assert.True(vm.IsSearchPanelOpen);

        var networkUtil = vm.SearchItems.First(i => i.Name == "Network Utility");
        vm.SelectItemCommand.Execute(networkUtil);

        Assert.False(vm.IsSearchPanelOpen);
    }

    [Fact]
    public void GoToDashboard_ShouldResetAllSelectionStates()
    {
        // Arrange
        var vm = CreateVM();
        var accordion = vm.SearchItems.First(i => i.Name == "Accordion");
        vm.SelectItemCommand.Execute(accordion);
        Assert.True(vm.IsAnyItemSelected);

        // Act
        vm.GoToDashboardCommand.Execute(null);

        // Assert
        Assert.False(vm.IsAnyItemSelected);
        Assert.False(vm.IsAccordionSelected);
        Assert.False(vm.IsDrawerSelected);
        Assert.False(vm.IsUtilitySelected);
    }

    [Fact]
    public void ToggleTheme_ShouldCycleDarkThemeState()
    {
        // Arrange
        var vm = CreateVM();
        bool initialTheme = vm.IsDarkTheme;

        // Act
        vm.ToggleThemeCommand.Execute(null);

        // Assert
        Assert.NotEqual(initialTheme, vm.IsDarkTheme);

        // Act again
        vm.ToggleThemeCommand.Execute(null);

        // Assert back to initial
        Assert.Equal(initialTheme, vm.IsDarkTheme);
    }

    [Fact]
    public void ToggleSearch_ShouldTogglePanelOpenState()
    {
        var vm = CreateVM();
        Assert.False(vm.IsSearchPanelOpen);

        vm.ToggleSearchCommand.Execute(null);
        Assert.True(vm.IsSearchPanelOpen);

        vm.ToggleSearchCommand.Execute(null);
        Assert.False(vm.IsSearchPanelOpen);
    }

    [Fact]
    public void ToggleSearch_Closing_ShouldClearSearchText()
    {
        var vm = CreateVM();
        vm.ToggleSearchCommand.Execute(null);
        vm.SearchText = "test query";
        Assert.Equal("test query", vm.SearchText);

        vm.ToggleSearchCommand.Execute(null);
        Assert.Empty(vm.SearchText);
    }

    [Fact]
    public void ExitCommand_ShouldNotThrow()
    {
        var vm = CreateVM();
        // ExitCommand accesses Application.Current which is null in tests
        // The null-conditional operator should prevent any crash
        var exception = Record.Exception(() => vm.ExitCommand.Execute(null));
        Assert.Null(exception);
    }

    [Fact]
    public void UtilityResultText_ShouldReflectExecution()
    {
        var vm = CreateVM();
        var util = vm.SearchItems.First(i => i.IsUtility);
        Assert.Equal(string.Empty, util.ResultText);
        Assert.True(util.ResultIsSuccess);

        vm.SelectItemCommand.Execute(util);
        Assert.NotEqual(string.Empty, util.ResultText);
    }

    [Fact]
    public void TotalItems_ShouldMatchSearchItemsCount()
    {
        var vm = CreateVM();
        Assert.Equal(vm.SearchItems.Count, vm.TotalItems);
    }

    [Fact]
    public void PinnedItems_ShouldBePopulated()
    {
        var vm = CreateVM();
        Assert.NotEmpty(vm.PinnedItems);
        Assert.Contains(vm.PinnedItems, i => i.Name == "Accordion");
    }

    [Fact]
    public void InDevelopmentItems_ShouldBePopulated()
    {
        var vm = CreateVM();
        Assert.NotEmpty(vm.InDevelopmentItems);
        Assert.Contains(vm.InDevelopmentItems, i => i.Name == "Drawer");
    }
}
