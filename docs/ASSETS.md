# HW3 asset shortlist

Researched 15 November 2026. The game now uses imported local Kenney ship, meteor, background, food and sound assets, PucciGames Jupiter, three music tracks, and a licensed chicken sound. Nine prepared character/prop artwork files are integrated as sliced sprites and prefab references. See [CREDITS](../CREDITS.md) for the final source/license inventory and [ASSET_GENERATION.md](ASSET_GENERATION.md) for artwork provenance.

The shortlist below preserves the original research, including alternatives that were not selected. Animal Pack was not needed: generated/user-supplied full-body chicken art is used instead.

## Recommended direction

Use a clean cartoon style: **Kenney Space Shooter Remastered + Animal Pack Remastered + Planets + Food Kit**, with simple custom wings, armor, and boss accessories. This keeps most artwork from one creator. Animal Pack chickens are icon-style bases, so budget time for chicken bodies/wing animation and stage variants.

If full-body chicken animation is the priority, use the pixel alternative below as a complete direction. Avoid mixing small pixel chickens with smooth cartoon foreground art without deliberately adapting one style.

## Primary art sources

### Ships, lasers, missiles, asteroids, stars, and HUD icons

- **[Space Shooter Remastered — Kenney](https://kenney.nl/assets/space-shooter-remastered)**
- License: **CC0**; original archive license inspected.
- The old `/space-shooter-redux` URL redirects to this current page. Prefer the canonical page when downloading.
- Use for the player ship, projectiles, missile graphics, asteroids, basic backgrounds, and matching game icons. The pack also includes fonts and sounds; select only what the game uses.
- Confirmed archive paths include `PNG/Meteors/meteorBrown_big1.png` and `PNG/Meteors/meteorGrey_big1.png`.
- These are sprites, not a complete shooter implementation. Add 2D colliders, health, movement, pooling, and attack behavior yourself.
- [Original Redux listing by Kenney on OpenGameArt](https://opengameart.org/content/space-shooter-redux) is another source for the older pack.

### Chicken art base

- **[Animal Pack Remastered — Kenney](https://kenney.nl/assets/animal-pack-remastered)**
- License: **CC0**; original archive license inspected.
- Confirmed chicken PNGs include `PNG/Round/chicken.png`, `PNG/Round (outline)/chicken.png`, and square/detail-free alternatives.
- Use the chicken face/motif as the common visual base. Add bodies and separately animated wings if needed; it is not a ready-made flying-chicken animation pack.
- Create three enemy variants: plain chicken; chicken with gray armor plates/visor; cosmic chicken with angular helmet, glowing eye details, and energy accents.
- Create three bosses from this visual language: larger Giant Chicken, Mecha-Chicken with mechanical attachments, Galactic Emperor with crown/energy wings. Give each a distinct silhouette, not just a different tint.

### Stage backgrounds and planets

- **[Planets — Kenney](https://kenney.nl/assets/planets)**
- License: **CC0**, stated on the creator's page; 50 listed files.
- Use as stylized planet/background elements alongside the space pack's stars. Assemble several independently scrolling layers in Unity.
- Ensure Level1 reads as Earth orbit and Level2 as Jupiter's belt. Verify the chosen image actually represents the needed planet; recolor or draw continent/band overlays if the generic artwork needs adaptation.
- Level3 can use a dark starfield with colored nebula layers created from particle textures. Parallax motion is project code, not supplied behavior.

### Food pickups and eggs

- **[Food Kit — Kenney](https://kenney.nl/assets/food-kit)**
- License: **CC0**; original archive license inspected.
- Although listed as a **3D** kit, the archive includes **2D PNG renders** under `Previews/`, also described by the creator in [this release post](https://www.construct.net/en/forum/game-development/tools-and-resources-27/kenneys-free-assets-30000-70995/page-17). Use those images as sprites; keep the game itself 2D.
- Confirmed render paths: `Previews/burger.png`, `Previews/burger-cheese.png`, and `Previews/egg.png`. Select a burger for food pickups, which the assignment explicitly allows instead of drumsticks.
- Pick the render orientation that reads best at gameplay size; if an egg needs a clearer silhouette, use a simple original egg sprite.

### Menus and bars

- **[UI Pack — Kenney](https://kenney.nl/assets/ui-pack)**
- License: **CC0**, stated on the creator's page; 430 listed files.
- Use a restrained subset for menu buttons, panels, sliders, food progress, and boss health. Reuse the space pack's life/missile icon language where appropriate.
- Apply Unity 9-slicing to scalable panels. UI prefabs, input navigation, binding, and events still need implementation.

### Particle textures

- **[Particle Pack — Kenney](https://kenney.nl/assets/particle-pack)**
- License: **CC0**, covered by [Kenney's asset licensing statement](https://kenney.nl/support).
- Use textures to build exhaust, impact sparks, smoke, and expanding missile-shockwave effects with Unity Particle Systems.
- Create small feather and broken-eggshell sprites if the selected textures do not cover these specific shapes. Pool effects or configure particle stop behavior to disable them for reuse.
- This pack provides texture artwork; bloom, screen shake, damage flashes, and particle emitters are implemented/configured in the project.

## Audio sources

### Weapons, explosions, and engines

- **[Sci-fi Sounds — Kenney](https://kenney.nl/assets/sci-fi-sounds)**
- [Creator-uploaded OpenGameArt mirror](https://opengameart.org/content/sci-fi-sounds).
- License: **CC0**, stated on both listings; 70 OGG effects described by the creator.
- Covers lasers, explosions, engines, and other sci-fi sounds. Pick a light repeating laser, a heavier missile explosion, and short UI/pickup feedback after auditioning.
- Trim/fade and balance effects in the AudioMixer. Limit simultaneous egg/death clucks during large multi-kills.

### Chicken clucks

- **[Chicken clucking — Breviceps, Freesound](https://freesound.org/people/Breviceps/sounds/456803/)**
- License: **CC0**, stated on the recording page.
- Listed as mono WAV, approximately 15 seconds. Trim a few short clucks for egg-laying and death; modest pitch variation can reduce repetition.
- **Access note:** the page requires a Freesound login to download the original. This is the only recommended source here with a verified account requirement.

### Distinct stage music

- **[Outer Space music pack — albesca](https://albesca.itch.io/outer-space-music-pack)**
- License: **CC0**, stated on the creator's page.
- Contains two songs and three loops, offered as five OGG downloads: `ALongJourney.ogg`, `AStarlessSky.ogg`, `BarrenPlanets.ogg`, `DistressSignal.ogg`, and `OuterSpace.ogg`.
- Audition and choose three distinct tracks/loops, one per stage. Check seamless loop points and whether the tempo suits combat before assigning them in `LevelDataSO`.
- Suggested audition order: `ALongJourney` for Earth orbit, `BarrenPlanets` for the belt, `OuterSpace` for deep space. This mapping is a proposal, not a claim that the tracks have already been auditioned.

## Pixel-art alternative

Choose this set together if using pixel characters:

- **[Chicken Sprites — Shepardskin, OpenGameArt](https://opengameart.org/content/chicken-sprites)** — **CC0**. Front/back/profile walk and peck animations; original and scaled versions. These are full-body RPG chickens, so flying/attack poses, armor, cosmic variants, and bosses still require editing. [Direct archive](https://opengameart.org/sites/default/files/chicken.zip).
- **[Pixel Shmup — Kenney](https://kenney.nl/assets/pixel-shmup)** — **CC0**, checked on the creator's page. 128 listed assets on a 16×16 tile basis. Use selected flying craft/projectiles as the matching pixel base; assess their fit before replacing the primary space pack.
- **[Pixel Platformer Food Expansion — Kenney](https://kenney.nl/assets/pixel-platformer-food-expansion)** — **CC0**, original archive license inspected; 110 listed sprites. Select a suitable food image or adapt one; verify the actual food silhouette rather than assuming a named drumstick is included.
- Reuse the audio sources above. Make a small matching pixel HUD and egg/gift/feather sprites; adapt background art to a consistent pixel grid.

## Additional sources found for the missing artwork

Follow-up research on 15 November 2026. The user subsequently requested original image-generated assets instead; the links below remain fallback sources. The finished artwork inventory is in [ASSET_GENERATION.md](ASSET_GENERATION.md). Basic and armored chicken sheets were generated in this chat; the user supplied the other seven files, which were made transparent with Python. All planned character/prop artwork is present.

### Flying chicken animation — found, CC0

- **[Bevouliin Free Flappy Chicken](https://opengameart.org/content/bevouliin-free-flappy-chicken)** by bevouliin.com.
- [Direct ZIP](https://opengameart.org/sites/default/files/free%20flappy%20chicken.zip). Archive inspected: four transparent flying PNGs at `Transparent PNG/flying/frame-1.png` through `frame-4.png`, plus two hit frames and GIF previews.
- Flying frames are 1100×868 RGBA. The artwork was visually inspected: rounded cartoon chicken with a side/three-quarter view, rather than the proposed front-facing formation view. Usable for a prototype; adopting it changes the character-view direction.
- Although the description mentions vector authoring, this inspected ZIP contains PNG/GIF files, not editable vector source.

### Rooster boss — found, pixel-art alternative

- **[Boss Rooster](https://opengameart.org/content/boss-rooster)** by Spring Spring.
- The page offers CC BY 3.0/4.0 and CC BY-SA alternatives. Record CC BY 4.0 if choosing that offered license, with creator/source/license credits and modifications disclosed.
- [PNG sheet](https://opengameart.org/sites/default/files/boss%20rooster_0.png) inspected: 1440×96 RGBA. The page includes standing, running, crowing, jumping, and feather-shooting previews and labels the work WIP.
- This is a side-view pixel rooster, not a finished front-facing space boss. It provides a genuine boss base for a pixel-art direction, but does not supply the three required boss designs or their FSM behavior.
- Creator notice: Julie Damsgaard / Spring Spring / Spring Enterprises, https://spring-enterprises.neocities.org.

### Gift pickup — found, CC0

- **[Present Gift Box Reward Seasonal](https://opengameart.org/content/present-gift-box-reward-seasonal)** by Shiroware.
- [Direct ZIP](https://opengameart.org/sites/default/files/presentsfromshiroware.zip). Archive and `licence.txt` inspected.
- Contains `present-gift-box-reward-full.png`, separate top/bottom PNGs, and a Paint.NET source file. Full image is 800×600 RGBA with transparent outer padding; crop padding and scale to the game's pickup size during production.

### Feather particles and energy feathers — found, CC0

- **[Duck & Duckling feather vector sprites](https://opengameart.org/content/duck-duckling-feather-vector-sprites)** by weirdybeardyman.
- [PNG](https://opengameart.org/sites/default/files/duckfeather2_0.png) inspected: 256×256 RGBA, clean isolated feather. [Editable SVG](https://opengameart.org/sites/default/files/duckfeathersvg.svg) is also offered. Recolor for chicken feathers or glowing energy projectiles.
- For the pixel alternative: **[Simple Feather](https://opengameart.org/content/simple-feather)** by Hiross, CC0, 26×27 pixels.
- **[Feathers](https://opengameart.org/content/feathers)** by syntactics is another CC0 source, described as a colorful radial-feather effect sheet. Listed only; its animation layout was not inspected.

### Cracking egg and eggshells — found, CC0

- **[Spider egg cracking](https://opengameart.org/content/spider-egg-cracking)** by Deevei.
- Creator explicitly offers 32×32 PNG frames and editable SVG under CC0: [PNG archive](https://opengameart.org/sites/default/files/egg_png.7z), [SVG](https://opengameart.org/sites/default/files/eggs.svg).
- Downloads and SVG structure verified; 7z file signature checked. The animation was authored for a spider egg, so recoloring and selecting suitable cracking frames are still production work. No claim that the source is already a white chicken-egg effect.
- A CC0 3D fallback, **[egg and cracked egg](https://opengameart.org/content/egg-and-cracked-egg)** by oglsdl, was also inspected. Its ZIP contains Blender models and a texture, not ready-made 2D sprite frames, so it would require rendering.

### Armor, cosmic details, and emperor components — found as parts

- **[Robot pack](https://opengameart.org/content/robot-pack)** by Kenney, CC0. [ZIP](https://opengameart.org/sites/default/files/Robot%20pack.zip) inspected: separate robot-body/track PNGs, spritesheets, and vector sources. Can contribute mechanical shapes; these are not armored-chicken sprites.
- **[Space Shooter Extension](https://kenney.nl/assets/space-shooter-extension)** by Kenney, CC0, original archive license inspected. Additional spacecraft/astronaut art can contribute machinery and accessories.
- **[Chest armor](https://game-icons.net/1x1/delapouite/chest-armor.html)**, **[Cyborg face](https://game-icons.net/1x1/delapouite/cyborg-face.html)**, and **[Imperial crown](https://game-icons.net/1x1/delapouite/imperial-crown.html)** by Delapouite; **[Wing cloak](https://game-icons.net/1x1/lorc/wing-cloak.html)** by Lorc. Creator pages checked: CC BY 3.0, SVG/PNG downloads offered. These are monochrome icons that need editing/coloring/compositing, not completed character assets.
- No exact, ready-made, clearly licensed Mecha-Chicken or Galactic Emperor sprite set was verified. Their final character compositions and animations remain to be produced.

### Recognizable Earth/Jupiter — found, CC0

- **[Planet solar system](https://opengameart.org/content/planet-solar-system)** by Guyome41. [ZIP](https://opengameart.org/sites/default/files/planet.zip) inspected: `planet/terre.png` and `planet/jupiter.png`, other planets, and a license file. Both images were visually inspected; they are small pixel sprites, suitable for small background elements rather than large smooth illustrations.
- **[Planet Sprite pack](https://opengameart.org/content/planet-sprite-pack)** by PucciGames, CC0. Archive inspected: `Jupiter_Sprite.png` and 19 other named images. No explicitly named Earth file was found in that archive.

## Import and credit checklist

1. Download through the creator's pack page; Kenney's free download flow includes “Continue without donating.” Use page links rather than copying temporary signed download URLs.
2. Preserve each original license under `Assets/ThirdParty/<Creator>/<Pack>/`, along with the files actually selected. Do not import entire 3D model libraries just to use food sprites.
3. Set images to Sprite (2D and UI); slice sprite sheets as needed. Use compatible pixels-per-unit so ships, pickups, and enemies have sensible relative sizes.
4. Use bilinear filtering for the proposed smooth art direction; use Point filtering and integer scaling for the pixel alternative. Check compression and alpha edges at final on-screen size.
5. Import sound as appropriate for short effects versus looping music, set loop behavior, and route Music/SFX through volume controls.
6. Record creator, exact source URL, chosen files, license, download date, and modifications. Carry this information into the report even for CC0 sources because the assignment requires source disclosure.
7. Do not import scripts from unrelated sample games. Build the assignment's scene, save, wave, pooling, input, and FSM systems in this project.

Verification scope: creator/listing pages were checked online. The original Space Shooter Remastered, Animal Pack Remastered, Food Kit, and Pixel Platformer Food Expansion archives were inspected in memory, including their license files. No pack was Unity-import tested and no music was auditioned during this planning task.
