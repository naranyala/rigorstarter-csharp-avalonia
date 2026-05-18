# Frontend Development Rules & Guidelines

To maintain scalability and prevent "Shotgun Surgery," RigorStarter follows a **Registry-based Architecture**. Adding a new component should require minimal changes to existing core logic.

## 🛠 1. Adding a New Component (The Correct Way)

When creating a new reusable component (e.g., `MyNewComponent`):

1.  **Create the Files**: 
    - Create `Components/MyNewComponent.axaml` for the layout.
    - Create `Components/MyNewComponent.axaml.cs` for the code-behind.
2.  **Create the ViewModel**:
    - Create `ViewModels/MyNewComponentViewModel.cs` (if the component requires its own state).
3.  **Register the Component**:
    - Add the component to the `ComponentRegistry` (or equivalent service).
    - **Do NOT** add manual `IsSelected` properties to `MainWindowViewModel`.
    - **Do NOT** add manual `OnPropertyChanged` calls to `MainWindowViewModel`.
4.  **Define the Data Template**:
    - Ensure a `DataTemplate` exists in `App.axaml` or `MainWindow.axaml` that maps `MyNewComponentViewModel` to `MyNewComponent.axaml`.

## 📺 2. Viewing Components

The `MainWindow` uses a single `ContentControl` to display the selected item. 

- The `MainWindowViewModel` manages a single `SelectedItem` property.
- The `MainWindow.axaml` binds the `Content` of the detail area to this `SelectedItem`.
- Avalonia's `DataTemplate` system automatically resolves the correct View for the selected ViewModel.

## 🎨 3. UI & Styling Consistency

To keep the "RigorStarter" look and feel:

- **Containers**: Use `Border` for cards/panels.
- **Standard Card Style**: 
  - `Background="White"`
  - `CornerRadius="8"`
  - `Padding="15"`
  - `BorderBrush="#DDD"`
  - `BorderThickness="1"`
- **Spacing**: Use `Spacing="10"` or `Spacing="15"` on `StackPanel` instead of individual margins where possible.
- **Typography**: 
  - Headers: `FontWeight="Bold"`, `FontSize="18"`
  - Secondary text: `Foreground="Gray"`, `FontSize="12"`

## ⚠️ 4. Prohibited Patterns (Anti-Patterns)

| Anti-Pattern | Why it's bad | Correct Approach |
| :--- | :--- | :--- |
| **Manual Visibility Properties** | Leads to "Shotgun Surgery" in `MainWindowViewModel`. | Use `ContentControl` with `DataTemplates`. |
| **Manual Property Notifications** | Error-prone and requires updating multiple methods. | Rely on the `SelectedItem` change notification. |
| **Hardcoded Component Lists** | Makes `DataService` and `MainWindowViewModel` brittle. | Use a Registry or Reflection-based discovery. |
| **ElementName Binding in Detail View** | Breaks encapsulation and creates tight coupling. | Use standard ViewModel-to-View bindings. |

## 🔍 5. Debugging Checklist

If a component isn't rendering:
1. [ ] Is it registered in the component registry?
2. [ ] Does a `DataTemplate` exist for its ViewModel?
3. [ ] Is the `SelectedItem` being correctly updated in `MainWindowViewModel`?
4. [ ] Are there any binding errors in the Avalonia logs?
