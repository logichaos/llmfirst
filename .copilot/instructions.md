# {project title} Copilot instructions

## Overview of project

This is an overview of the project and what it 1. 

## Directory layout

- root
  - src -> all source code for the application
  - docs -> documentation
  - .github -> github workflows
  - test -> all tests
  - build -> scripts for building
  - artifacts -> artifact output for build scripts or CI/CD

## Code guidelines

- we prefer simple, clean, maintainable solutions over clever or complex ones
- Readability and maintainability are primary concerns
- self-documenting names and code
- Small functions
- Follow single responsibility principle in classes and methods
- Write the tests for what you want to implement first, then implement the code to make the tests pass
- For each functionality, make sure to have tests that cover this functionality before starting to implement that functionality
- no PR before the tests are succeeding
- Red -> Green -> Refactor
  - Start with the specification of what needs to be done
  - Implement the functionality
  - Refactor the code until it is SOLID
- use clean architecture to help make components modular, changeable, and testable

## Architecture & Patterns

### Clean Architecture Layers

- **Domain** (`src/Domain/`): Core business logic, entities, value objects, and interfaces
- **Application** (`src/Application/`): CQRS commands/queries with Mediator pattern
- **Infrastructure** (`src/Infrastructure/`): Data persistence and external services
- **Presentation** (`src/Presentation/`): Presentation layers and API projects

### Key Design Patterns

- **CQRS** with Mediator pattern for command/query separation
- **Repository Pattern** with Unit of Work for data access
- **Dependency Injection**

## Technology Stack

- **.NET 9.0** - Primary runtime and SDK
- **Mediator** - martinothamar's Mediator for CQRS implementation
- **xUnit** - Testing framework
- **FakeItEasy** - Mocking framework for unit tests
- **Microsoft.Extensions.DependencyInjection** - DI container
- **FluentResults** - For Result pattern implementation
- **FluentValidation** - For validating commands and inputs
- **Serilog** - For logging

## Coding Standards

### Naming Conventions

- Use PascalCase for classes, methods, properties, and public fields
- Use camelCase for private fields with underscore prefix (`_fieldName`)
- Use PascalCase for namespaces following project structure
- Command/Query classes should end with "Command" or "Query"
- Handler classes should end with "Handler"
- Do not start Interfaces with "I", use names that describe the role/functionality instead

### File Organization

- use vertical slices for features.
- Put related files (entity, repository, commands, handlers, tests) in the same folder or very close to each other
- Match file names to class names if only one class is in the file
- Group related functionality in appropriate folders
- Follow the established folder structure within each layer

### Entity Design

- Entities should inherit from `BaseEntity`
- Use private setters with public methods for state changes
- Include validation in entity methods (throw exceptions for invalid state)
- Call `SetUpdatedAt()` when modifying entity state
- Use `[Required]` and `[MaxLength]` attributes for validation

### CQRS Implementation

- Commands modify state, Queries read state
- Each command/query should have a corresponding handler
- Handlers should be thin and delegate to domain services or repositories
- Use the Mediator pattern for decoupling

## Testing Guidelines

### Test Structure

- Follow Arrange-Act-Assert pattern
- Use descriptive test method names describing the scenario
- One logical assertion per test
- Group related tests in the same test class
- Use xUnit's `[Fact]` and `[Theory]` attributes appropriately
- Organize tests in folders mirroring the main project structure
- ensure high code coverage, especially for domain and application layers
- ensure good, bad, and edge cases are tested

### Mocking

- Use FakeItEasy for creating test doubles
- Mock external dependencies and repositories
- Don't mock value objects or entities
- Verify behavior, not just state

### Test Categories

- **Domain Tests**: Entity behavior, business rules, edge cases
- **Application Tests**: CQRS handlers with mocked dependencies
- **Infrastructure Tests**: Repository implementations, data persistence
- **Presentation Tests**: UI component behavior

## Development Workflow

### Building and Testing

```bash
dotnet restore          # Restore dependencies
dotnet build           # Build solution
dotnet test            # Run all tests
dotnet run --project src/{runnable project here} # Run the application
```

### Git Workflow

- Use meaningful commit messages
- Follow conventional commit format when possible
- Keep commits atomic and focused
- Ensure all tests pass before committing

## Common Tasks

### Adding New Entities

1. Create entity class in `src/Domain/Entities/`
2. Add repository interface in `src/Domain/Interfaces/`
3. Implement repository in `src/Infrastructure/Persistence/`
4. Create corresponding CQRS operations
5. Write comprehensive tests

### Adding CQRS Operations

1. Create command/query in appropriate Application folder
2. Create handler implementing IRequestHandler
3. Register handler in DI container
4. Add validation and error handling
5. Write handler tests with mocked dependencies

## Error Handling

- Use exceptions for exceptional cases, not control flow
- Validate inputs at application boundaries
- Use Result pattern for operations that can fail gracefully
- Log errors appropriately for debugging
- Provide meaningful error messages to users

## Performance Considerations

- Use async/await for I/O operations
- Cache expensive operations when appropriate
- Profile and optimize hot paths

## Dependencies and Extensions

### When Adding Dependencies

- Use central package management
- Prefer packages that support .NET 9.0
- Consider cross-platform compatibility
- Evaluate impact on application startup time
- Update project files and documentation
- Ensure proper dependency injection setup

## Documentation

- Update README.md for significant changes
- Document public APIs with XML comments
- Include usage examples for complex features
- Update help system for new shortcuts or features
- Maintain architectural documentation for major changes

