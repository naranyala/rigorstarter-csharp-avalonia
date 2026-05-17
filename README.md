# RigorStarter Avalonia

RigorStarter is a professional component showcase and system utility dashboard built with C# and the Avalonia UI framework. It demonstrates a modular architecture for building reusable UI components and integrating system-level diagnostics.

## Features

- Component Dashboard: A searchable registry of UI components with live previews.
- System Utilities: Integrated tools for monitoring CPU, Memory, Disk, and Network status.
- MVVM Architecture: Strict separation of concerns using the CommunityToolkit.Mvvm.
- Rigorous Testing: Comprehensive xUnit test suite covering utilities and business logic.

## Project Structure

- /Components: Reusable UI controls.
- /ViewModels: Application state and business logic.
- /Views: Avalonia XAML views.
- /Utilities: OS-level diagnostic tools.
- /docs: Technical documentation.
- /tests: Test suite.

## Getting Started

### Prerequisites
- .NET 10.0 SDK
- Avalonia UI environment

### Installation and Execution
To build and launch the application, run the provided shell script:

```bash
bash run.sh
```

### Running Tests
To execute the test suite and verify system stability:

```bash
dotnet test tests/RigorStarter.Tests/RigorStarter.Tests.csproj
```

## Documentation
Detailed technical guides are available in the /docs directory:
- Architecture: docs/architecture.md
- Component Guide: docs/components.md
- Testing Guide: docs/testing.md
