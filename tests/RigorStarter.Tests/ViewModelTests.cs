using System.Linq;
using RigorStarter.Core;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class ViewModelTests
{
    private MainWindowViewModel CreateVM() => ServiceProvider.GetService<MainWindowViewModel>();

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

        var property = typeof(MainWindowViewModel).GetProperty(propertyName);
        var value = (bool)property.GetValue(vm);

        Assert.True(value);
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
}
