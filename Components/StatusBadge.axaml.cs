using Avalonia;
using Avalonia.Controls;
using RigorStarter.Shared.Utilities;

namespace RigorStarter.Components;

public partial class StatusBadge : UserControl
{
    public static readonly StyledProperty<BadgeStatus> StatusProperty = AvaloniaProperty.Register<
        StatusBadge,
        BadgeStatus
    >(nameof(Status), BadgeStatus.Info);

    public BadgeStatus Status
    {
        get => GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public StatusBadge()
    {
        InitializeComponent();
        DataContext = this;
    }
}
