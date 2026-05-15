using UnityEngine;
// ZombieData is a ScriptableObject that holds zombie stats as a reusable asset.
// Each instance lives in the Project as its own asset file, separate from any scene
// or prefab. The Zombie prefab reads its stats from whichever ZombieData asset is
// assigned to it. This means we can have different zombie variants (fast/weak,
// slow/tanky, etc.) without duplicating the entire prefab.
//
// The [CreateAssetMenu] attribute adds a menu entry under Assets > Create > Undead Arcade
// so we can make new ZombieData assets from the Project panel right-click menu.
[CreateAssetMenu(fileName = "NewZombieData", menuName = "Undead Arcade/Zombie Data")]
public class ZombieData : ScriptableObject
{
    [Header("Health")]
    // How much HP the zombie spawns with.
    public float maxHealth = 40f;

    [Header("Movement")]
    // How fast the zombie moves toward the player.
    public float moveSpeed = 3f;

    [Header("Attack")]
    // How close the zombie has to be to start attacking.
    public float attackRange = 1.5f;
    // How much damage one bite does to the player.
    public float damagePerHit = 20f;
    // How many seconds between bites.
    public float attackInterval = 1f;
}