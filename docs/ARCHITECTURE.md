# Implementation architecture

## Saved objects, not a runtime scene builder

`ProjectBuilder` is an Editor-only authoring utility. It produces `.unity`, `.prefab`, `.asset`, material, sprite-import, and Input Actions files. Those files are the playable project. The game does not need to run the authoring utility on startup. Inspect or modify the saved objects directly.

Every gameplay scene has its own camera/bounds, starfield layers, pool registry, wave director, player prefab, feedback director, HUD, menus, and Input System UI module. A persistent GameSession prefab carries run state and audio between scenes. Duplicate session instances are discarded during scene changes; audio also guards its persistent instance.

## Component responsibilities

- **GameSession / RunState:** current stage/round, score, lives, weapon level, missiles, food count, and gameplay phase transitions.
- **SaveService:** validates checkpoint data and writes JSON via a temporary file plus replacement. It has no enemy, UI, or camera responsibilities.
- **PoolService / PooledObject:** prewarms Inspector-listed prefabs and returns inactive instances for reuse. Projectiles, eggs, food, gifts, asteroids, enemies, bosses, and effects use the pool.
- **PlayerMovement / PlayerHealth / PlayerWeapon:** Input Actions movement, clamping, damage/i-frames, firing patterns, and the missile action.
- **Damageable / Projectile:** health, one-time resolution, scores/drops, damage routing, travel, lifetime, and collision guards.
- **EnemyController / AsteroidController:** curved entry, formation motion, egg attacks, aimed dives, asteroid drift and offscreen completion.
- **BossController:** entry and explicit barrage/spread/dive/enraged states. Boss prefabs supply independent health, cadence, projectile/summon references, and artwork.
- **LevelController:** pending spawns, unresolved wave members, round completion, collection delay, checkpoint advancement, stage-clear messages, and stage completion requests.
- **HudPresenter / GameScreens / MenuController:** event-driven UI and serialized screen objects. Pause does not expose a save or restart command.
- **AudioDirector:** serialized music/SFX sources and cue table, volume settings, scene music, and rate limiting.
- **SpriteFlipbook / DamageFlash / EffectsDirector / PooledParticles / EffectLifetime / ScreenShake / ParallaxScroller:** focused presentation behaviors.

## Authoring data

Three `LevelDataSO` assets contain 5, 7, and 10 round references and scene/music/theme/next-stage data. Each `WaveDataSO` controls a formation, destructible-asteroid round, or boss round. The final numbered round is the boss; it is not an additional round. There are five asteroid rounds across the campaign. `WeaponDataSO` configures the player's projectile, damage, speed, cadence, and spread.

Shared data assets are read-only during play. Mutable progress lives in RunState, so replaying a scene does not mutate a wave asset or another enemy's configuration.

## Checkpoint contract

The JSON record contains schemaVersion, levelIndex, roundIndex, score, lives, missiles, powerLevel, and foodProgress. Stages/rounds are one-based. Continue restores the manager first and then loads the referenced scene; the level controller starts that saved round from its beginning.

Round completion waits for both pending spawns and unresolved participants. A short collection interval allows pickups before transient objects are cleared and the next round checkpoint is written. Offscreen enemies count as resolved but award no kill points. Summons do not block boss-round completion. Final player death immediately writes same-stage round 1 with three lives, preserving score, remaining missiles, food, and the current reduced weapon level. Retry and later Continue use the same checkpoint. Final victory clears the completed run.

## Scene and event lifetimes

HUD, screens, feedback, damage flash, and audio subscribe/unsubscribe to C# events with their activation lifetime. GameSignals clears static listeners on subsystem initialization. Pool returns disable objects; activation resets projectile guards/timers, damageable health, AI configuration, flipbook time, and particles. Scene unload releases scene-owned pools and UI. Persistent session/audio do not retain scene cameras, players, or spawners.

## Validation

EditMode tests exercise save integrity and combat/pooling guards. PlayMode tests use the actual saved scenes, isolate their save slots, and exercise menu/new game/pause/death/retry/continue, restored later-stage stats, and all 22 rounds to victory. A graphics test renders the menu, all three stages, and all three bosses and rejects blank or missing-shader output. Editor project validation checks saved scene references, audio, sprite rectangles, round counts, and boss/asteroid placement.

Generated animation frames may still benefit from timing/art cleanup, and difficulty should be tuned through the Inspector after human playtesting. Automated campaign completion validates progression by resolving spawned enemies; it is not evidence that the difficulty is balanced for a person.

## Rendered saved scenes

The following images were rendered by the graphics smoke test from the actual saved scenes.

![Main menu](images/MainMenu.png)

![Jupiter stage](images/Level2.png)

![Galactic Emperor boss](images/Boss3.png)

## Inspector tuning entry points

Open a level scene and select **Wave Director** to inspect its LevelDataSO reference. Open that asset to change the ordered rounds, music, or next scene. Select a referenced WaveDataSO to change enemy count, spacing, entry timing, movement, fire intervals, and dive frequency. Change boss health and attack timings on the corresponding boss prefab. Change player cadence/damage in the WeaponDataSO and movement/invulnerability on the PlayerShip prefab. The scene's Object Pools list explicitly assigns all pooled prefabs and prewarm counts. HUD and screen objects have serialized references and persistent button callbacks.
