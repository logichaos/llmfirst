# AGENTS Playbook

## Mission

- Keep every automation pass aligned with the product vision: a todo management experience with a modern web UI and a maintainable .NET backend.
- Translate requests into incremental, well-tested improvements that preserve architectural intent and documentation quality.
- Surface uncertainties early and leave the repository in a better state after each change.

## Project Snapshot

| Area | Path | Notes |
| --- | --- | --- |
| Backend API | `src/backend/Todo.Api/` | ASP.NET Core minimal API targeting `net10.0`, shaped by [docs/adr/20250929-initial-architecture.md](docs/adr/20250929-initial-architecture.md). Expect repository abstractions to keep persistence swappable. |
| Frontend | `src/frontend/` | Vite + React + TypeScript client (scaffolding may not exist yet); design for web first, with future PWA/Electron paths. |
| Documentation | `docs/` | Centralized knowledge base. ADRs capture strategic decisions—consult them before altering architecture. |
| Automation Guidance | `.copilot/instructions.md` | Canonical LLM coding guidelines (clean architecture mindset, testing-first). Keep this document and ADRs in sync when behavior changes. |
| Shared Configuration | `Directory.Packages.props`, `src/Directory.Build.props` | Manage .NET version and shared build settings. Update thoughtfully with corresponding docs. |

## Operating Principles

1. **Honor the architecture.** Follow the clean boundaries described in `.copilot/instructions.md` and the ADR. Keep domain logic isolated from infrastructure concerns and maintain CQRS-style separation when relevant.
2. **Documentation is part of the deliverable.** Reflect meaningful changes in `docs/` (especially ADRs or README-style docs) and note nuances in `AGENTS.md` when they affect automation routines.
3. **Tests before code.** When adding features, sketch (or update) tests first. Mirror structure between production and test directories even if the test project needs scaffolding.
4. **Ship in safe slices.** Prefer small, reviewable changes that can be validated with the existing build and test tooling.
5. **Be explicit about assumptions.** If context is missing, document any temporary assumptions and flag them for follow-up.
6. **Git First.** Commit often with clear messages. Use branches for larger initiatives and rebase to keep history linear.
7. **Continuous Delivery.** Ensure the solution builds and tests pass after each change. If a change breaks the build, fix it before proceeding.

## Cadence for Each Task

- **Prepare**
  - Review the latest ADRs and recent commits touching the same area.
  - Inspect relevant directories to confirm current structure (do not rely on outdated assumptions).
  - Capture a lightweight contract: inputs, outputs, side effects, edge cases.
- **Implement**
  - Follow the Red → Green → Refactor loop.
  - Keep functions small, naming clear, and dependencies injected.
  - Update or add tests alongside code changes.
- **Validate**
  - Run `dotnet restore`, `dotnet build`, and `dotnet test` for touched projects.
  - For frontend work, run the relevant `npm`/`pnpm` commands once the scaffold exists.
- **Document & Signal**
  - Summarize what changed, how it was verified, and any follow-up items.
  - Update this playbook if automation procedures evolve.

## Coordination & Sources of Truth

- **Architecture**: ADRs in `docs/adr/` establish durable decisions—extend or supersede via new ADRs when necessary.
- **Automation Policy**: `.copilot/instructions.md` defines style, tooling expectations, and testing discipline. Treat it as required reading before large refactors.
- **Task Intake**: User prompts are authoritative. If a request conflicts with ADRs or instructions, clarify in the response and propose a reconciled plan.
- **State Tracking**: Use repository issues/notes to capture multi-step initiatives so agents can resume work seamlessly.

## Engineering Standards

### Overview

This todo platform blends an ASP.NET Core minimal API backend with a Vite + React frontend. The objective is to ship a modern web UI backed by maintainable .NET 10 services while keeping documentation and automation in lock-step.

### Code Guidelines

- Prefer simple, clean, and maintainable solutions over clever ones.
- Prioritize readability, self-documenting names, and small, focused functions.
- Adhere to the single responsibility principle across classes and methods.
- Lead with tests: define coverage for the functionality you intend to add before writing production code.
- Keep the Red → Green → Refactor loop at the core of every change.
- Maintain a clean architecture mindset to preserve modularity and testability.

### Architecture & Patterns

- Target clean architecture layering (Domain, Application, Infrastructure, Presentation) as the solution grows.
- Apply CQRS with a mediator to separate reads from writes.
- Preserve repository abstractions and introduce a unit-of-work pattern as persistence evolves.
- Lean on dependency injection to decouple vertical slices and enable testing.

### Technology Stack

- **.NET 10.0** – Primary runtime and SDK for the backend services.
- **Mediator** (martinothamar) – Planned CQRS mediator implementation.
- **xUnit** – Testing framework of choice.
- **FakeItEasy** – Preferred mocking framework for unit tests.
- **FluentResults** and **FluentValidation** – For result handling and validation in the application layer.
- **Serilog** – Structured logging pipeline.

### Coding Standards

- Use PascalCase for classes, methods, properties, and public fields.
- Use camelCase with an underscore prefix for private fields (for example, `_currentUser`).
- Align namespaces with the project folder structure.
- Command and query classes should end with `Command` or `Query`; handlers should end with `Handler`.
- Favor descriptive interface names (avoid the `I` prefix) that convey purpose.
- Organize files by vertical slice, keeping related entities, repositories, commands, handlers, and tests nearby.
- Match file names to the primary class they contain and keep files focused.
- Entities should inherit from a shared base where appropriate, expose private setters, and guard invariants.
- Use data annotations such as `[Required]` and `[MaxLength]` where they reinforce validation at the boundaries.

### Testing Guidelines

- Structure tests using Arrange–Act–Assert with descriptive method names.
- Limit each test to one logical assertion and group related scenarios together.
- Mirror the production project structure within the test projects for easy navigation.
- Cover happy paths, failure modes, and edge cases to build confidence.
- Use FakeItEasy to mock external dependencies while keeping value objects and entities real.
- Validate behavior rather than implementation details.
- Maintain dedicated suites for Domain, Application, Infrastructure, and Presentation tests.

### Development Workflow

```bash
dotnet restore          # Restore dependencies
dotnet build            # Build the solution
dotnet test             # Execute all automated tests
dotnet run --project src/{runnable project here}  # Launch a target project when needed
```

### Git Workflow

- Craft meaningful commits that remain small and focused.
- Prefer conventional commit messages when it clarifies intent.
- Rebase frequently to keep history linear and resolve conflicts early.
- Treat a green test suite as the gate for every commit and pull request.

### Common Tasks

1. **Add a new entity**
   - Create the entity under `src/backend/Domain/Entities/` (or the domain layer equivalent when introduced).
   - Define the repository contract in the domain layer.
   - Implement persistence within the infrastructure layer.
   - Introduce matching CQRS operations and handlers.
   - Add comprehensive tests covering the new behavior.
2. **Add a new command or query**
   - Create the request type in the appropriate application folder.
   - Implement the handler via the mediator pattern.
   - Register the handler with dependency injection.
   - Layer in validation, error handling, and tests for success and failure cases.

### Error Handling

- Reserve exceptions for exceptional situations rather than control flow.
- Validate inputs at application boundaries and return meaningful errors.
- Use the Result pattern to model recoverable failures.
- Capture and log errors with sufficient context for troubleshooting.

### Performance Considerations

- Apply `async`/`await` for I/O-bound operations.
- Cache expensive or frequently requested data when it improves responsiveness.
- Profile critical paths before optimizing and document findings.

### Dependencies and Extensions

- Manage NuGet dependencies centrally and prefer packages compatible with .NET 10.
- Evaluate the impact of new dependencies on startup, runtime performance, and deployment.
- Wire new services through dependency injection with clear lifetimes.
- Update manifests and documentation whenever dependencies change.

### Documentation

- Update `README.md` or feature-specific docs for notable changes.
- Provide XML comments or usage notes for public APIs when they aid discoverability.
- Add examples for complex features to reduce onboarding time.
- Keep ADRs current by recording significant architectural decisions promptly.

## Ready-to-Run Commands

```bash
# Restore and build the .NET solution
 dotnet restore
 dotnet build

# Execute all tests (add filters for targeted runs when projects appear)
 dotnet test
```

> ℹ️ When the frontend scaffold lands, prefer the project-local package manager (likely `npm` or `pnpm`) and document new scripts here.

## Quality Gates Checklist

- [ ] Requirements mapped and answered explicitly.
- [ ] Tests covering new behavior (or rationale documented if none were added).
- [ ] `dotnet build` + `dotnet test` (and frontend equivalents) succeed or failures are explained with mitigation steps.
- [ ] Docs updated for any user-facing change.
- [ ] Follow-up items logged or noted when work is intentionally incomplete.

## When to Update This Document

- New automation workflows or conventions emerge.
- The architecture shifts (new layers, frameworks, or hosting targets).
- Shared tooling commands change (e.g., custom build scripts, container workflows).
- Repeated friction points appear that future agents should anticipate.

Staying synchronized through this playbook keeps every autonomous contribution coherent, verifiable, and easy to extend.
