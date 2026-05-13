# Blog Post #2 — Game Design Document & Milestones

*Project: Undead Arcade — Game Development course, May 2026*

With Roll-a-Ball wrapped up, it's time to commit to a real plan. This post is both the **v1.0 Game Design Document** for my project — **Undead Arcade** — and the breakdown of the three milestones that will drive the rest of development. The course says the GDD should evolve with the project, so this is a starting point, not a contract.

---

## The Concept

**Undead Arcade** is a single-player top-down 3D twin-stick zombie shooter. You stand in the middle of an arena, zombies come at you from all sides, and you shoot them. Survive three waves and you win.

That's the whole pitch. It's intentionally simple — I have seven days, I'm working solo, and the goal is a polished small game rather than an ambitious unfinished one.

## Player Experience (Aesthetics)

The vibe I'm chasing is closer to **Doom** than to a survival horror game. The player should feel **powerful** — the gun is fast, individual zombies are weak, and killing a horde is meant to feel satisfying rather than scary. The threat comes from the *volume* of enemies, not from any one zombie being dangerous.

Using the MDA framework from the lectures: my target **aesthetic** is power fantasy (Sensation + Challenge in MDA terms — punchy feedback and overwhelming odds, in that order). For that aesthetic, the **dynamics** I need are: fast shooting, weak-but-numerous enemies, no downtime between targets. The **mechanics** that produce those dynamics are: high fire-rate shooting on the right trigger, zombies that die in 1–2 hits, and a wave spawner that keeps the screen full of targets.

Working backward from feeling to mechanic kept me honest. I almost designed in tanky boss enemies and weapon-switching, but neither serves "feel powerful" — bosses *slow down* the killing, and weapon variety adds complexity for no aesthetic payoff. Cut.

## Gameplay

**Objective:** Survive three waves. Last wave cleared = you win. HP hits zero = you die.

**Core loop:** wave starts → zombies spawn at arena edges → player moves with the left stick, aims and shoots with the right stick → kills increase the score → wave clears → short breather → next wave. Repeat until win or death.

**Progression:** Wave 1 is light (a few zombies, learn the controls). Wave 2 ramps up volume. Wave 3 is the chaotic finale — significantly more zombies, no breathing room.

## Platform & Controls

The game ships in two builds from one project: a **Windows executable** for the VIA Arcade Machine and a **WebGL build** hosted on GitHub Pages so the teacher can play it in the browser.

Input is **gamepad-first** because the arcade machine demands it — no keyboard or mouse on the cabinet. Unity's new Input System abstracts the hardware: the same code that reads the arcade's joystick (via Steam Input) also reads an Xbox controller plugged into a browser. For convenience during browser testing, I'll add a secondary **keyboard + mouse** binding (WASD to move, mouse to aim, left-click to fire), so the WebGL build remains playable even if no controller is plugged in. The primary design target is still gamepad — keyboard is a backup, not a balanced control scheme.

## Mechanics

- **Player:** Rigidbody-based movement. Left stick moves, right stick aims, right trigger fires continuously (rate-limited). Decent move speed so the player feels mobile. Health bar visible at the top.
- **Zombie:** A single enemy type. Uses a NavMeshAgent to pathfind toward the player and a simple finite state machine (Chase → Attack → Dead). Dies in 1–2 hits. Deals damage on touch.
- **Bullets:** Trigger colliders, short lifetime, deal damage on `OnTriggerEnter` and despawn.
- **Wave system:** A coroutine-driven `WaveManager` spawns the wave, waits for all zombies to die, then triggers the next one.

## Game World & Style

A single arcade-style arena (~25×25 units). Visuals are stylised low-poly using free **Synty POLYGON Starter Pack** assets — bright and readable rather than dark and atmospheric, which fits the "fun power fantasy" tone better than a horror look would. Top-down camera at ~60°.

## Assets

| Type | Source |
|------|--------|
| 3D models | Synty POLYGON Starter Pack (free) |
| Character animations | Mixamo (free) |
| SFX (gunshot, zombie, hit) | Freesound / Pixabay (CC0) |
| Music | Pixabay royalty-free |

All third-party sources will be credited in the README.

## Required Course Topics — Coverage Plan

| Topic | Where it lives |
|-------|---------------|
| **Scripting** | MonoBehaviours, coroutines for wave timing, C# events for `OnPlayerDied` / `OnWaveCleared` |
| **Input & Vectors** | Unity's new Input System; twin-stick aiming via Vector2 → Vector3 |
| **Physics** | Rigidbody player, trigger colliders on bullets, solid colliders on walls |
| **Graphics & Audio** | URP materials, AudioSources for gunshots and zombie sounds |
| **Animation** | Animator controllers for player and zombie (idle / walk / attack) |
| **Game Architecture** | `GameManager`, `WaveManager` as managers; `EnemyData` ScriptableObject; SOLID single-responsibility per script |
| **Game AI** | NavMeshAgent + FSM for zombie behavior |
| **UI** | Main menu, pause menu, HUD (health/wave/score), game-over screen — all gamepad-navigable |

## Scope Cuts (so I know what I'm *not* building)

To stay on time, the following are explicitly out: multiple enemy types, weapon switching, power-ups, dash, multiple levels, custom shaders, particle effects beyond muzzle flash. Anything I add later beyond the minimum is bonus, not baseline.

---

## The Three Milestones

These drive blog posts #3, #4, and #5.

**Milestone 1 — Core Loop.** *"It is technically a game."* Player moves, aims, and shoots. One zombie chases and damages the player. Bullets kill the zombie. Player can die. No HUD, no waves, no audio — just the verbs working end-to-end. The goal is to prove the core loop *feels* right in grey-box form before adding anything on top.

**Milestone 2 — Wave System + HUD.** *"It is a real game now."* The full three-wave spawner. Health, wave counter, and score visible on the HUD. Basic audio (gunshot, zombie groan, hit, death). Game-over and win screens. By the end of this milestone, the game is fully playable start-to-finish.

**Milestone 3 — Polish + Architecture.** *"It is shippable."* Main menu and pause menu. ScriptableObject extracted for zombie stats. Code refactored against SOLID principles. Animations on the player and zombie. WebGL build verified on GitHub Pages.

---

## Reflection

The hardest part of this stage wasn't writing the document — it was **cutting**. Every time I described a mechanic, I wanted to add an exception or a variant. *"What if there's a tanky zombie? What if there's a pistol* and *a shotgun? What if waves can spawn from different sides?"* All reasonable ideas, none of them necessary, every one a multi-hour rabbit hole.

The MDA framework helped me make those calls. Once I settled on "powerful" as the target aesthetic, any feature that didn't directly serve that feeling was cut. Tanky enemies *reduce* the feeling of power. Weapon switching *interrupts* the flow of killing. Better to ship one clean idea than three half-built ones.

The next post (#3) covers Milestone 1 — getting from an empty Unity project to a working core loop.
