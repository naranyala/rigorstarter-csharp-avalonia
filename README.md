# RigorStarter Avalonia

RigorStarter is a professional component showcase and system utility dashboard built with C# and the Avalonia UI framework. It demonstrates a scalable, modular architecture for building reusable UI components and integrating system-level diagnostics.

## Features

- Component Dashboard: A searchable registry of UI components with live previews and source code access.
- System Utilities: Integrated tools for monitoring CPU, Memory, Disk, and Network status.
- Layered Architecture: Strict separation of concerns using a Core/Shared/Presentation model and a lightweight Dependency Injection system.
- Comprehensive Testing: Full xUnit test suite covering utilities, business logic, and UI state transitions.

## Project Structure

- /Core: Business logic, service interfaces, and the DI container.
- /Shared: Low-level OS utilities and common data models.
- /ViewModels: Application state and presentation logic.
- /Views: Avalonia XAML views.
- /Components: Reusable UI controls.
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
