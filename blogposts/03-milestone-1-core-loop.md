# Blog Post #3 — Milestone 1: Getting the Core Loop Working
*Project: Undead Arcade — Game Development course, May 2026*

Milestone 1 covered the core verbs of the game: player movement, shooting, one zombie chasing the player, bullets killing the zombie, and the zombie killing the player. No UI, no waves, no audio. The goal was to confirm the game is fun before spending time on polish.

## The NavMesh issue

After setting up the arena and adding a zombie with a NavMeshAgent, the zombie did not chase the player. Instead it slid off the side of the arena and fell out of the world. The blue NavMesh overlay was visible on the ground so I assumed the NavMesh was baked correctly.

The cause was that the NavMeshSurface component on the Ground GameObject was missing. The blue overlay was a leftover from a previous bake but no component was generating it. Re-adding the NavMeshSurface and baking again fixed it. The lesson here was to check that the component is still present, not just that it is configured correctly.

## The fire button not releasing

Shooting was set up with the new Input System, using the OnFire(InputValue) method that the PlayerInput component calls automatically. Pressing Fire registered. Releasing it did not. The player kept firing after one click.

The default Fire action is configured as a Button action type, which only fires the callback on press. OnFire was being called once on press and never again on release. I tried changing the Action Type to Value but the Unity 6 options did not cleanly match the behavior I wanted.

The fix was to skip the callback entirely and poll the action state every frame with fireAction.IsPressed(). This returns true while the button is held and false when released, regardless of the Action Type configuration.

## Design change: walk-and-shoot

The GDD from Blog #2 specified auto-aim. The gun would target the nearest zombie automatically and fire when the player presses the button. This was chosen because the VIA Arcade Machine only has one analog stick per player, which rules out a twin-stick shooter.

During implementation I decided to change this. Auto-aim makes the player passive: they walk around and the gun does the work. The new design is walk-and-shoot: bullets fire in the direction the player is moving, and the player keeps facing the last direction they walked so they can still shoot while standing still. This is the Smash TV / Robotron approach. It fits one-stick hardware and adds skill expression. The GDD said the design should evolve during development, and this was that kind of change.

## Reusable Health component

A single Health.cs script is used for both the Player and the Zombie. The Player has it with 100 HP and the Zombie with 40 HP. Same script, different values in the Inspector.

Health does not know who can be damaged or what dying means. It tracks HP and fires a UnityEvent OnDied when HP reaches zero. The Zombie's controller listens to that event and destroys the GameObject. The GameManager listens to the Player's event and freezes the game. This applies the single-responsibility principle and the Observer pattern from the architecture lecture.

## Where the game is now

The core loop works. The player moves, shoots in their movement direction, the zombie chases, the zombie dies in four hits, the player dies in five seconds of contact. Everything is grey capsules and white walls, but the verbs work and the game already feels fun.

Next is Milestone 2: the wave spawner, full HUD, and audio.
