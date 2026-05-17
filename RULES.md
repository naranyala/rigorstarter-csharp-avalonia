# Frontend Development Rules & Guidelines

Writing UI in Avalonia with MVVM can be tricky. Follow these rules to avoid common "surprises" and ensure components render correctly.

## 🛠 1. Adding a New Component
When creating a new reusable component (e.g., `MyNewComponent`):

1.  **Create the Files**: 
    - Create `Components/MyNewComponent.axaml` for the layout.
    - Create `Components/MyNewComponent.axaml.cs` for the code-behind.
2.  **Register in Data Service**:
    - Add the component to `ViewModels/ViewModelDataService.cs` using `AddComponent()`.
    - If it's a real implementation, set `isMockup = false`.
    - If it's just a placeholder, set `isMockup = true`.

## 📺 2. Integrating Components into the Main View
Simply creating a component and adding it to the data service is **not enough** to make it appear in the detail view. You must perform these three steps:

### Step A: ViewModel Property
In `ViewModels/MainWindowViewModel.cs`, add a boolean property to track if this specific component is selected:
```csharp
public bool IsMyNewComponentSelected => SelectedItem?.Name == "MyNewComponent";
```

### Step B: Trigger Property Notifications
In `ViewModels/MainWindowViewModel.cs`, you **must** call `OnPropertyChanged` for the new property in both `SelectItem` and `GoToDashboard` methods:
```csharp
OnPropertyChanged(nameof(IsMyNewComponentSelected));
```
*Failure to do this will result in the UI not updating when you click the item in the search bar.*

### Step C: XAML Integration
In `Views/MainWindow.axaml`, add the component to the "Item Detail View" `StackPanel` and bind its visibility:
```xml
<comp:MyNewComponent IsVisible="{Binding IsMyNewComponentSelected}" />
```

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

## ⚠️ 4. Common Pitfalls ("The Surprises")

| Surprise | Cause | Fix |
| :--- | :--- | :--- |
| **Component not appearing** | Missing from `MainWindow.axaml` | Check Step 2C above. |
| **Clicking item does nothing** | Missing `OnPropertyChanged` | Check Step 2B above. |
| **UI not updating** | Property not `virtual` or missing notification | Ensure `OnPropertyChanged` is called for computed properties. |
| **Binding Error in logs** | Incorrect namespace or property name | Verify `xmlns:comp="using:RigorStarter.Components"` is present. |
| **Layout is squashed** | `StackPanel` inside a `Grid` without constraints | Use `HorizontalAlignment="Stretch"` or explicit `Width`/`Height`. |

## 🔍 5. Debugging Checklist
If a component isn't rendering:
1. [ ] Is it registered in `ViewModelDataService`?
2. [ ] Does `MainWindowViewModel` have an `Is...Selected` property?
3. [ ] Does `SelectItem` call `OnPropertyChanged` for that property?
4. [ ] Is the `<comp:... />` tag present in `MainWindow.axaml`?
5. [ ] Is the `IsVisible` binding pointing to the correct property?
