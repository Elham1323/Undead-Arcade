# Blog Post #2 — Game Design Document & Milestones

*Project: Undead Arcade — Game Development course, May 2026*

This post is both the **v1.0 Game Design Document** for my project — **Undead Arcade** — and the breakdown of the three milestones that will drive development. The course says the GDD should evolve with the project, so this is a starting point, not a contract.

---

## The Concept

**Undead Arcade** is a single-player top-down 3D arcade zombie shooter. The player stands in an arena, zombies attack from all sides, and the goal is to survive three waves. Last wave cleared is a win; running out of health is a loss.

## Player Experience (Aesthetics)

The vibe is closer to **Doom** than to a survival horror game. The player should feel **powerful** — the gun is fast, individual zombies are weak, and clearing a horde should feel satisfying rather than scary. The threat comes from the *volume* of enemies, not from any one zombie being dangerous.

Using the MDA framework from the lectures: the target **aesthetic** is power fantasy (Sensation + Challenge in MDA terms — punchy feedback and overwhelming odds, in that order). For that aesthetic, the **dynamics** needed are: fast shooting, weak-but-numerous enemies, no downtime between targets. The **mechanics** that produce those dynamics are: a high fire-rate weapon that auto-targets the nearest zombie, zombies that die in 1–2 hits, and a wave spawner that keeps the screen full of targets.

Working backward from feeling to mechanic kept the design honest. Tanky boss enemies and weapon-switching were both considered and cut — bosses *slow down* the killing, and weapon variety adds complexity for no aesthetic payoff.

## Gameplay

**Objective:** Survive three waves. Last wave cleared = win. HP hits zero = lose.

**Core loop:** wave starts → zombies spawn at arena edges → player moves around the arena, gun auto-aims at the nearest zombie, fire button kills it → kills increase the score → wave clears → short breather → next wave. Repeat until win or death.

**Progression:** Wave 1 is light (a few zombies, learn the controls). Wave 2 ramps up volume. Wave 3 is the chaotic finale — significantly more zombies, no breathing room.

## Platform & Controls

The game ships in two builds from one project: a **Windows executable** for the VIA Arcade Machine and a **WebGL build** hosted on GitHub Pages.

The VIA Arcade Machine gives each player **one analog joystick and six buttons**. That's an important constraint: a traditional twin-stick shooter (one stick to move, one to aim) is not possible on this hardware. Instead, the player **moves with the joystick** and the gun **auto-aims at the nearest zombie**. The fire button (the black button, mapped to the right trigger of a virtual gamepad) shoots. This is the same control philosophy used by games like *Vampire Survivors* — the player's job is positioning, not aiming.

For browser play on the WebGL build, a keyboard fallback is provided: WASD to move, left-click or spacebar to fire. Auto-aim works the same way regardless of input device.

## Mechanics

- **Player:** Rigidbody-based movement controlled by the joystick. The weapon auto-aims at the closest zombie within range; the fire button shoots continuously (rate-limited). Health bar visible at the top of the screen.
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
| **Input & Vectors** | Unity's new Input System; joystick Vector2 input converted to Vector3 world movement |
| **Physics** | Rigidbody player, trigger colliders on bullets, solid colliders on walls |
| **Graphics & Audio** | URP materials, AudioSources for gunshots and zombie sounds |
| **Animation** | Animator controllers for player and zombie (idle / walk / attack) |
| **Game Architecture** | `GameManager`, `WaveManager` as managers; `EnemyData` ScriptableObject; SOLID single-responsibility per script |
| **Game AI** | NavMeshAgent + FSM for zombie behavior; nearest-target search for the player's auto-aim |
| **UI** | Main menu, pause menu, HUD (health/wave/score), game-over screen — all gamepad-navigable |

## Scope (in and out)

**In scope:** joystick-controlled player with auto-aim, one zombie type, three waves, HUD, audio, menus, animations, ScriptableObject for enemy stats, WebGL build.

**Out of scope:** multiple enemy types, weapon switching, power-ups, dash, multiple levels, custom shaders, particle effects beyond muzzle flash.

---

## The Three Milestones

These drive blog posts #3, #4, and #5.

**Milestone 1 — Core Loop.** *"It is technically a game."* Player moves around the arena. The weapon auto-aims and fires at zombies. One zombie chases and damages the player. Bullets kill the zombie. Player can die. No HUD, no waves, no audio — just the verbs working end-to-end. The goal is to prove the core loop *feels* right in grey-box form before adding anything on top.

**Milestone 2 — Wave System + HUD.** *"It is a real game now."* The full three-wave spawner. Health, wave counter, and score visible on the HUD. Basic audio (gunshot, zombie groan, hit, death). Game-over and win screens. By the end of this milestone, the game is fully playable start-to-finish.

**Milestone 3 — Polish + Architecture.** *"It is shippable."* Main menu and pause menu. ScriptableObject extracted for zombie stats. Code refactored against SOLID principles. Animations on the player and zombie. WebGL build verified on GitHub Pages.

---

## Reflection

The hardest part of this stage was **cutting**. Every time I described a mechanic, I wanted to add an exception or a variant — a tanky zombie, a second weapon, waves with different spawn patterns. All reasonable ideas, none of them necessary.

The MDA framework helped me make those calls. Once "powerful" was locked in as the target aesthetic, any feature that didn't directly serve that feeling was cut. Tanky enemies *reduce* the feeling of power. Weapon switching *interrupts* the flow of killing. A clean, focused design beats a sprawling one.

A bigger lesson came from the controls. My first instinct was a traditional twin-stick shooter (one stick to move, one to aim), but the VIA Arcade Machine only has one joystick per player. Designing around the hardware instead of against it forced the switch to auto-aim — which turned out to be a *better* fit for the "feel powerful" goal anyway. Positioning matters; aiming becomes pure flow.

The next post (#3) covers Milestone 1 — getting from an empty Unity project to a working core loop.
