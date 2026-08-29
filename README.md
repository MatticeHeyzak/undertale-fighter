# Undertale Fighter

A learning-focused, single-boss battle game inspired by turn-based bullet-hell
combat systems.

## Architecture

- `UndertaleBattle.Core`
  - Deterministic gameplay simulation.
  - Battle state machine, runtime state, attacks, movement, projectiles,
    collision, and arena behavior.
  - No Raylib dependency.

- `UndertaleBattle.App`
  - Raylib window, input polling, fixed-step loop, assets, and rendering.
  - Renders to a fixed virtual resolution and scales it to the physical window.

## Runtime flow

```text
Menu / Dialogue / QTE
  → Enemy turn
  → Select a fresh attack
  → Player dodge phase
  → Menu