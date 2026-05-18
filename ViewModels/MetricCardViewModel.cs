using CommunityToolkit.Mvvm.ComponentModel;
using RigorStarter.Shared.Utilities;

namespace RigorStarter.ViewModels;

public partial class MetricCardViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = "CPU Usage";

    [ObservableProperty]
    private BadgeStatus _status = BadgeStatus.Success;

    [ObservableProperty]
    private string _value = "24%";

    [ObservableProperty]
    private string _trend = "↓ 2% from last hour";
}
