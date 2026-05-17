using System.Linq;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class ViewModelTests
{
    [Theory]
    [InlineData("accordion")] // Case insensitive
    [InlineData("Card")] // Exact match
    [InlineData("S")] // Partial match (Accordion, Search, System, etc.)
    [InlineData("!!!")] // No match
    [InlineData("")] // Empty search (Total items)
    public void Search_ShouldFilterCorrectly(string query)
    {
        var vm = new MainWindowViewModel();
        vm.SearchText = query;

        // We can't assert exact count without knowing current dataset,
        // so we verify it's not crashing and produces results relative to the query.
        if (query == "!!!")
            Assert.Empty(vm.FilteredItems);
        else if (string.IsNullOrEmpty(query))
            Assert.Equal(vm.SearchItems.Count, vm.FilteredItems.Count);
        else
            Assert.True(
                vm.FilteredItems.All(i =>
                    i.Name.Contains(query, System.StringComparison.OrdinalIgnoreCase)
                    || i.Description.Contains(query, System.StringComparison.OrdinalIgnoreCase)
                )
            );
    }

    [Fact]
    public void Selection_ShouldUpdateCorrectStateProperties()
    {
        // Arrange
        var vm = new MainWindowViewModel();
        var accordion = vm.SearchItems.First(i => i.Name == "Accordion");

        // Act
        vm.SelectItemCommand.Execute(accordion);

        // Assert
        Assert.True(vm.IsAccordionSelected);
        Assert.False(vm.IsDrawerSelected);
        Assert.False(vm.IsUtilitySelected);
        Assert.True(vm.IsAnyItemSelected);
    }

    [Fact]
    public void GoToDashboard_ShouldResetAllSelectionStates()
    {
        // Arrange
        var vm = new MainWindowViewModel();
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
}
