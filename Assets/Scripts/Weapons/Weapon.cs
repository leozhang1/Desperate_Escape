using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private int ammoStandby;
    
    public Sprite WeaponSprite { get; private set; }

    public Type WeaponType { get; private set; }

    public int AmmoLoaded { get; private set; }

    public int AmmoStandby 
    {
        get
        {
            return ammoStandby;
        }
        set
        {
            ammoStandby = value;
            if (ammoStandby > PouchCapacity)
            {
                Debug.LogWarning("You are taking more ammo than you can hold. " + (ammoStandby - PouchCapacity) + " " + WeaponType + " has been discarded.");
                ammoStandby = PouchCapacity;
            }
        }
    }

    public bool IsThrown
    {
        get
        {
            return MagazineCapacity == 0;
        }
    }

    public int MagazineCapacity
    {
        get
        {
            switch (WeaponType)
            {
                case Type.Pistol:
                    return DataDriven.PistolMaxAmmoLoaded;
                default:
                    return 0;
            }
        }
    }

    public int PouchCapacity
    {
        get
        {
            switch (WeaponType)
            {
                case Type.Pistol:
                    return DataDriven.PistolMaxAmmoPouch;
                case Type.Grenade:
                    return DataDriven.GrenadeMaxAmmo;
                default:
                    return 0;
            }
        }
    }

    public float ReloadTime
    {
        get
        {
            switch (WeaponType)
            {
                case Type.Pistol:
                    return DataDriven.PistolReloadTimer;
                case Type.Grenade:
                    return DataDriven.GrenadeCooldown;
                default:
                    return 0;
            }
        }
    }

    public Weapon(Type type, int ammo)
    {
        WeaponType = type;
        AmmoLoaded = ammo;
        if (AmmoLoaded > MagazineCapacity)
        {
            AmmoStandby = AmmoLoaded - MagazineCapacity;
            AmmoLoaded = MagazineCapacity;
        }

        switch (WeaponType)
        {
            case Type.Pistol:
                WeaponSprite = Resources.Load<Sprite>("Sprites/pistol3");
                break;
            case Type.Grenade:
                WeaponSprite = Resources.Load<Sprite>("Sprites/grenade_icon");
                break;
        }
    }

    public bool Fire()
    {
        if (!IsThrown)
        {
            if (AmmoLoaded > 0)
            {
                Shooting shooting = GameObject.Find("Player").GetComponent<Shooting>();
                if (shooting.Shoot())
                    AmmoLoaded--;
                else
                    return false;
                if (shooting.SilencedShots <= 0 && WeaponType == Type.Pistol)
                    WeaponSprite = Resources.Load<Sprite>("Sprites/pistol2");
                return true;
            }

            //This part will run when AmmoLoaded <= 0 since the function did not return yet.
            GameObject.Find("Game Manager").GetComponent<AudioManager>().PlaySound(AudioManager.SoundEffect.FireEmpty);
            return false;
        }
        else
        {
            if (AmmoStandby > 0 && GameObject.Find(DataDriven.thrownObjName) == null && GameObject.Find(DataDriven.explosionName) == null)
            {
                //Throw weapon if one isn't already thrown and visible.
                AmmoStandby--;
                switch (WeaponType)
                {
                    case Type.Grenade:
                        Grenade.Spawn(GameObject.Find("Player"));
                        break;
                }
                return true;
            }

            //This part will run when AmmoStandby <= 0 and a thrown weapon is not found since the function did not return yet.
            return false;
        }
    }

    public bool CanReload()
    {
        return AmmoLoaded < MagazineCapacity && AmmoStandby > 0;
    }

    public void Reload()
    {
        int reloadAmount = Mathf.Min(MagazineCapacity - AmmoLoaded, AmmoStandby);
        AmmoLoaded += reloadAmount;
        AmmoStandby -= reloadAmount;
    }

    public enum Type
    {
        Pistol,
        Grenade
    }
}
