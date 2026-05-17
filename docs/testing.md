# Testing Framework

The project uses a rigorous testing suite based on xUnit to ensure stability across different system environments.

## Test Categories

### ViewModel Tests
Focuses on the logic within `MainWindowViewModel`. 
- Search filtering (case-insensitivity, empty results).
- State transitions (selecting items, returning to dashboard).

### Utility Tests
Focuses on the `Utilities` directory.
- Validates that system calls return non-empty summaries.
- Ensures that `UtilityResult` objects are correctly formed.
- Checks for resilience against missing system data.

### Converter Tests
Validates that `IValueConverter` implementations map data types to the correct Avalonia Brushes.

### Data Service Tests
Tests the `ViewModelDataService` for correct component initialization and file line counting.

## How to Run Tests

Execute the following command from the project root:

```bash
dotnet test tests/RigorStarter.Tests/RigorStarter.Tests.csproj
```

## Writing New Tests
When adding a new utility or ViewModel property, add a corresponding test case in the `tests/RigorStarter.Tests` directory. Focus on edge cases such as null inputs, empty strings, and unexpected OS responses.
