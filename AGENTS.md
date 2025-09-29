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

## Cadence for Each Task

- **Prepare**
  - Review the latest ADRs and recent commits touching the same area.
  - Inspect relevant directories to confirm current structure (do not rely on outdated assumptions).
  - Capture a lightweight contract: inputs, outputs, side effects, edge cases.
- **Implement**
  - Follow the Red → Green → Refactor loop from `.copilot/instructions.md`.
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
