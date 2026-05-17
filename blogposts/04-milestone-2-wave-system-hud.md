# Blog Post #4 — Milestone 2: Wave System, HUD, and Game Feel
*Project: Undead Arcade, Game Development course, May 2026*

Milestone 1 ended with one zombie chasing one player in a grey box. Milestone 2 turned that into a playable arcade loop. The plan was a wave spawner, score, HUD, Game Over screen, Win screen, and basic audio. By the end of the milestone the game can be played start to finish, with either a survive or die ending.

## The Wave System

The WaveManager is a coroutine that loops over a List<WaveSettings>, spawns the configured number of zombies at the spawn point farthest from the player, and waits for them all to die before starting the next wave. Each spawned zombie subscribes to its own death event and decrements a counter when it dies. When the counter hits zero the wave is over. This is the same Observer pattern as Milestone 1's player death event, just scaled up.

I set the waves to 7, 13, 17, and 27 zombies with increasing speeds. The first is a warm-up, the second starts to crowd you, and the fourth is intentionally chaotic. Tuning those numbers took longer than writing the code did. The 30-zombie wave I originally planned felt unfair more than challenging, so I dropped it to 27 and bumped the speed instead. Speed feels more threatening than count once there are around 20 zombies on screen.

## HUD

The HUD is three pieces of TextMeshPro plus a Slider for the health bar. Top-left is health, top-center is the wave counter, top-right is the score. A separate HUDManager script listens to events from the Health, WaveManager, and ScoreManager, and updates the UI when each event fires. Nothing in the game logic references the UI directly, and nothing in the UI references the game logic. They only meet at the HUDManager. That separation paid off when I added the score later. It was a five-line change instead of touching three different scripts.

## The Game Over and Win Screens

The end screens caused more issues than expected.

The first issue was with Unity's UI menu itself. When I right-clicked the Canvas to add a Button, the Text child object sometimes did not get created. The button would exist as just an Image with no label. I had to manually add a Text (TMP) child every time.

The second issue was bigger. After wiring up both Restart buttons, the buttons did not respond to clicks. The text changed color on hover, so the mouse was being detected, but actually clicking did nothing. I thought it was an EventSystem issue with the new Input System, since Time.timeScale = 0 is known to break UI input on that combination. I rewrote the GameManager to skip Time.timeScale entirely and disable the player and zombie scripts manually when the game ends.

That did not fix the click issue. The real cause was that the button's Rect Transform had a Height of 0. The text inside was visible because the text has its own raycast area, which is why the hover detection worked. The button itself had no clickable surface. Setting the Height to 80 fixed it instantly.

The GameManager rewrite turned out to be a useful change anyway. Freezing the game by disabling scripts is cleaner than Time.timeScale = 0 because it does not affect UI input or coroutines in unexpected ways.

The hover effect on the restart buttons is also a custom script (ButtonHoverColor) instead of Unity's built-in Color Tint transition. The built-in version has a known bug in Unity 6 where assigning a TMP text as the Target Graphic crashes the Inspector. The custom script avoids the bug entirely.

## Audio

Four sounds were added: gunshot, zombie groan, hit, and death. All were downloaded from Pixabay. The gunshot plays from the Player's AudioSource as 2D audio. The zombie groan plays from each Zombie's AudioSource as 3D audio with Linear Rolloff and a 60-unit max distance, so groans fade naturally with distance. Each zombie schedules its first groan within a second of spawning, then picks a random delay between 3 and 8 seconds for subsequent ones.

The hit sound caused a small detour. I first played it with AudioSource.PlayClipAtPoint, but that adds noticeable lag because of 3D spatial processing. I switched to playing it through the Player's 2D AudioSource. There was still a delay, and checking the waveform in the Inspector showed the MP3 file had a quiet pre-roll before the impact sound. I trimmed it on mp3cut.net and re-imported it. The hit feedback is responsive now.

The death sound is a single line of code in GameManager. When the player dies, it plays the Death clip through the Player's AudioSource.

## Where the game is now

Four waves of escalating zombies, a full HUD, working Game Over and Win screens with hover-coloured Restart buttons, and audio for the four key events. The game is fully playable end to end.

Next is Milestone 3, which is the polish pass: a main menu, a pause menu, ScriptableObjects for zombie stats, animations, and a WebGL build hosted on GitHub Pages.
