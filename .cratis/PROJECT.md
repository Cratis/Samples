# Samples — project context

Runnable event sourcing and CQRS samples for the Cratis stack — from a small
Chronicle event-sourcing process to a React application composed with Arc,
Components, and Aspire. Covers the Chronicle event store, Arc CQRS for
ASP.NET Core, React Components, the CLI, the Workbench, and the model-first
Screenplay and Stage (experimental).

## Conventions

- Samples compile against released packages (not local source) so they show
  consumers the real experience.
- Each sample keeps its own README with run instructions; when a released
  package changes in a way that breaks a sample, fixing the sample is part
  of the release.

## AI-assisted development

This repository uses the Cratis AI contract:

- **`.cratis/ai.json`** records the subscription — `cratis/documentation` plus the `cratis/engineering/csharp` and `cratis/engineering/typescript` maintainer cells for the mixed sample stacks.
- **`.cratis/PROJECT.md`** (this file) is the canonical project context; the root `AGENTS.md`, `CLAUDE.md`, and `GEMINI.md` are minimal bootstraps that point here and do nothing else.
- There is **no local AI corpus and no generated tool adapters** in this repository. Shared skills arrive through the Cratis AI marketplace plugins (Claude Code, Codex, GitHub Copilot, Cursor, and Pi are installable today — see the [harness guide](https://www.cratis.io/ai/harnesses/)).

For contributors:

1. Install the Cratis plugin for your harness once (per the harness guide); the subscribed profiles' skills then load automatically when tasks match.
2. General, reusable improvements are proposed in [`Cratis/AI`](https://github.com/Cratis/AI) — never copied into, or synchronized from, this repository.
3. Repository-specific facts and conventions belong in this file; repository-local skills live under `.agents/skills/`.
4. AI session work records (plans, handovers, session notes, scratch analyses) stay in the untracked `.ai-work/` folder and never enter git; a durable follow-up becomes a GitHub issue.
