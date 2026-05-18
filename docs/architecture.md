# Project Architecture

RigorStarter is a professional showcase application built using C# and the Avalonia UI framework, following a strict Model-View-ViewModel (MVVM) pattern and a layered architecture.

## Architectural Layers

The application is divided into three primary layers to ensure scalability, maintainability, and testability.

### 1. Presentation Layer
This layer handles the user interface and user experience.
- **Views**: XAML-based layouts defining the visual structure.
- **ViewModels**: Logic that bridges the Views and the Core layer. It manages UI state and handles user commands.
- **Converters**: Logic to map data states to visual properties (e.g., status to color).

### 2. Core Layer
The brain of the application, containing business logic and orchestration.
- **Interfaces**: Defines the contracts for services (e.g., IDataService, ISystemService), allowing for easy swapping of implementations.
- **Services**: Implementation of business logic and service coordination.
- **ServiceProvider**: A lightweight dependency injection (DI) container that manages singleton lifetimes and provides services to the ViewModels.

### 3. Shared Layer
Cross-cutting concerns and low-level utilities used across the entire application.
- **Utilities**: OS-level diagnostic tools that interact with the host system to gather hardware and software information.
- **Models**: Common data structures (e.g., UtilityResult) used for communication between layers.

## Dependency Rule
To prevent circular dependencies and maintain a clean architecture, the following rules are enforced:
Presentation $\rightarrow$ Core $\rightarrow$ Shared.
The Shared layer must never depend on Core or Presentation.

## Data Flow
1. **Initialization**: The `ServiceProvider` instantiates services and the `MainWindowViewModel`.
2. **Data Loading**: `DataService` populates the component and utility registries.
3. **Interaction**: The user interacts with the View $\rightarrow$ triggers a Command in the ViewModel $\rightarrow$ calls a method in a Core Service $\rightarrow$ utilizes a Shared Utility.
4. **Feedback**: Results flow back through the ViewModel to the View via data binding.
