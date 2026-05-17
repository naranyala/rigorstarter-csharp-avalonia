# Component Development Guide

This document outlines the process for adding new UI components to the project. For a quick-start guide on implementation, refer to RULES.md.

## Implementation Workflow

### 1. Creation
Create a new UserControl in the /Components directory.
- Define the visual structure in .axaml.
- Implement any necessary logic in .axaml.cs.

### 2. Registration
Register the component in `ViewModelDataService.cs` using the `AddComponent` method. This ensures the component appears in the search results and is categorized correctly (Pinned, In Development, or Archive).

### 3. Integration
To make the component visible in the detail view:
- Add a selection property in `MainWindowViewModel.cs` (e.g., `IsMyComponentSelected`).
- Update `SelectItem` and `GoToDashboard` to notify property changes for this property.
- Add the component tag to the detail view StackPanel in `MainWindow.axaml` with an `IsVisible` binding.

## Design Standards
- Use a consistent Border style (White background, 8px CornerRadius, #DDD Border).
- Maintain standard spacing (10-15px) using StackPanel.
- Use semi-bold headers and gray secondary text for a clean, professional look.
