# SCARS.Core

**SCARS.Core** is a foundational library for the SCARS framework that focuses on clean, testable, and logic-separated code. SCARS encourages real data usage for testing to avoid the performance overhead and inconsistencies introduced by mocks.

The core components include:
1. **Architecture Rules**: Enforce design patterns to maintain clean architecture.
2. **Attributes**: Used to enforce separation of logic and glue, ensure services are used correctly, and prevent mocks.
3. **Storage**: A key component that provides mock data to be used with real services, making it easier to test services with data that behaves consistently.

## Key Features

### Architecture Rules
SCARS enforces architectural rules through Roslyn analyzers and custom attributes. These rules ensure that services are designed with separation of concerns in mind:
- **[Glue]**: Services that wire other services together without adding business logic.
- **[Logic]**: Services that contain the actual business logic and should have minimal dependencies.
- **[Unmockable]**: A marker attribute to prevent mocking of certain services in tests. Forces the use of real services or mock data.

### Attributes
SCARS uses attributes to:
- Mark classes and methods with specific roles like `Glue` and `Logic`.
- Enforce rules about which services can be mocked and which can't.
- Keep the service architecture clean, reducing technical debt over time.

### Storage
The **Storage** component provides an abstraction for storing and retrieving data. It includes:
- **IDataStorage<T>**: A simple interface for storing and retrieving data.
- **MemoryStorage<T>** and **FileStorage<T>**: Implementations that store data either in memory or in files.
  
Storage is key to SCARS because it allows the use of mock data passed to real services, enabling testing without the need for mock services. This mimics real-world data interactions and prevents over-reliance on mocks.

### Benefits of SCARS
- **Prevent Mocks**: SCARS makes it harder to mock services, encouraging the use of real services or mock data that behave like real data.
- **Cleaner Code**: Through analyzers and attributes, SCARS ensures that your code maintains separation of concerns and enforces good architectural practices.
- **Easier Testing**: By using Storage to provide mock data to real services, testing becomes more reliable and representative of production data.

## Getting Started

### Install SCARS.Core

Add SCARS.Core to your project via NuGet:

```bash
dotnet add package SCARS.Core
