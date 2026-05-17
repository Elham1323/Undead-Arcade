# Undead Arcade

A top-down 3D arcade zombie shooter built in Unity 6.4 LTS as part of the Game and Media Design course at VIA University College, May 2026.

The player controls a SWAT character holding a rifle and must survive four waves of escalating zombie hordes in a forest arena. Movement and shooting are walk-and-shoot style: bullets fire in the direction the player is moving, and the player keeps facing the last walked direction when standing still.

## Play the Game

Playable in the browser via GitHub Pages:
https://elham1323.github.io/Undead-Arcade/

## Video Demonstration

YouTube demo (less than 3 minutes):
https://youtu.be/ezYFiHTjq8Q

## Blog Posts

Development was documented as a series of blog posts in the `blogposts/` folder:

- [Blog 1 — Roll-a-Ball: First Steps in Unity](blogposts/01-roll-a-ball.md)
- [Blog 2 — Game Design Document & Milestones](blogposts/02-gdd-and-milestones.md)
- [Blog 3 — Milestone 1: Getting the Core Loop Working](blogposts/03-milestone-1-core-loop.md)
- [Blog 4 — Milestone 2: Wave System, HUD, and Game Feel](blogposts/04-milestone-2-wave-system-hud.md)
- [Blog 5 — Milestone 3: ScriptableObjects, Menus, Animations, and Deployment](blogposts/05-milestone-3-polish-and-deployment.md)
- [Blog 6 — Final Showcase](blogposts/06-final-showcase.md)

## Controls

**Keyboard / Mouse:**
- WASD — Move
- Left mouse button — Fire
- ESC — Pause

**Gamepad / VIA Arcade Machine:**
- Left analog stick — Move
- Black button — Fire
- Green button — Accept
- Yellow Button - Pause

## Third-Party Assets

| Type | Source | License |
|------|--------|---------|
| Character models (SWAT, Romero zombie) | Mixamo | Free for use |
| Character animations | Mixamo | Free for use |
| Sound effects (gunshot, zombie groan, hit, death) | Pixabay | CC0 |
| Background music (main menu) | Pixabay | CC0 |

## Tech Stack

- Unity 6.4 LTS (6000.4.5f1)
- Built-in Render Pipeline
- Unity New Input System
- NavMesh AI Navigation
- Git LFS for large FBX model files

## Known Issues

- Zombie death animation only displays the first frame instead of the full falling motion. Workaround: short destroy delay so the zombie disappears quickly after dying.
- The shooting animation does not visually fit the player's fast fire rate. The bullets convey the shooting action well enough.
- On the first pause after a scene loads, the freeze does not fully take effect. Zombies can still move and play groans. Pressing pause a second time freezes them in place, but their walking animation continues even though they are stationary.
- The Quit button on the main menu freezes the page in the WebGL build. This is a known WebGL limitation as the platform has no concept of quitting an application.
- On the VIA Arcade Machine, the joystick navigates menus correctly between most buttons but skips the Restart button in the pause menu. The button is still reachable by mouse and can also be triggered via the death screen Restart button or by quitting to the main menu. The cause is unknown and could not be identified in the time available.
