using Avalonia.Controls;
using System.ComponentModel;
using RigorStarter.ViewModels;

namespace RigorStarter.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // Subscribe to DataContext changes to handle focus
        this.DataContextChanged += (s, e) =>
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(MainWindowViewModel.IsSearchPanelOpen))
                    {
                        if (vm.IsSearchPanelOpen)
                        {
                            // Use Dispatcher to ensure the element is rendered before focusing
                            Avalonia.Threading.Dispatcher.UIThread.Post(() => 
                            {
                                SearchTextBox?.Focus();
                            });
                        }
                    }
                };
            }
        };
    }
}
