# Project — Claude Code Configuration

<!-- 
  This file is the quick-reference entry point for Claude.
  Fill in the sections below with your project's specifics.
  Agents and commands read this file FIRST before any context docs.

  AUTO-GENERATION: If this file is empty or missing when /setup or /ticket
  is run, the codebase will be scanned ONCE to generate this file and all
  context docs (.claude/context/*). After that, no further scans are needed.
-->

## Project
<!-- One-liner: What the project is, who it's for, and what it does. -->
Study Quest is a React Native + ASP.NET Core mobile app helping West African secondary school students (Grades 10-12) manage their studies with AI-powered features.

## Quick Reference

### Dependency Map
<!-- List every dependency manifest, its directory, and install command. -->
| Directory | Manifest | Install Command |
|-----------|----------|-----------------|
| `./` | `package.json` | `npm install` |
| `backend/StudyQuest.API/` | `StudyQuest.API.csproj` | `dotnet restore` |

### Build & Run
<!-- List the commands to build and run all parts of the project. Include working directories. -->
```bash
# Backend
cd backend/StudyQuest.API
dotnet build
dotnet run

# Frontend
npm install
npx expo start

# Database
docker compose up -d          # Start PostgreSQL
dotnet ef database update      # Apply migrations
dotnet ef migrations add Name  # New migration
```

### Key Paths
<!-- List the most important entry points and directories. -->
- Backend entry: `backend/StudyQuest.API/Program.cs`
- Database context: `backend/StudyQuest.API/Data/AppDbContext.cs`
- Frontend entry: `App.tsx`
- API client: `src/shared/api/client.ts`
- Theme: `src/shared/theme/`

### Architecture Rules
<!-- Summarize the key patterns and constraints. -->
- Backend: Vertical Slices with MediatR CQRS, ErrorOr results, Minimal APIs
- Frontend: Feature folders, React Context state, React Native Paper UI
- All endpoints authenticated except auth + health
- Entity IDs are GUIDs
- No `any` types, no console.log in production

## Agents & Commands
See `.claude/agents/` and `.claude/commands/` for automated workflows.
See `.claude/policies/engineering.md` for coding standards.
See `.claude/context/` for API, architecture, and database reference docs.