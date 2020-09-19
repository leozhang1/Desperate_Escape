using UnityEngine;

public class DataDriven : MonoBehaviour
{
    /*
     * Class that contains data-driven elements, denoted by constants.
     * This file should only contain constant or static readonly variables.
     * Please do not define functions here.
     */

    //Strings
    public const string explosionName = "Explosion";
    public const string thrownObjName = "Thrown Weapon";

    //Player
    public const int PlayerMeleeDamage = 1;
    public const int PlayerBaseHealth = 6;
    public const int PlayerInitHealth = 10;
    public const int PlayerMaxHealth = 10;
    public const float PlayerSpeed = 45000f;

    //Terrorist
    public const int TerroristMaxHealth = 5;
    public const int TerroristDamage = 1;
    public const float TerroristAggroRadius = 5f;
    public const float TerroristViewAngle = 45f;
    public const float terroristViewDistance = 10;
    public const float TerroristPatrolSpeed = 8f;
    public const float TerroristChaseSpeed = 15f;
    public const float TerroristBulletSpeed = 5f;

    //Health Kit
    public const int HealthKitHealValue = 2;

    //Pistol
    public const int PistolMinAmmoDrop = 3;
    public const int PistolMaxAmmoDrop = 6;
    public const int PistolMaxAmmoLoaded = 5;
    public const int PistolMaxAmmoPouch = 20;
    public const int PistolMaxSilencedShots = 10;
    public const int PistolDamage = 2;
    public const float PistolBulletSpeed = 5f;
    public const float PistolReloadTimer = 1.25f;

    //Grenade
    public const int GrenadeMaxAmmo = 3;
    public const int GrenadeDamage = 6;
    public const float GrenadeInitVelocity = 30f;
    public const float GrenadeDeceleration = -15f;
    public const float GrenadeRadius = 5f;
    public const float GrenadeTimer = 3f;
    public const float GrenadeCooldown = 10f;

    //Scenes
    public static readonly string[] Levels =
    {
        "Level0",
        "Level1",
        "Level2",
        "Level3",
        "Level4",
        "Level5",
        "Level6",
        "Level7"
    };
}
