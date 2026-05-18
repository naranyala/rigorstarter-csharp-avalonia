using System.Collections.ObjectModel;
using RigorStarter.ViewModels;

namespace RigorStarter.Core.Interfaces;

public interface IDataService
{
    void InitializeComponents(
        ObservableCollection<SearchItemViewModel> searchItems,
        ObservableCollection<SearchItemViewModel> pinnedItems,
        ObservableCollection<SearchItemViewModel> inDevelopmentItems,
        ObservableCollection<SearchItemViewModel> archivesItems,
        ObservableCollection<AccordionItemViewModel> accordionItems
    );
}
