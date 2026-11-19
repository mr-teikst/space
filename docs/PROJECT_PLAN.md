# HW3: Multi-Stage Chicken Shooter — implementation plan

Prepared 15 November 2026. This is the original planning record. Implementation is now complete; see [README](../README.md), [architecture](ARCHITECTURE.md), and [validation](VALIDATION.md) for the delivered state.

## Basis and current project

The requirements below come from [the assignment PDF](/home/erfant/Downloads/HW3/HW3.pdf), PDF pages 2–6. Suggested architecture, round layouts, estimates, and unspecified gameplay policies are implementation decisions, not additional assignment requirements.

At planning time the repository contained the Unity 2D URP template, `SampleScene`, and template input actions. Its recorded editor is **6000.6.6f1**, with Input System **1.20.0**, URP **17.6.0**, and Unity Test Framework already declared. New Input System is already selected. Keep these project versions unless an actual compatibility issue appears.

## Required final result

- Four separate scenes: `MainMenu`, `Level1`, `Level2`, and `Level3`, all included in the build scene list.
- Stage lengths: **5, 7, and 10 rounds**. Treat the last round of each stage as its boss round, consistent with the detailed boss requirement; do not accidentally add a 6th, 8th, or 11th round.
- Earth-orbit basic chickens in grid formations; Jupiter/asteroid-belt armored chickens in V formations; deep-space cosmic chickens in tighter, faster formations. Each stage needs distinct visuals and behavior.
- Curved entry paths, destructible eggs, non-shooting dive attackers, and one or two destructible-asteroid rounds in every stage.
- Three bosses, each using an explicit FSM with at least three attack phases, including an enraged phase below 30% health with fast attacks and summoned chickens.
- WASD/arrows movement clamped to screen bounds, Space tap/hold firing, and X secondary fire, all through Input Actions.
- Three starting lives, temporary invulnerability and blinking after damage, and loss of one weapon level per hit, clamped to level 1.
- Three weapon patterns: single shot, parallel double shot, and triple spread; gifts increase power up to level 3.
- Two starting screen-clearing missiles. Each clears normal chickens, boss summons, and eggs, but deals only 15–20% of a boss's maximum health as damage.
- Falling food gives score and fills a meter. A full meter awards one missile and resets.
- JSON checkpoints, cross-scene player state, menus, complete HUD, stage transitions, and final victory with the save reset.
- Required architecture: `LevelDataSO`, `WaveDataSO`, `WeaponDataSO`, object pooling, and event-driven communication.
- Parallax backgrounds; exhaust, feathers, broken eggs, and missile shockwave effects; screen shake, laser bloom, and enemy damage flashes. Separate stage music, weapons/explosions, chicken sounds, and pickup sounds.
- Final delivery: Git repository link, Windows x86_64 build, and illustrated architecture report inside `GD_Hw3_[firstName]_[lastName]_[StudentId].zip`.

## Implementation order

The estimates are planning ranges for one developer already comfortable with Unity. They exclude learning time and waiting for art. Total: approximately **40–60 focused hours**; progress by the acceptance gates below rather than calendar dates.

### 1. Project foundation and asset selection — 3–4 hours

- Keep the existing 2D URP setup; create the four required scenes and register them in the build scene list.
- Set up project folders, 2D physics layers, sorting layers, camera/playfield bounds, and an Input Actions asset with Move, Fire, Missile, Pause, and UI actions.
- Select a consistent art direction using [ASSETS.md](ASSETS.md). Import only selected art/audio and preserve license files. Use simple placeholders where adaptations are still needed.
- Build a Windows smoke-test early to check that Windows build support is available on the development machine.

**Gate:** MainMenu can load each scene by name; movement/input works in a Windows build; selected sources are recorded.

### 2. One playable combat round — 6–8 hours

- Implement player movement, health, invulnerability, three weapon patterns, gift upgrades, and the two-missile starting loadout.
- Add reusable pools for player bullets, eggs, food, gifts, asteroids, and effects. Pool reset must clear health, velocity, timers, subscriptions, and prior hit state.
- Add a basic chicken, destructible eggs, asteroid health, collision filtering, score, food progress, and missile effects.
- Give each projectile a single-hit guard so simultaneous trigger callbacks cannot damage or score twice.
- Add a minimal event-driven HUD for lives, score, power, missiles, food, and the current round.

**Gate:** one round demonstrates all three firing patterns, damage/invulnerability, upgrades, food-to-missile conversion, destructible eggs, and pooled reuse without duplicate rewards.

### 3. Persistent state and checkpoint policy — 5–7 hours

- Implement a duplicate-safe `GameManager` root using `DontDestroyOnLoad`; it owns runtime player/run state, not scene-specific object references.
- Implement a plain serializable save record and `SaveService` using `JsonUtility` and `Application.persistentDataPath/savegame.json`.
- Add New Game and Continue. Continue validates the record, restores the manager first, then loads the saved scene and starts the saved round once.
- At each round boundary, advance the round index and write its starting checkpoint before spawning enemies. After a stage boss, checkpoint the next stage's round 1 before loading it.
- On final life loss, stop progression and **immediately overwrite** the checkpoint to round 1 of the current stage, with a retry-ready life count of 3, before showing Game Over. Returning to menu or closing the game must preserve this penalty.
- Implement retry, pause/resume, and exit to MainMenu. Pause has no save or restart-round button. Restore normal time scale and correct action maps when leaving pause.
- Add malformed-save handling and write via a temporary file followed by replacement; never silently start a different stage from invalid indices.

**Gate:** closing/reopening resumes the correct checkpoint with all recorded fields; dying in a later round and quitting from Game Over still resumes round 1 of that same stage with three lives.

### 4. Data-driven rounds and enemy behavior — 6–9 hours

- Implement `LevelDataSO`, `WaveDataSO`, and `WeaponDataSO` with Inspector-editable references and tuning values.
- Build a `LevelController`/`WaveSpawner` that reads level data, tracks pending spawns and unresolved enemies, and advances exactly once when a round finishes.
- Use curved waypoint paths and grid, V, and tight formations. Implement explicit enemy states for entry, formation, dive, and removal. Diving chickens must stop laying eggs.
- Treat offscreen exits as resolved wave participants without awarding kill rewards. Prevent late entry spawns or pooled disable callbacks from prematurely completing a wave.
- Configure all stages using reusable wave templates and per-round modifiers. Keep difficulty changes in data rather than stage-specific conditionals.

**Gate:** all non-boss rounds can run in order; every stage contains asteroid rounds; adding/tuning a wave requires no core-code edits.

### 5. Bosses and complete campaign — 7–11 hours

- Build one reusable boss FSM and three distinct boss prefabs/configurations.
- Use states such as Entry, Egg Barrage, Telegraph/Dive with Energy Feathers, Enraged Summoning, and Dead. Entry and Dead do not count toward the three attacking phases.
- Give all bosses at least three attacking phases; select their attack rates, health, movement, projectile patterns, and summon types per stage.
- Telegraph dangerous attacks. Below 30% health, enter an enraged state with faster fire and summons.
- Ensure missile damage applies to the boss once per detonation; choose 20% of maximum health as the initial tuning value. Boss summons and eggs are cleared immediately.
- Show boss health, stage-clear messages, next-scene transitions, and final victory. On final victory, clear/reset the save so Continue cannot reopen the final boss.

**Gate:** a complete MainMenu → Level1 → Level2 → Level3 → Victory run works, with preserved player stats and no duplicate transitions.

### 6. Final art, audio, UI, and game feel — 6–9 hours

- Finish basic, armored, and cosmic chicken variants plus three recognizable bosses; avoid relying on recolor alone for stage identity.
- Add Earth/Jupiter/deep-space parallax layers, scrolling stars, themed debris, and distinct stage music.
- Finish menus, volume sliders, pause, Game Over, victory, and HUD. Route UI navigation through the Input System UI module.
- Add pooled exhaust/death/egg/shockwave effects, screen shake, damage flashes, bloom, and weapon/chicken/pickup sounds. Keep effects readable during the heaviest boss phase.
- Tune movement, fire rates, enemy health, drop rates, food threshold, and round duration through configuration.

**Gate:** each stage is visually identifiable, combat feedback is readable, and all required sounds/screens are present.

### 7. Verification, report, and submission — 7–12 hours

- Run the acceptance checklist below and profile the heaviest boss phase for recurring allocations, pool leaks, and frame spikes.
- Make the final Windows x86_64 build and test it outside the Unity Editor, including persistent saves across process restarts.
- Write the report with architecture, scene persistence, the JSON schema, ScriptableObject hierarchy, pooling lifecycle, boss FSMs, and screenshots of all stages and victory.
- Record exact sources and collaborators in credits/report, including free/CC0 assets. The assignment requires references even when the asset license does not require attribution.
- Compare the repository ignore rules with the instructor-supplied `.gitignore` if available. The supplied file was not present beside the PDF during inspection; the repository already has a Unity ignore file. Preserve `.meta` files and exclude generated `Library`, `Temp`, and logs.
- Package the repository URL text file, complete build output, and report using the required archive name. Re-extract and run it; after upload, download and check the submitted archive as the brief requests.

**Gate:** the archive contains a working complete Windows build and report; the repository can be cloned and opened with the recorded editor.

## Proposed code organization

Keep gameplay code beneath `Assets/_Game/` and original third-party content beneath `Assets/ThirdParty/`.

- `Scripts/Core/`: `GameManager`, `RunState`, gameplay-state transitions, shared events.
- `Scripts/Save/`: `SaveData`, validation, JSON file operations.
- `Scripts/Player/`: input, movement, health, weapon controller, missile controller.
- `Scripts/Enemies/`: health, entry/formation/dive behavior, egg attacks, asteroids.
- `Scripts/Bosses/`: boss controller, explicit FSM states, attack implementations.
- `Scripts/Progression/`: level controller, wave spawner, formation and path helpers.
- `Scripts/Pickups/`: food, gifts, score and food-meter rules.
- `Scripts/Pooling/`: pool registry and spawn/despawn lifecycle.
- `Scripts/UI/`, `Audio/`, `VFX/`: event subscribers and presentation.
- `Data/Levels/`, `Data/Waves/`, `Data/Weapons/`: immutable authoring assets.
- `Scenes/`, `Prefabs/`, `Art/`, `Audio/`, `Tests/EditMode/`, `Tests/PlayMode/`.

`LevelDataSO` holds level number, scene name, music, environment theme, and round entries. Each round entry references a `WaveDataSO` and difficulty modifiers. `WaveDataSO` holds round type, enemy/asteroid/boss prefab, count, movement/fire values, formation, and entry path. `WeaponDataSO` holds name, sprite, projectile prefab, damage, speed, fire rate, and firing pattern. Never put mutable run progress in these shared assets.

The persistent manager owns the run. Scene objects own their spawners, cameras, UI, and pools. HUD and audio react to events such as LivesChanged, ScoreChanged, WeaponLevelChanged, FoodChanged, RoundChanged, and BossHealthChanged. Scene subscribers unsubscribe on disable/destruction.

## Suggested campaign layout

These are initial content choices, not fixed assignment instructions. Bosses occupy the last numbered round.

- **Level1 / Earth orbit / 5:** basic grid; wider grid; asteroids; grid plus occasional divers; Giant Chicken boss.
- **Level2 / Jupiter belt / 7:** armored V; staggered V; asteroids; crossing V with divers; asteroids; armored mixed attack; Mecha-Chicken boss.
- **Level3 / deep space / 10:** tight cosmic formation; crossing entry paths; asteroids; fast dive wave; dense egg wave; asteroids; mirrored tight formations; mixed elite wave; final endurance wave; Galactic Emperor boss.

## Save contract and decisions

Proposed record, with one-based stage/round numbers for consistency with the HUD:

```json
{
  "schemaVersion": 1,
  "levelIndex": 2,
  "roundIndex": 4,
  "score": 12500,
  "lives": 2,
  "missiles": 1,
  "powerLevel": 3,
  "foodProgress": 6
}
```

The score above is illustrative. Store food as an integer count and define the threshold in configuration. Validate stage/round bounds against level data, nonnegative score/missiles/food, life bounds, and power in [1, 3].

- **New Game:** stage 1, round 1, score 0, three lives, two missiles, power 1, food 0; replace any previous run.
- **Continue:** start the saved round from its beginning. Enemy positions, projectiles, and partial boss health are not required checkpoint data.
- **Round complete:** finish pending scoring/pickup policy, stop outgoing attacks, clear transient objects, advance the index, save, then spawn the new round.
- **Pause → menu / ordinary quit mid-round:** keep the last round-boundary checkpoint; do not create a manual mid-round save.
- **Game Over:** immediately save the same stage's round 1 and three lives. Proposed policy for fields the brief does not specify: retain score, missiles, current reduced weapon power, and food at death. Apply the same policy to Retry and Continue and document it in the report.
- **Victory:** remove/reset the active run and disable Continue until a new run exists.
- **Simultaneous final player/enemy death:** resolve player Game Over first to prevent a late wave-clear event from overwriting its penalty checkpoint.
- **Round-boundary pickups:** allow a short configured collection interval, then remove remaining drops and save. This is a design choice to make checkpoint contents deterministic.

## Acceptance checklist

- [ ] All four scenes are included and load by configured names.
- [ ] Campaign has exactly 5/7/10 rounds, with a boss in each final round and 1–2 asteroid rounds per stage.
- [ ] WASD/arrows, Space hold, X, Pause, and UI input use Input Actions exclusively.
- [ ] Movement respects visible camera bounds at supported aspect ratios.
- [ ] Damage removes one life and one power level; invulnerability prevents repeated contact damage.
- [ ] Gifts clamp at level 3; food adds score and awards/reset progress for a missile.
- [ ] Player bullets destroy eggs; diving chickens do not drop eggs.
- [ ] Missiles clear normal enemies/summons/eggs and damage each boss once for the configured 15–20% of max HP.
- [ ] Every boss has an inspectable FSM, three attacking phases, enrage below 30%, and an accurate health bar.
- [ ] JSON round-trip preserves every required field; missing or corrupt saves do not crash or enable an invalid Continue.
- [ ] Close/reopen at ordinary, asteroid, boss, and stage-transition checkpoints resumes the correct scene/round once.
- [ ] Die in a late round, close from Game Over, relaunch, and Continue: same stage, round 1, three lives.
- [ ] Pause resumes cleanly; exit to menu does not create a manual save or expose restart-round.
- [ ] Repeated menu/stage/retry transitions do not duplicate managers, event listeners, music, or rewards.
- [ ] Offscreen despawns, bomb multi-kills, and pending entry spawns cannot stall or prematurely complete a round.
- [ ] Pooled bullets/eggs/food/asteroids reset on reuse and return cleanly during transitions.
- [ ] Final victory resets the save and shows the final score.
- [ ] All required themes, effects, music, SFX, menus, sliders, and HUD indicators are present.
- [ ] Final Windows build, clean-clone opening, report screenshots, source credits, and submission archive are verified.

Automate save validation/round-trip, progression counts, damage clamps, food rewards, and boss missile damage in EditMode where practical. Use focused PlayMode/manual checks for scene persistence, pooled reuse, input, visual feedback, and quitting/relaunching. No tests have been run at the planning stage.

## First implementation target

Complete milestones 1–3 before expanding campaign content: one playable round with upgrades, pooling, correct checkpoint persistence, and the death penalty. That provides a stable foundation for the remaining stages and bosses.
