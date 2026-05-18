using System;
using Avalonia;
using Avalonia.Controls;
using RigorStarter.Core.Interfaces;
using RigorStarter.Shared.Utilities;

namespace RigorStarter.Components;

public partial class StatusBadge : UserControl, IComponentModule
{
    public static readonly StyledProperty<BadgeStatus> StatusProperty = AvaloniaProperty.Register<
        StatusBadge,
        BadgeStatus
    >(nameof(Status));

    public BadgeStatus Status
    {
        get => GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public new string Name => "StatusBadge";
    public string Description => "A small indicator for status states";
    public ComponentCategory Category => ComponentCategory.InDevelopment;
    public Type ViewType => typeof(StatusBadge);
    public bool IsMockup => false;

    public StatusBadge()
    {
        InitializeComponent();
    }
}
