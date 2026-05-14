# Blog Post #3 — Milestone 1: Getting the Core Loop Working

*Project: Undead Arcade — Game Development course, May 2026*

Milestone 1 was supposed to be the easy one. The plan: player moves, player shoots, one zombie chases, bullets kill the zombie, zombie kills the player. No UI, no waves, no audio. Just the core verbs working. The whole point was to find out if the game is fun before spending any time polishing it.

It mostly worked out. But there were a few moments along the way where I had to stop and figure out what I was actually doing, and one big design decision that completely changed how the game plays.

## The zombie that flew off the map

After setting up the arena and adding a zombie capsule with a NavMeshAgent, I hit Play expecting to see it walk toward the player. Instead it just shot off the side of the arena and fell into the void. My first thought was "I forgot to bake the NavMesh." But I checked, and there was a blue NavMesh overlay on the ground, so it *had* been baked.

When I looked at the Ground GameObject more carefully, the NavMeshSurface component was just gone. The blue overlay I was seeing was a leftover from the previous bake but there was no component actually generating it. Re-adding the NavMeshSurface and clicking Bake again fixed it instantly.

Not sure what removed it. Possibly something I clicked, possibly Unity weirdness from when I was setting tags. **Lesson: when something doesn't behave, don't just check that the component is configured right. Check that the component is even still there.**

## The fire button that wouldn't stop firing

This one took me a while. I set up shooting with the new Input System, using the magic `OnFire(InputValue value)` method that gets called automatically by the PlayerInput component. The idea was simple: when Fire is pressed, `isFiring = true`. When released, `isFiring = false`. Then `Update()` would spawn bullets while `isFiring` was true.

Pressing the button worked. Releasing it didn't. The player just kept firing forever after one click.

Turns out the default "Fire" action in the Input Actions asset is configured as a `Button` action type, and Button actions only fire the callback *on press*, not on release. So `OnFire` was being called once when I clicked, setting `isFiring = true`, and then never being called again. The release event was just never reaching my script.

I tried changing the Action Type to `Value`, but Unity 6 doesn't have a clean "Button" control type for Value actions. The dropdown options are things like Axis, Digital, Vector2, etc. I didn't want to fight the Input Actions editor for something that should be simple.

The fix that actually worked was to ignore the `OnFire` callback entirely and just poll the action state directly every frame. In `Update()`, I now check `fireAction.IsPressed()`, which returns true while the button is held and false the moment it's released, regardless of how the action is configured. Same Input System, different API. Way more reliable.

**Lesson: the "magic" callbacks are great when they work, but when they don't, going one layer deeper to the polling API is often less effort than reconfiguring the asset.**

## Killing my own design

The GDD I wrote in Blog #2 said the player would use auto-aim. The gun automatically targets the nearest zombie and fires when you press the button. I picked that because the VIA Arcade Machine only has one analog stick per player, so a traditional twin-stick shooter (one stick to move, one to aim) wasn't possible.

When I got to actually implementing it, I stopped and thought about whether auto-aim was really the right choice. The case against it: it makes the player passive, since they walk around and the gun does the work. The case for it: it's the simplest single-stick solution.

I ended up going with a different approach. I called it walk-and-shoot. Bullets fire in the direction the player is moving. If you stop, you keep facing whatever direction you last walked, so you can still shoot. This is the *Smash TV* / *Robotron* model, old-school arcade design that fits one-stick hardware perfectly.

It plays much better than auto-aim would have. You have to angle your movement to line up shots, dodge backward while spraying forward, kite zombies in circles. There's actual skill expression even with one stick. And it still feels powerful, because the gun is fast and zombies die in a few hits, so the "Doom-lite" aesthetic from the GDD is intact.

The GDD already said it should evolve as I built the game. This is exactly the kind of change it was talking about, a design assumption that didn't survive contact with reality. Auto-aim isn't *wrong*, it's just *less interesting* for this game.

## Health as a single reusable component

One thing I tried to do right from the start was structure the code so it could grow. The Health system is the main example. Instead of writing one health system for the Player and a separate one for the Zombie, I made a single `Health.cs` script that both use. The Player has it with 100 HP, the Zombie has it with 40 HP. Same script, different numbers in the Inspector.

Health doesn't know who can be damaged or what dying means. It just tracks HP and fires a `UnityEvent OnDied` when HP hits zero. The Zombie's controller listens to that event and decides "for me, dying means stopping pathfinding and destroying the GameObject." The GameManager listens to the Player's event and freezes the game. Each script does one thing.

This is the single-responsibility principle from the architecture lecture, and the Observer pattern (the death event) from Game Programming Patterns. They felt abstract when I read about them. Putting them into a real project where I actually had to make decisions made them click.

## Where the game is now

The core loop works. You move, you shoot in the direction you're walking, the zombie chases you, you can kill it in four hits, it can kill you in five seconds of contact. The game freezes when you die (placeholder for a proper Game Over screen later).

It's not pretty. Everything is grey capsules and white walls. But the verbs work and it already feels fun in a way I didn't expect from grey-box. That's the whole point of Milestone 1.

Next: Milestone 2, the wave spawner, full HUD, audio, and the game becoming something I'd actually want to play.
