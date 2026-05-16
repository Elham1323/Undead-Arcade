# Blog Post #1 — Roll-a-Ball: First Steps in Unity
*Project: Undead Arcade — Game Development course*

Before starting work on my own game, the course requires going through Unity's official Roll-a-Ball tutorial. This post covers what I built, the issues I ran into at each stage, and what I am carrying forward into my main project.

## Setup

I installed Unity Hub and Unity 6.4 LTS (6000.4.5f1) with both WebGL Build Support and Windows Build Support (IL2CPP) modules, needed later for the WebGL build on GitHub Pages and for the arcade machine executable.

I also created a public GitHub repo called Undead-Arcade with a Unity-specific .gitignore and an MIT license. The .gitignore is required because Unity generates large cache folders that should not be committed.

For the tutorial itself I made a separate Unity project with the URP Empty Template, since Roll-a-Ball is throwaway work and does not need to share a repo with the actual project.

## Part 1 — Building the Play Area

The first part was setting up the scene with a plane (the ground), a sphere (the player), a directional light, and the URP sky/sun environment. I scaled the ground on the X and Z axes. Scaling the Y axis on a plane has no visible effect unless the value goes negative.

I also created two materials, one for the ground and one for the ball. Adjusting the ball's smoothness changed how light reflected off it, which made the shadow more visible underneath.

## Part 2 — Moving the Player

This part introduced Rigidbody, scripts, and Unity's new Input System. I added a Rigidbody to the ball and wrote PlayerController.cs with an OnMove function that applies force in FixedUpdate. The ball moved but very slowly.

The fix was to add a public float speed field and multiply the input vector by it: rb.AddForce(movement * speed). Setting speed = 10 in the Inspector made the ball roll at the correct speed. Public fields in the Inspector override the script's default value.

There was also a capitalization bug, getComponent instead of GetComponent. C# is case-sensitive and Unity's API uses PascalCase. A similar issue came up later with OntriggerEnter vs OnTriggerEnter, but this one did not throw an error. The pickups just silently never disappeared, since Unity's message system only calls the correctly-named method.

## Part 3 — Camera Control

I first made the camera a child of the player. Every small movement of the ball was inherited by the camera, which made the view unstable. I moved the camera out of the player and wrote a CameraController script that computes an offset at Start and applies it in LateUpdate. LateUpdate runs after Update, so the camera reads the player's final position for the frame rather than the previous one.

## Parts 4-5 — Walls and Pickups

Walls are stretched cubes with default colliders. The ball bounces off naturally without extra code. Pickups are cubes with a Rotator script (transform.Rotate(... * Time.deltaTime), where the Time.deltaTime multiplier keeps the rotation speed framerate-independent). They were turned into prefabs so they could be reused. I placed 12 of them in a circle around the player by typing positions directly into the Inspector. Dragging them in the 3D view kept clipping them through the floor.

## Part 6 — Game State

This part introduced OnTriggerEnter, tags, and the difference between triggers (detection only) and collisions (physical contact). Pickups got Is Trigger enabled, plus Is Kinematic so they do not fall under gravity. The player script checks other.gameObject.CompareTag("PickUp"), deactivates the pickup, and increments a count. TextMeshPro displays the score, and a hidden "You Win!" text becomes active at 12 collected.

Adding a Lose condition revealed another issue. winTextObject.text = "You Lose!" failed because winTextObject was a GameObject, not a TextMeshPro component. GameObjects are containers, and the behaviour lives in components. Most interactions need GetComponent<T>() first.

## Going Further — NavMesh AI

I extended the tutorial by adding a chasing enemy using NavMeshAgent and dynamic obstacles with NavMesh Obstacle (Carve enabled). The SetDestination(player.position) call is one line, but the setup took longer than the rest of the tutorial combined. Unity 6.4 changed NavMesh behaviour from the version the tutorial was written for, and the agent did not move correctly until I worked through the differences.

## What I am Taking to Undead Arcade

- Rigidbody + AddForce for player movement
- Triggers vs collisions for pickups vs walls
- Prefabs for enemies, bullets, and pickups
- NavMeshAgent for zombie AI
- TextMeshPro UI for score and health
- Tags for object identification
- Trust the Inspector over hardcoded values

I went past the core tutorial because the NavMesh material is directly relevant to my game. It cost extra time, but it gave me a working pattern for zombie AI that I can reuse in Undead Arcade.

Next is Blog 2: the Game Design Document and the three milestones for Undead Arcade.
