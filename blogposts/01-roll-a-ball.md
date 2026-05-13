# Blog Post #1 — Roll-a-Ball: First Steps in Unity

*Project: Undead Arcade — Game Development course*

Before starting work on my own game, the course requires going through Unity's official Roll-a-Ball tutorial. This post walks through what I built, what I learned at each stage, and what I'm carrying forward into my main project.

## Setup

I installed Unity Hub and Unity 6.4 LTS (6000.4.5f1) with both WebGL Build Support and Windows Build Support (IL2CPP) modules — needed later for the WebGL build hosted on GitHub Pages and for the arcade machine executable.

I also created a public GitHub repo called Undead-Arcade with a Unity-specific `.gitignore` and an MIT license. The `.gitignore` is critical — Unity generates massive cache folders that should never be committed.

For the tutorial itself, I made a separate Unity project with the URP Empty Template, since Roll-a-Ball is throwaway work and shouldn't share a repo with the actual project.

## Part 1 — Building the Play Area

The first part was setting up the scene with a plane (the ground), a sphere (the player), a directional light, and the sky/sun environment that comes with URP. I used the scale tool to make the ground bigger by stretching the X and Z axes — and learned that scaling the Y axis on a plane does nothing visually unless it goes negative.

I also created two materials: one for the ground and one for the ball. Adjusting the ball's smoothness changed how light reflected off it, which made the shadow much more visible underneath. This was the first time I realised how much lighting and material settings affect the feel of a scene.

## Part 2 — Moving the Player

This part introduced Rigidbody, scripts, and Unity's new Input System. I added a Rigidbody to the ball and wrote PlayerController.cs with an OnMove function that applies force in FixedUpdate. The ball moved... but barely. It crawled across the plane like it was stuck in syrup.

The fix was to add a `public float speed` field and multiply the input vector by it: `rb.AddForce(movement * speed)`. Setting speed = 10 in the Inspector made the ball roll normally. **Lesson: public fields in the Inspector override the script's default value.**

I also hit my first capitalization bug — `getComponent` instead of `GetComponent`. C# is case-sensitive, and Unity's API uses PascalCase. Later I'd hit it again with `OntriggerEnter` vs `OnTriggerEnter`, except this time Unity didn't throw an error — the pickups just silently never disappeared. **When something doesn't work and there's no error, suspect a misspelled Unity callback first.**

## Part 3 — Camera Control

My first instinct was to make the camera a child of the player. Bad idea — every tiny wobble of the ball was inherited by the camera and the view became unwatchable. I moved the camera back out and wrote a CameraController script that computes an offset at Start and applies it in LateUpdate. Using LateUpdate matters — it runs after Update, so the camera always sees the player's latest position rather than lagging a frame behind.

## Parts 4-5 — Walls and Pickups

Walls are just stretched cubes with default colliders, so the ball bounces off naturally — no extra code. Pickups are cubes with a Rotator script (`transform.Rotate(... * Time.deltaTime)` — the Time.deltaTime multiplier is essential to keep speed framerate-independent) turned into prefabs so I could reuse them. I placed 12 in a circle around the player by typing positions directly into the Inspector — dragging in 3D view kept sliding cubes through the floor.

## Part 6 — Game State

This part introduced OnTriggerEnter, tags, and the difference between triggers (detection only) and collisions (physical contact). Pickups got Is Trigger ticked, plus Is Kinematic so they don't fall under gravity. The player script checks `other.gameObject.CompareTag("PickUp")` to identify them, then deactivates them and increments a count. TextMeshPro displays the score, and a hidden "You Win!" text becomes active at 12 collected.

Trying to extend this for a Lose condition revealed another lesson: `winTextObject.text = "You Lose!"` failed because `winTextObject` was a GameObject, not a TextMeshPro component. **GameObjects are containers; behaviour lives in components.** Almost every interaction needs `GetComponent<T>()` first.

## Going Further — NavMesh AI

I extended the tutorial by adding a chasing enemy using NavMeshAgent and dynamic obstacles with NavMesh Obstacle (Carve enabled). The whole `SetDestination(player.position)` thing is one line — but the setup took longer than the rest of the tutorial combined. Unity 6.4 changed NavMesh behaviour from the tutorial's older version, and I spent real time debugging weird agent movement before it worked.

**Lesson: when a tutorial is even a year or two old, expect to spend serious time reconciling Unity version differences.**

## What I'm Taking to Undead Arcade

- Rigidbody + AddForce for player movement
- Triggers vs collisions for pickups vs walls
- Prefabs for enemies, bullets, and pickups
- NavMeshAgent for zombie AI
- TextMeshPro UI for score and health
- Tags for object identification
- Trust the Inspector over hardcoded values

The biggest non-technical lesson was scope management. I went past the core tutorial because the NavMesh material is directly relevant to my game — but it cost me time. Going forward I need to recognise when something is "done enough" and move on.

Next: Blog Post #2 — Game Design Document and three milestones for Undead Arcade.
