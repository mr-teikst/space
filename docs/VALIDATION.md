# Validation record

Validated on 15 November 2026 with Unity 6000.6.6f1. Validation ran in an isolated project snapshot so the user's open Editor and save slot were not disturbed. Authored scenes, prefabs, data, importer settings, and intended PlayerSettings were synced back while preserving workspace asset GUIDs.

## Automated results

**9 tests passed, 0 failed.**

Five EditMode tests cover complete checkpoint round trips and replacement, invalid/incomplete saves, one-time health resolution and reuse, projectile collision guards and reuse, and missile target rules including the 20% boss damage rule.

Four PlayMode tests cover menu/pause/death/retry/continue, later-stage checkpoint stats and same-stage death reset, all 22 numbered rounds through all three scenes and final victory, and rendering. These tests use isolated save filenames and clean them up.

The rendering test captured MainMenu, Level1, Boss1, Level2, Boss2, Level3, and Boss3, rejecting blank output or missing-shader magenta. Screenshots were also visually reviewed. See [menu](images/MainMenu.png), [Earth](images/Level1.png), [Jupiter](images/Level2.png), and [final boss](images/Boss3.png).

Editor validation passed: four registered scenes, 22 total rounds (5/7/10), five asteroid rounds, three bosses in the final numbered rounds, required scene/component references, valid sprite rectangles, audio clips, and stage links. A separate workspace reference check found no duplicate asset GUIDs or missing references in the authored scenes/prefabs/data/input files.

Raw results: [EditMode XML](validation/chicken-editmode.xml), [PlayMode XML](validation/chicken-playmode.xml), and [scene validation JSON](validation/scene-validation.json).

## Scope of validation

The automated campaign test accelerates time and resolves spawned enemies to verify progression and lifecycle behavior. It does not simulate a skilled player or establish that the difficulty is balanced. Human playtesting is still appropriate for enemy cadence, drop rates, boss damage, and animation timing. The graphics test verifies rendering in the Editor, not every graphics driver.

## Windows build

Windows x86_64 player build succeeded with Unity's installed Windows module (`BuildResult.Succeeded`, `CHICKEN_WINDOWS_BUILD_COMPLETE`, Editor exit code 0). The distributable is `Builds/ChickenShooter-Windows.zip`; extract the entire directory and run `ChickenShooter.exe`. Credits and launch instructions are included. This player was compiled on Linux and has not been manually played on native Windows.
