using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private Animator animator;
    private AudioManager aManager;

    private int health, currWeaponIdx;
    private List<Weapon> weapons;

    public void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        aManager = GameObject.Find("Game Manager").GetComponent<AudioManager>();

        health = DataDriven.PlayerBaseHealth > LevelManager.PlayerHealth ? DataDriven.PlayerBaseHealth : LevelManager.PlayerHealth;
        weapons = new List<Weapon>();
        if (LevelManager.PlayerWeapons != null)
            foreach (Weapon.Type weaponType in LevelManager.PlayerWeapons.Keys)
                LootAmmo(weaponType, LevelManager.PlayerWeapons[weaponType]);
    }

    public Weapon CurrentWeapon
    {
        get
        {
            if (weapons == null || weapons.Count == 0)
                return null;
            return weapons[currWeaponIdx];
        }
    }

    /*
     * Return the player's current health.
     */

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health > DataDriven.PlayerMaxHealth)
                health = DataDriven.PlayerMaxHealth;
            else if (health <= 0)
            {
                health = 0;
                Particles.Spawn(gameObject, Particles.Type.BloodHeavy, Vector3.zero);
                Destroy(gameObject);
                Debug.Log("Player has died. Triggering game over.");

                //Get number of terrorists chasing player.
                GameObject[] terrorists = GameObject.FindGameObjectsWithTag("Terrorist");
                int count = 0;
                foreach (GameObject terrorist in terrorists)
                    if (terrorist.GetComponent<AdvancedFollowPlayer>().enabled)
                        count++;
                GameObject.Find("Game Manager").GetComponent<GameOver>().TriggerGameOver(count);
            }
        }
    }

    public bool IsAnimating { get; private set; }

    /*
     * Restores the player's health by a certain amount.
     * Passing a negative number indicates that the player has taken damage.
     */
    public void RestoreHealth(int restoredHealth)
    {
        Health += restoredHealth;
        if (restoredHealth < 0)
            Particles.Spawn(gameObject, Particles.Type.BloodLight, 0f, -0.5f, -1f, gameObject.transform);
    }

    public IEnumerator Melee()
    {
        if (!IsAnimating)
        {
            IsAnimating = true;
            gameObject.GetComponent<Melee>().Attack();
            yield return new WaitForSeconds(0.5f);
            IsAnimating = false;
        }
    }

    public void Fire()
    {
        if (!IsAnimating && CurrentWeapon != null)
        {
            CurrentWeapon.Fire();
        }
    }

    /*
     * Determines if the player can reload the weapon.
     */
    public bool CanReload()
    {
        if (CurrentWeapon == null)
            return false;
        return CurrentWeapon.CanReload();
    }


    /*
     * Reloads the current weapon.
     */
    public IEnumerator Reload()
    {
        if (!IsAnimating && CurrentWeapon != null)
        {
            IsAnimating = true;
            Debug.Log("Reloading...");
            animator.SetTrigger("Reload");
            aManager.PlaySound(AudioManager.SoundEffect.Reload);
            yield return new WaitForSeconds(CurrentWeapon.ReloadTime);
            CurrentWeapon.Reload();
            IsAnimating = false;
            Debug.Log("Done reloading!");
        } 
    }

    public void CycleWeapon(bool useNext)
    {
        if (weapons.Count > 1)
        {
            currWeaponIdx = (useNext ? currWeaponIdx + 1 : currWeaponIdx - 1);
            if (currWeaponIdx >= weapons.Count)
                currWeaponIdx -= weapons.Count;
            if (currWeaponIdx < 0)
                currWeaponIdx += weapons.Count;
        }
    }

    /*
     * 
     */
    public void LootAmmo(Weapon.Type weapon, int ammo)
    {
        System.Predicate<Weapon> weaponPredicate = weap => weap.WeaponType == weapon;
        if (weapons.Exists(weaponPredicate))
            weapons.Find(weaponPredicate).AmmoStandby += ammo;
        else
        {
            weapons.Add(new Weapon(weapon, ammo));
        }
    }

    public Dictionary<Weapon.Type, int> GetAmmoCollection()
    {
        Dictionary<Weapon.Type, int> ammoCollection = new Dictionary<Weapon.Type, int>();
        if (weapons != null)
            foreach (Weapon weapon in weapons)
                ammoCollection.Add(weapon.WeaponType, weapon.AmmoLoaded + weapon.AmmoStandby);
        return ammoCollection;
    }
}
