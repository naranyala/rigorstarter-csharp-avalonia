using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RigorStarter.ViewModels;

public enum MarkdownMode
{
    Edit,
    View,
}

public partial class MarkdownDemoViewModel : SearchItemViewModel
{
    [ObservableProperty]
    private string _markdownText = "# Hello Markdown\n\nStart editing to see changes.";

    [ObservableProperty]
    private MarkdownMode _currentMode = MarkdownMode.Edit;

    [RelayCommand]
    private void ToggleMode()
    {
        CurrentMode = CurrentMode == MarkdownMode.Edit ? MarkdownMode.View : MarkdownMode.Edit;
    }

    // A very primitive "renderer" for demo purposes since we don't have a markdown library.
    // In a real app, this would use Markdig.
    public string RenderedText => SimulateMarkdownRendering(MarkdownText);

    private string SimulateMarkdownRendering(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Very basic replacement for demo: # -> Title, ** -> Bold-ish
        var lines = input.Split('\n');
        var renderedLines = new List<string>();

        foreach (var line in lines)
        {
            if (line.StartsWith("# "))
            {
                renderedLines.Add($"--- {line.Substring(2).ToUpper()} ---");
            }
            else if (line.StartsWith("## "))
            {
                renderedLines.Add($"*** {line.Substring(3)} ***");
            }
            else
            {
                renderedLines.Add(line);
            }
        }

        return string.Join(Environment.NewLine, renderedLines);
    }
}
