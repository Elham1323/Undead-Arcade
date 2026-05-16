# Blog Post #2 — Game Design Document & Milestones
*Project: Undead Arcade — Game Development course, May 2026*

This post contains the v1.0 Game Design Document for the project, Undead Arcade, and the three milestones that will drive development. The GDD is a starting point. The course expects it to evolve during the project.

## The Concept

Undead Arcade is a single-player top-down 3D arcade zombie shooter. The player stands in an arena, zombies attack from all sides, and the goal is to survive three waves. Last wave cleared is a win. Running out of health is a loss.

## Player Experience (Aesthetics)

The reference points are arcade shooters like Doom, not survival horror. The player should feel powerful: the gun is fast, individual zombies are weak, and clearing a horde should feel satisfying rather than scary. The threat comes from the volume of enemies, not from any one zombie being dangerous.

Using the MDA framework from the lectures, the target aesthetic is power fantasy (Sensation and Challenge). The dynamics needed are fast shooting, weak but numerous enemies, and no downtime between targets. The mechanics that produce those dynamics are a high fire-rate weapon that auto-aims at the nearest zombie, zombies that die in one or two hits, and a wave spawner that keeps the screen full of targets.

Working backward from feeling to mechanic kept the design focused. Tanky bosses and weapon-switching were both considered and cut, since both slow down the killing and reduce the feeling of power.

## Gameplay

Objective: survive three waves. Last wave cleared is a win. HP at zero is a loss.

Core loop: wave starts, zombies spawn at arena edges, the player moves around the arena, the gun auto-aims at the nearest zombie, the fire button kills it, kills increase the score, the wave clears, and the next wave begins. Repeat until win or death.

Progression: Wave 1 is light to teach the controls. Wave 2 increases volume. Wave 3 is the chaotic finale with significantly more zombies and no breathing room.

## Platform & Controls

The game ships in two builds from one project: a Windows executable for the VIA Arcade Machine and a WebGL build hosted on GitHub Pages.

The VIA Arcade Machine gives each player one analog joystick and six buttons. A traditional twin-stick shooter is not possible on this hardware. Instead the player moves with the joystick and the gun auto-aims at the nearest zombie. The fire button (mapped to the right trigger of a virtual gamepad) shoots. This is the same control philosophy used by games like Vampire Survivors. The player's job is positioning, not aiming.

For browser play on the WebGL build, a keyboard fallback is provided: WASD to move, left-click or spacebar to fire.

## Mechanics

- **Player:** Rigidbody-based movement controlled by the joystick. The weapon auto-aims at the closest zombie in range. The fire button shoots continuously, rate-limited. Health bar visible at the top of the screen.
- **Zombie:** A single enemy type. Uses a NavMeshAgent to pathfind toward the player and a finite state machine (Chase, Attack, Dead). Dies in one or two hits. Deals damage on touch.
- **Bullets:** Trigger colliders, short lifetime, deal damage on OnTriggerEnter and despawn.
- **Wave system:** A coroutine-driven WaveManager spawns the wave, waits for all zombies to die, then triggers the next one.

## World & Style

A single arcade-style arena (about 25 by 25 units). Visuals are stylised low-poly using free Synty POLYGON Starter Pack assets, bright and readable rather than dark and atmospheric. Top-down camera at about 60 degrees.

## Assets

| Type | Source |
|------|--------|
| 3D models | Synty POLYGON Starter Pack (free) |
| Character animations | Mixamo (free) |
| SFX | Freesound / Pixabay (CC0) |
| Music | Pixabay royalty-free |

All third-party sources will be credited in the README.

## Required Course Topics — Coverage Plan

| Topic | Where it lives |
|-------|---------------|
| Scripting | MonoBehaviours, coroutines for wave timing, C# events for OnPlayerDied / OnWaveCleared |
| Input & Vectors | New Input System; joystick Vector2 converted to Vector3 world movement |
| Physics | Rigidbody player, trigger colliders on bullets, solid colliders on walls |
| Graphics & Audio | URP materials, AudioSources for gunshots and zombie sounds |
| Animation | Animator controllers for player and zombie (idle, walk, attack) |
| Game Architecture | GameManager, WaveManager as managers; EnemyData ScriptableObject; single-responsibility per script |
| Game AI | NavMeshAgent + FSM for zombie behavior; nearest-target search for player auto-aim |
| UI | Main menu, pause menu, HUD (health, wave, score), game-over screen, all gamepad-navigable |

## Scope

In scope: joystick-controlled player with auto-aim, one zombie type, three waves, HUD, audio, menus, animations, ScriptableObject for enemy stats, WebGL build.

Out of scope: multiple enemy types, weapon switching, power-ups, dash, multiple levels, custom shaders, particle effects beyond muzzle flash.

## The Three Milestones

These drive blog posts 3, 4, and 5.

**Milestone 1 — Core Loop.** Player moves around the arena. The weapon auto-aims and fires at zombies. One zombie chases and damages the player. Bullets kill the zombie. Player can die. No HUD, no waves, no audio. The goal is to confirm the core loop feels right in grey-box form before adding anything on top.

**Milestone 2 — Wave System and HUD.** The full three-wave spawner. Health, wave counter, and score visible on the HUD. Basic audio (gunshot, zombie groan, hit, death). Game-over and win screens. By the end of this milestone the game is fully playable start to finish.

**Milestone 3 — Polish and Architecture.** Main menu and pause menu. ScriptableObject extracted for zombie stats. Code refactored against SOLID principles. Animations on the player and zombie. WebGL build verified on GitHub Pages.

## Reflection

The hardest part of writing this GDD was cutting features. Every time I described a mechanic I wanted to add an exception or a variant: a tanky zombie, a second weapon, waves with different spawn patterns. The MDA framework helped me decide. Once "powerful" was locked in as the target aesthetic, any feature that did not directly serve that feeling was cut.

The controls also caused a decision early. The first instinct was a traditional twin-stick shooter, but the VIA Arcade Machine only has one joystick per player. Designing around the hardware instead of against it led to the auto-aim solution, which also fits the power-fantasy goal better than a twin-stick would have.

The next post (Blog 3) covers Milestone 1 and the work of getting from an empty Unity project to a working core loop.
