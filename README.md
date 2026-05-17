# RigorStarter C# Avalonia

A sample application built with Avalonia UI and C#, demonstrating a clean implementation of the MVVM pattern.

## Features

- **Custom Accordion Component**: A reusable UI component that displays collapsible sections.
- **Dynamic Filtering**: A search functionality that filters accordion items in real-time based on header or content.
- **MVVM Architecture**: Built using the `CommunityToolkit.Mvvm` library for observable properties and relay commands.
- **Cross-Platform**: Developed with Avalonia UI for compatibility across multiple operating systems.

## Tech Stack

- **Language**: C#
- **Framework**: .NET 10.0
- **UI Framework**: Avalonia UI (v11.0.10)
- **MVVM Toolkit**: CommunityToolkit.Mvvm (v8.2.2)

## Getting Started

### Prerequisites

Ensure you have the following installed:
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### Running the Application

The project includes a convenience script to handle cleaning, building, and launching the app.

```bash
chmod +x run.sh
./run.sh
```

Alternatively, you can use the dotnet CLI:

```bash
dotnet build
dotnet run
```

## Project Structure

- `Components/`: Custom Avalonia controls (e.g., `Accordion`).
- `ViewModels/`: Application logic and state management.
- `Views/`: XAML files defining the user interface.
- `App.axaml`: Global application styling and configuration.
