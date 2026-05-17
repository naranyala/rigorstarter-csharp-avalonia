# Project Architecture

RigorStarter is built using the Avalonia UI framework and follows the Model-View-ViewModel (MVVM) architectural pattern.

## Directory Structure

- **/Components**: Contains reusable UI controls. Each component consists of an .axaml file for layout and an .axaml.cs file for logic.
- **/Views**: Contains the main window and page-level layouts.
- **/ViewModels**: contains the application logic. It separates the UI state from the view.
- **/Utilities**: Contains system-level utilities that interact with the host OS to gather hardware and software information.
- **/Converters**: Contains IValueConverter implementations used to map data states to visual properties (e.g., boolean to color).
- **/tests**: Contains the xUnit test suite for validating business logic and utilities.

## Data Flow

1. The `ViewModelDataService` initializes the list of available components and utilities.
2. `MainWindowViewModel` manages the selection state and search filtering.
3. The `MainWindow` view binds to these properties to determine which component or utility detail to render.
4. Utilities are executed via commands, and their results are piped back to the UI through the `SearchItemViewModel`.
