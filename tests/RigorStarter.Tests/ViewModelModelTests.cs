using RigorStarter.Shared.Models;
using RigorStarter.Shared.Utilities;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class ViewModelModelTests
{
    [Fact]
    public void SearchItemViewModel_DefaultValues_ShouldBeCorrect()
    {
        var vm = new SearchItemViewModel();
        Assert.Equal(string.Empty, vm.Name);
        Assert.Equal(string.Empty, vm.Description);
        Assert.False(vm.IsComponent);
        Assert.False(vm.IsUtility);
        Assert.False(vm.IsMockup);
        Assert.Equal(string.Empty, vm.SourceCode);
        Assert.Null(vm.ExecutionResult);
        Assert.Equal(ComponentCategory.InDevelopment, vm.Category);
        Assert.Equal(0, vm.LinesOfCode);
        Assert.Null(vm.ExecuteAction);
        Assert.Equal(string.Empty, vm.ResultText);
        Assert.True(vm.ResultIsSuccess);
    }

    [Fact]
    public void SearchItemViewModel_ShouldSetProperties()
    {
        var vm = new SearchItemViewModel
        {
            Name = "Test",
            Description = "Test description",
            IsComponent = true,
            IsUtility = false,
            IsMockup = false,
            SourceCode = "source",
            Category = ComponentCategory.Pinned,
            LinesOfCode = 42,
        };

        Assert.Equal("Test", vm.Name);
        Assert.Equal("Test description", vm.Description);
        Assert.True(vm.IsComponent);
        Assert.False(vm.IsUtility);
        Assert.False(vm.IsMockup);
        Assert.Equal("source", vm.SourceCode);
        Assert.Equal(ComponentCategory.Pinned, vm.Category);
        Assert.Equal(42, vm.LinesOfCode);
    }

    [Fact]
    public void SearchItemViewModel_ExecutionResult_ShouldUpdateResultText()
    {
        var vm = new SearchItemViewModel();
        Assert.Equal(string.Empty, vm.ResultText);
        Assert.True(vm.ResultIsSuccess);

        vm.ExecutionResult = new UtilityResult(false, "Failed", "Error details");
        Assert.Equal("Failed", vm.ResultText);
        Assert.False(vm.ResultIsSuccess);

        vm.ExecutionResult = new UtilityResult(true, "OK", null);
        Assert.Equal("OK", vm.ResultText);
        Assert.True(vm.ResultIsSuccess);
    }

    [Fact]
    public void SearchItemViewModel_ExecuteAction_ShouldBeInvokable()
    {
        var vm = new SearchItemViewModel();
        bool invoked = false;
        vm.ExecuteAction = (item) =>
        {
            invoked = true;
            item.ExecutionResult = new UtilityResult(true, "done");
        };

        vm.ExecuteAction(vm);

        Assert.True(invoked);
        Assert.NotNull(vm.ExecutionResult);
        Assert.True(vm.ExecutionResult.IsSuccess);
        Assert.Equal("done", vm.ExecutionResult.Message);
    }

    [Fact]
    public void AccordionItemViewModel_DefaultValues_ShouldBeCorrect()
    {
        var vm = new AccordionItemViewModel();
        Assert.Equal(string.Empty, vm.Header);
        Assert.Equal(string.Empty, vm.Content);
        Assert.False(vm.IsExpanded);
    }

    [Fact]
    public void AccordionItemViewModel_ShouldSetProperties()
    {
        var vm = new AccordionItemViewModel
        {
            Header = "Section 1",
            Content = "Content here",
            IsExpanded = true,
        };

        Assert.Equal("Section 1", vm.Header);
        Assert.Equal("Content here", vm.Content);
        Assert.True(vm.IsExpanded);
    }

    [Fact]
    public void AccordionItemViewModel_ShouldToggleExpanded()
    {
        var vm = new AccordionItemViewModel();
        Assert.False(vm.IsExpanded);

        vm.IsExpanded = true;
        Assert.True(vm.IsExpanded);

        vm.IsExpanded = false;
        Assert.False(vm.IsExpanded);
    }

    [Fact]
    public void ComponentDemoViewModel_DefaultValues_ShouldBeCorrect()
    {
        var vm = new ComponentDemoViewModel();
        Assert.Equal(string.Empty, vm.Name);
        Assert.Equal(string.Empty, vm.Description);
        Assert.False(vm.IsMockup);
    }

    [Fact]
    public void ComponentDemoViewModel_ShouldSetProperties()
    {
        var vm = new ComponentDemoViewModel
        {
            Name = "Button",
            Description = "A button",
            IsMockup = true,
        };

        Assert.Equal("Button", vm.Name);
        Assert.Equal("A button", vm.Description);
        Assert.True(vm.IsMockup);
    }

    [Fact]
    public void ComponentCategory_ShouldHaveAllValues()
    {
        Assert.True(Enum.IsDefined(typeof(ComponentCategory), ComponentCategory.Pinned));
        Assert.True(Enum.IsDefined(typeof(ComponentCategory), ComponentCategory.InDevelopment));
        Assert.True(Enum.IsDefined(typeof(ComponentCategory), ComponentCategory.Archives));
    }
}
