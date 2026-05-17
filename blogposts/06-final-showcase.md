# Blog Post #6 — Final Showcase
*Project: Undead Arcade, Game Development course, May 2026*

This is the final blog post for Undead Arcade. The project is complete and ready for submission.

## The Final Product

Undead Arcade is a single-player top-down 3D arcade zombie shooter. The player controls a SWAT character with a rifle and has to survive four waves of escalating zombies in a forest arena. Bullets fire in the direction the player is moving, so positioning matters as much as aiming.

The game is playable in three places:
- Browser: https://elham1323.github.io/Undead-Arcade/
- YouTube demo: https://youtu.be/ezYFiHTjq8Q
- GitHub repo and Windows build: https://github.com/Elham1323/Undead-Arcade

The Windows build is also installed on the VIA Arcade Machine, in the Games folder on the desktop with a shortcut in the Game shortcuts folder. It is also added to Steam as a non-Steam game and launchable from there.

## The Arcade Test

The Windows build was transferred to the arcade PC via USB, set up in the Games folder, and added to Steam so the arcade buttons would map correctly through Steam Input. The core gameplay worked on the first try: left joystick to move, right trigger to fire, yellow button (mapped to gamepad north) to pause.

Menu navigation took longer to get working. The default Submit and Cancel actions in Unity's new Input System were bound to a generic device-tagged input that the arcade does not provide. The green button (south) and red button (east) did nothing in menus until I added explicit gamepad bindings to the PlayerInputActions asset.

The joystick still did not navigate menus even though it controlled the player in-game. I wrote a small GamepadMenuNav script that reads the left stick directly and selects the next button via the EventSystem. This works for the main menu but has an unresolved issue in the pause menu where the Restart button is skipped. The button is still reachable via the death screen or by quitting to the main menu.

## Closing

The project met its milestones and the GDD's scope. The architecture decisions made early in the project (reusable Health component, ScriptableObject for zombie stats, event-driven wave system) held up under the full game and made adding features in the later milestones easier than expected.

The biggest lesson was that small problems take far more time than expected. Most of the development hours went into bugs that looked simple at first: a button with a Height of 0, a death animation that played its first frame and locked, an arcade joystick that worked for movement but not menus. Documenting these in the blog posts as they happened, rather than after, kept the focus on what was actually slowing the project down.

Known issues at submission are listed in the README and discussed in the milestone blogs. They are acceptable per the course requirements.

This is the end of the GMD project. Thanks for reading.
