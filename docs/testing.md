# Testing Framework

RigorStarter employs a comprehensive testing strategy using xUnit to ensure system stability and correctness across various operating system environments.

## Test Suite Structure

### 1. ViewModel Tests
Validates the presentation logic and state transitions.
- **Search Filtering**: Tests case-insensitivity, partial matches, and empty result handling.
- **Navigation State**: Verifies that selecting components and returning to the dashboard updates the UI state properties correctly.
- **Feature Toggles**: Validates the theme switching logic and state persistence.

### 2. Core Service Tests
Validates the business logic and dependency injection.
- **ServiceProvider**: Ensures that all required services are correctly registered and retrieved as the appropriate types.
- **DataService**: Verifies the correct initialization of the component registry and the accuracy of file line counting.

### 3. System Utility Tests
Tests the lowest level of the application.
- **Output Validation**: Ensures that OS-level calls return non-null, formatted summaries.
- **Resilience**: Checks that the utilities handle missing system data or unexpected OS responses without crashing.

### 4. Converter Tests
Validates the mapping of data states to Avalonia UI properties.
- Ensures that `BadgeStatus` values map to the correct `IBrush` colors.
- Validates that boolean states map to correct borders and colors for success/failure states.

## Execution

Run the complete test suite from the project root:

```bash
dotnet test tests/RigorStarter.Tests/RigorStarter.Tests.csproj
```

## Guidelines for New Tests
When introducing a new feature or utility:
1. Create a corresponding test class in `tests/RigorStarter.Tests`.
2. Focus on edge cases: null inputs, empty strings, and unexpected system responses.
3. Use `[Theory]` for data-driven tests to cover multiple input scenarios in a single method.
