# Blog Post #4 — Milestone 2: Wave System, HUD, and Game Feel

*Project: Undead Arcade, Game Development course, May 2026*

Milestone 1 ended with one zombie chasing one player in a grey box. That was enough to prove the core loop works. Milestone 2 was the milestone where the project actually starts to feel like a game. The plan: wave spawner, score, HUD, Game Over screen, Win screen, and basic audio. By the end of it, you can boot the project, play start to finish, and either survive or die. That is what shipped.

## The Wave System

The WaveManager was easier than I expected. It is just a coroutine that loops over a `List<WaveSettings>`, spawns the configured number of zombies at the spawn point farthest from the player, and waits for them all to die before starting the next wave. The trick was using events for the "wave done" signal instead of polling. Each spawned zombie subscribes to its own death event, decrements a counter when it dies, and when the counter hits zero the wave is over. Same pattern as Milestone 1's player death event, just scaled up.

I set the waves to 7, 13, 17, and 27 zombies with increasing speeds. The first wave is a warm-up, the second one starts to crowd you, and the fourth is intentionally chaotic. Tuning those numbers took longer than writing the code did. The 30-zombie wave I originally planned felt unfair more than challenging, so I dropped it to 27 and bumped the speed instead. Speed feels more threatening than count once you have around 20 zombies on screen.

## HUD

The HUD is three pieces of TextMeshPro plus a Slider for the health bar. Top-left is health, top-center is the wave counter (Wave 1 / 4), top-right is the score. A separate HUDManager script listens to events from the Health, WaveManager, and ScoreManager, then updates the UI when each event fires. Nothing in the game logic knows the UI exists. Nothing in the UI knows the game logic exists. They only meet at the HUDManager. That separation is one of the things the architecture lecture pushed hard, and now I see why. Adding the score later was a five-line change instead of touching three different scripts.

## The Game Over and Win Screens

These were supposed to be quick. They were not.

The first problem was Unity itself. When I right-clicked the Canvas to add a Button, sometimes the Text child object did not get created. The button would exist as just an Image with no label. I had to manually add a Text (TMP) child every time. That alone cost me thirty minutes of confusion the first round.

The second problem was bigger. After wiring up both Restart buttons, the buttons did not respond to clicks. The text changed color on hover (so the mouse was clearly registering), but actually clicking did nothing. I thought it was an EventSystem issue with the new Input System, since `Time.timeScale = 0` is known to break UI input on that combo. I rewrote the GameManager to stop using `Time.timeScale` and instead disable the player and zombie scripts manually when the game ends.

That did not fix it.

The real cause was that the button's Rect Transform had a Height of 0. The text inside was visible because the text has its own raycast area, which is why the hover detection worked. But the button itself had no clickable surface. Setting the Height to 80 fixed it instantly. I had been staring at the right component the whole time and missed the number. Frustrating, but I learned to read the Rect Transform values carefully before assuming something deeper is wrong.

The rewrite of GameManager turned out to be a good change anyway. Freezing the game by disabling scripts is cleaner than `Time.timeScale = 0`, because it does not affect the UI input or coroutines in unexpected ways. The hover effect on the restart buttons is also a custom script (`ButtonHoverColor`) instead of Unity's built-in Color Tint transition, because the built-in version has a known bug in Unity 6 where assigning a TMP text as the Target Graphic crashes the Inspector. Bypassing it was simpler than fighting it.

## Audio

Four sounds: gunshot, zombie groan, hit, and death. All from Pixabay, all free, all CC0. The gunshot plays from the Player's AudioSource as 2D audio. The zombie groan plays from each Zombie's AudioSource as 3D audio with Linear Rolloff and a 60-unit max distance, so groans fade naturally with distance and you can tell roughly where a zombie is just from the sound. Each zombie schedules its first groan within a second of spawning, then picks a random delay between 3 and 8 seconds for subsequent groans. With twenty zombies on screen the audio gets satisfyingly busy.

The hit sound caused a small detour. I first played it with `AudioSource.PlayClipAtPoint`, but that adds noticeable lag because of 3D spatial processing. I switched to playing it through the Player's 2D AudioSource, which was instant. Then I noticed there was still a delay, and after checking the waveform in the Inspector I realized the actual MP3 file had a quiet pre-roll before the impact sound. I trimmed it on mp3cut.net and re-imported it, and now the hit feedback is snappy.

The death sound is one line of code in GameManager: when the player dies, it plays the Death clip through the same Player AudioSource.

## Where the game is now

Four waves of escalating zombies. Full HUD. Working Game Over and Win screens with hover-coloured Restart buttons. Audio for the four most important events. The game is fully playable end to end and already loops in a satisfying way.

Next is Milestone 3, which is the polish pass: a main menu, a pause menu, ScriptableObjects for zombie stats, animations, and a WebGL build hosted on GitHub Pages.
