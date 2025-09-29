# ADR 2025-09-29: Initial architecture

## Status

Accepted

## Context

We need a foundation for a simple todo management application with a modern web UI and a maintainable backend. The backend should leverage .NET 10 to stay aligned with upcoming platform capabilities. The frontend should start as a standard browser app, but we want to keep the door open for progressive web app (PWA) enhancements or an Electron-based desktop client later. The existing repository expects backend and frontend assets under `src/backend` and `src/frontend`, and documentation is centralized in `docs/`.

## Decision

- Create an ASP.NET Core minimal API project targeting `net10.0` under `src/backend`, exposing REST endpoints for todo items and designed with a repository abstraction so the persistence layer can evolve beyond in-memory storage.
- Create a Vite + React + TypeScript frontend under `src/frontend` that consumes the backend API through a lightweight client wrapper, organized to support future platform targets (PWA/Electron) with minimal refactoring.
- Capture shared architectural knowledge, API contracts, and future migration notes in `docs/` to keep LLM-assisted workflows grounded.

## Consequences

- We stay on the leading edge of the .NET platform, potentially consuming preview SDKs; CI/CD must ensure the proper SDK is available.
- An abstraction layer around persistence incurs a small upfront cost but eases future adoption of durable storage (e.g., EF Core, Cosmos DB).
- Using Vite gives us fast iteration today and built-in pathways for PWA manifests, service workers, or Electron preload scripts later.
- Documentation discipline keeps humans and agents aligned, especially when new automations or contributors join the project.
