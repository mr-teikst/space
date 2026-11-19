# Original missing game assets — generation brief

Requested 15 November 2026. This supersedes the custom-art sourcing proposal for the missing sprites in `ASSETS.md`; existing ship/UI/music sources remain available.

## Status

**All nine planned character/prop artwork files are now saved** under `Assets/_Game/Art/Generated/`. Basic and armored chicken sheets were made with built-in image generation. The user supplied cosmic chicken, three bosses, gift, feather, and egg artwork; Python removed their baked-in backgrounds. Original copies are preserved as `*-source.png`. RGBA transparency was checked and the cleaned artwork visually inspected over a dark backdrop. No API/CLI fallback was used.

Successful prompts and import notes are recorded in `Assets/_Game/Art/Generated/README.md`. Basic/armored sheets are 1261×1247, cosmic is 1035×1024, and all six newly supplied files are 1024×1024. Use `sprites.json` for sprite rectangles; the egg sheet has four 512×512 cells. Animation and gameplay integration still need an in-engine preview.

## Deliverables

Use one generation call per character sheet or individual prop. Save final selected PNGs under `Assets/_Game/Art/Generated/` after generation. Preserve generated transparency. Check output dimensions, padding, consistent scale/pivots, and visual continuity before claiming animation readiness.

1. **Created:** `chicken-basic.png`: four-pose flapping sheet, front-facing full-body white chicken.
2. **Created:** `chicken-armored.png`: matching four-pose sheet with a steel helmet, chest armor, and mechanical wing details.
3. **Created from user-supplied artwork:** `chicken-cosmic.png`, 1035×1024 RGBA. The supplied `al_chicken.png` is preserved as `Assets/_Game/Art/Generated/chicken-cosmic-source.png`. The selected design has a green alien face, antennae, silver armor, and cyan accents in four poses. On the user's explicit instruction, `scripts/remove_sprite_background.py` removed border-connected white and unmatted edge pixels while preserving enclosed armor highlights. The result was visually checked over a dark background; frame rectangles are in `sprites.json`.
4. **Prepared from `bos1.png`:** `boss-giant-chicken.png`, giant white chicken with spread wings, 1024×1024 RGBA.
5. **Prepared from `bos2.png`:** `boss-mecha-chicken.png`, steel-blue robotic chicken with orange details and egg-launcher ports, 1024×1024 RGBA.
6. **Prepared from `bos3.png`:** `boss-galactic-emperor.png`, crowned green alien chicken with purple/cyan armor and luminous wings, 1024×1024 RGBA.
7. **Prepared from `gift.png`:** `pickup-gift.png`, red gift box with gold ribbon, 1024×1024 RGBA.
8. **Prepared from `feather.png`:** `feather.png`, white feather with blue-gray shading; the dark-gray matte was removed, 1024×1024 RGBA.
9. **Prepared from `egg.png`:** `egg-breaking.png`, intact/cracked/splitting/fragment frames in a 2×2 layout. Light-gray matte removed, 1024×1024 RGBA. Play once, top-left → top-right → bottom-left → bottom-right.

Cleanup commands use `scripts/remove_sprite_background.py` with `--background white` for bosses/gift, `--background dark-gray` for the feather, and `--background light-gray` for eggs. Flood fill removes only border-connected neutral matte; local edge unmatting reduces halos and enclosed highlights remain intact. Original downloads were copied byte-for-byte before processing. This is artwork preparation, not completed boss animation, prefabs, VFX emitters, or gameplay logic.

## Shared art direction

Original 2D cartoon arcade sprites for a Unity vertical space shooter. Clean vector-like raster rendering, bold dark navy outlines, broad shapes, restrained cel shading, readable at small sizes. Characters face the player straight-on in an orthographic view. No copied game characters, text, numbers, grid lines, backgrounds, ground shadows, or watermarks. Genuine alpha transparency. Keep every sprite fully inside its frame with transparent padding.

Animation continuity matters: body centers, head shape, feet positions, and scale remain stable; only the animated parts move. Generated frames need inspection before use. Bosses initially use a single isolated sprite, animated through transforms and separately configured effects in Unity; attack-frame sheets can be added later if needed.

## First prompt attempted

Use case: stylized-concept. Asset type: production 2D game sprite animation sheet for a Unity vertical space shooter inspired by classic cartoon arcade games. Generate ORIGINAL artwork, not copied characters. Subject: one cute but aggressive white invading chicken with a round full body, red comb and wattle, orange beak and two little feet, chunky white feathered wings. Straight-on front-facing orthographic view, symmetrical anatomy, no ground or perspective. Style: polished clean cartoon vector-like raster art, bold dark navy outlines, broad simple shapes, subtle cel shading, clear small-size silhouette. Animation sheet: exactly 4 equal square cells arranged horizontally in ONE row, wide landscape canvas, four complete fully separate poses of the SAME IDENTICAL chicken at the same scale and same center-height; wings high, wings horizontal, wings low, wings horizontal. Body and head stay fixed across frames, only wings flap. Wings must be well-separated from body silhouette. Each character fully contained in its cell with at least 12% transparent padding on all sides; keep feet, wing tips and comb intact. Background must be TRUE alpha transparency throughout, no checkerboard drawing, no colored matte, no scene, no shadow outside the sprite. No captions, text, numbers, grid lines, watermark, glow or particles. Deliver a usable aligned animation atlas, not a presentation/mockup.

## Additional prompt specifications

Use the shared direction above as the prefix, then the corresponding deliverable's subject. For armored/cosmic chickens, preserve the selected basic sheet as the style/anatomy reference and use the same four wing positions. For bosses, request a single large centered sprite with at least 12% transparent padding. For the egg sheet, request four equal cells with stable centers and separated pieces contained within the final cell. Props should remain clear without strong bloom; add dynamic glow through Unity materials/effects.

Record the exact prompts actually submitted and generator mode alongside final outputs after successful generation. Do not describe this brief as finished artwork or the proposed animation sheets as already validated.
