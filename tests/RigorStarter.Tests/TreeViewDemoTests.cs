using System.Linq;
using RigorStarter.Shared.Models;
using RigorStarter.ViewModels;
using Xunit;

namespace RigorStarter.Tests;

public class TreeViewDemoTests
{
    [Fact]
    public void ViewModel_ShouldHaveRootNodes()
    {
        var vm = new TreeViewDemoViewModel();

        Assert.NotEmpty(vm.Items);
    }

    [Fact]
    public void ViewModel_RootNode_ShouldBeExpandedByDefault()
    {
        var vm = new TreeViewDemoViewModel();
        var root = vm.Items[0];

        Assert.True(root.IsExpanded);
    }

    [Fact]
    public void ViewModel_RootNode_ShouldHaveChildren()
    {
        var vm = new TreeViewDemoViewModel();
        var root = vm.Items[0];

        Assert.NotEmpty(root.Children);
    }

    [Fact]
    public void ViewModel_AllNodes_ShouldBeExpandedByDefault()
    {
        var vm = new TreeViewDemoViewModel();

        void AssertAllExpanded(TreeNodeItem node)
        {
            Assert.True(node.IsExpanded);
            foreach (var child in node.Children)
                AssertAllExpanded(child);
        }

        foreach (var root in vm.Items)
            AssertAllExpanded(root);
    }

    [Fact]
    public void TreeNodeItem_DefaultValues_ShouldBeCorrect()
    {
        var node = new TreeNodeItem();

        Assert.Equal(string.Empty, node.Name);
        Assert.Empty(node.Children);
        Assert.True(node.IsExpanded);
    }

    [Fact]
    public void TreeNodeItem_ShouldSetProperties()
    {
        var node = new TreeNodeItem { Name = "test.cs", IsExpanded = false };
        node.Children.Add(new TreeNodeItem { Name = "child" });

        Assert.Equal("test.cs", node.Name);
        Assert.False(node.IsExpanded);
        Assert.Single(node.Children);
        Assert.Equal("child", node.Children[0].Name);
    }

    [Fact]
    public void TreeNodeItem_IsExpanded_ShouldToggle()
    {
        var node = new TreeNodeItem();

        Assert.True(node.IsExpanded);

        node.IsExpanded = false;
        Assert.False(node.IsExpanded);

        node.IsExpanded = true;
        Assert.True(node.IsExpanded);
    }

    [Fact]
    public void ViewModel_PopulateDemoData_ShouldHaveNestedStructure()
    {
        var vm = new TreeViewDemoViewModel();
        var root = vm.Items[0];

        Assert.Contains(root.Children, c => c.Name == "src");
        Assert.Contains(root.Children, c => c.Name == "tests");
        Assert.Contains(root.Children, c => c.Name == "docs");

        var src = root.Children.First(c => c.Name == "src");
        Assert.Contains(src.Children, c => c.Name == "main.cs");
        Assert.Contains(src.Children, c => c.Name == "Components");
        Assert.Contains(src.Children, c => c.Name == "ViewModels");

        var components = src.Children.First(c => c.Name == "Components");
        Assert.Contains(components.Children, c => c.Name == "Button.axaml");
    }

    [Fact]
    public void ViewModel_Data_ShouldBeConsistent()
    {
        var vm = new TreeViewDemoViewModel();
        var root = vm.Items[0];

        Assert.Equal("Project Root", root.Name);
        Assert.Equal(3, root.Children.Count);
    }
}
