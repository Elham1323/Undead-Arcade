# Blog Post #5 — Milestone 3: ScriptableObjects, Menus, Animations, and Deployment

*Project: Undead Arcade, Game Development course, May 2026*

Milestone 3 covered a ScriptableObject for zombie stats, a pause menu, a main menu scene, character animations, arena polish, and a WebGL build hosted on GitHub Pages.

## ScriptableObject and Pause Menu

Zombie stats (health, damage, attack range, attack interval) used to live as public fields on ZombieController. I moved them into a ZombieData ScriptableObject so each zombie variant can be its own asset file. Speed is still controlled by the WaveManager so wave difficulty stays independent of zombie type.

The pause menu has three buttons: Resume, Restart, and Main Menu, toggled with ESC. My first version used Time.timeScale = 0, which broke UI clicks because of a known issue with Unity's new Input System. The fix was to disable the individual components that produce movement and sound instead (PlayerController, ZombieControllers, NavMeshAgents, AudioSources). The Time.timeScale value stays at 1.

One small bug remains. On the first pause after entering the scene, zombies sometimes still play a groan, or their walk animation keeps animating even though they are not moving. I tried disabling whole GameObjects instead of components, including inactive objects in FindObjectsByType, and stopping coroutines explicitly. None of these fully removed it. I did not see this as critical enough to spend more hours on after the time I had already put in, but I believe I could fix it with more time and more research into when zombies are spawned.

## Main Menu

I created a separate scene called MainMenu with a Play button and a Quit button. Both scenes had to be added to Build Settings or SceneManager.LoadScene fails silently with no error message. The menu has a dark background, a blood-red title with a red shadow effect, and a looping ambient music track downloaded from Pixabay.

## Animations

Animations came from Mixamo. I imported a SWAT character for the player and the Romero zombie, both with idle, walk, attack, and death animations. The idle, walk, and attack states work. The death animation does not.

When a zombie dies the Animator transitions to the Death state, the clip is assigned, the avatar maps correctly, and the FBX contains animation curves. But the model only shows the first frame of the animation and locks in that pose. I tried re-downloading the FBX with skin included, switching shaders, every combination of the Bake Into Pose toggles, turning off Apply Root Motion, and forcing Death as the default state to confirm the animation existed. Forcing Death as default showed the zombie locked in its final pose instead, which suggests the curves are present but not playing. The cause is still unknown to me. As a workaround I shortened the destroy delay so the zombie disappears quickly after dying, which in normal play reads as a fast death rather than a freeze.

The shoot animation has a separate issue. Mixamo's firing rifle animation fires on every bullet, but at the player's fast fire rate it looks like the SWAT character is lowering his arms and shaking instead of aiming. I considered disabling it but decided to keep it and document the limitation. The bullets themselves convey the shooting well enough.

Textures needed manual setup. After importing, both characters appeared in flat grey because the materials existed but the textures were not assigned to them yet. Unity has Extract Materials and Extract Textures buttons that pull the textures out of the FBX so they can be assigned by hand. The SWAT head material refused to display its texture under the Standard shader. Switching that one material to Mobile/Diffuse fixed it. I do not know why Standard failed.

## Arena Polish

The arena started as a white box. I first tried a dark concrete look with fog for a creepy mood, but it ended up too dark to see the zombies clearly. I changed direction to a bright daylight forest theme. The floor became grass green, the walls forest brown, and I added seven trees built from cylinder trunks with sphere foliage on top. Each tree has a NavMesh Obstacle with Carve enabled, which updates the NavMesh at runtime so zombies path around the trees instead of walking through them. A few small bushes were added for variety.

I also added a primitive rifle as a child of the SWAT's right hand bone. Mixamo's rifle animations pose the hands as if holding a rifle but do not include the rifle model itself. The rifle is a dark elongated cube positioned to sit in the hand. The camera was moved slightly closer to the player, and the bullets were resized smaller with a brass color so they are easier to see in flight.

## WebGL Build and Hosting

Switching the build target to Web produced a folder containing index.html, Build, and TemplateData. I copied this into a docs folder at the repo root, which GitHub Pages serves directly. The first push was rejected because the Mixamo FBX files are over 100MB, which is GitHub's per-file limit. The fix was to install Git LFS and configure it to track all .fbx files. The push then went through.

The build is live at https://elham1323.github.io/Undead-Arcade/. The Quit button on the main menu freezes the page rather than closing it. WebGL has no concept of quitting an application, only of closing a browser tab, so this is documented as a known WebGL limitation rather than a bug.

## Status

Three known bugs:
- Zombies occasionally play a groan or keep their walk animation playing on the first pause after a scene loads
- The zombie death animation only shows the first frame instead of the full falling motion
- The shooting animation does not fit the player's fast fire rate

Next is the arcade test and the final blog post.
