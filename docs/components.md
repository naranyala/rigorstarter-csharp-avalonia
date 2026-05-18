# Component Development Guide

This guide describes the process for implementing and registering new UI components within the RigorStarter framework.

## Implementation Workflow

### 1. Create the Component
Implement a new `UserControl` in the `/Components` directory.
- **Visuals**: Define the layout in the `.axaml` file.
- **Logic**: Implement behavioral logic in the `.axaml.cs` file. Ensure the component remains generic and avoids direct dependencies on specific ViewModels where possible.

### 2. Register the Component
Add the component to the registry in `Core/Services/DataService.cs` using the `AddComponent` method. This registers the component for search and assigns it to a category:
- **Pinned**: High-priority components.
- **InDevelopment**: Active work-in-progress.
- **Archives**: Legacy or reference components.

### 3. Integrate into Main View
To expose the component in the dashboard detail view:
- **ViewModel State**: Add a selection property in `MainWindowViewModel.cs` (e.g., `IsMyComponentSelected`).
- **Selection Logic**: Update `SelectItem` and `GoToDashboard` to trigger property changes for this new state.
- **View Binding**: Add the component tag to the detail view `StackPanel` in `MainWindow.axaml` with an `IsVisible` binding to the corresponding ViewModel property.

## Design Guidelines
- **Consistency**: Use a standard Border style (SurfaceBackground, 8px CornerRadius, BorderBrush).
- **Typography**: Use SemiBold headers and SecondaryText for descriptions to maintain professional visual hierarchy.
- **Layout**: Maintain standard spacing (10-15px) using StackPanel to ensure a clean, airy interface.
