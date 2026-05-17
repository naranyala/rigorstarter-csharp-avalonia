using Xunit;
using RigorStarter.ViewModels;
using System.Linq;

namespace RigorStarter.Tests;

public class ViewModelTests
{
    [Fact]
    public void Search_ShouldBeCaseInsensitive()
    {
        // Arrange
        var vm = new MainWindowViewModel();
        
        // Act
        // Simulate typing "accordion" (lowercase) into the search box
        // In the real VM, this happens via the OnSearchTextChanged partial method
        // We can invoke it by setting the property if it's ObservableProperty
        vm.SearchText = "accordion"; 

        // Assert
        // "Accordion" should be in the filtered items
        Assert.Contains(vm.FilteredItems, item => item.Name.Equals("Accordion", System.StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Search_WithNoResults_ShouldReturnEmptyList()
    {
        // Arrange
        var vm = new MainWindowViewModel();
        
        // Act
        vm.SearchText = "NON_EXISTENT_COMPONENT_12345";

        // Assert
        Assert.Empty(vm.FilteredItems);
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
