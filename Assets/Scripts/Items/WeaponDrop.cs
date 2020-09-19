using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : Item
{
    [SerializeField] private Weapon.Type weaponType;

    public int Ammo { get; private set; }

    private void Start()
    {
        switch (weaponType)
        {
            case (Weapon.Type.Pistol):
                SetAmmoCount(DataDriven.PistolMinAmmoDrop, DataDriven.PistolMaxAmmoDrop);
                break;
            case (Weapon.Type.Grenade):
                SetAmmoCount(1, 1);
                break;
        }
    }

    public Weapon.Type GetWeaponType()
    {
        return weaponType;
    }

    protected void SetAmmoCount(int lower, int upper)
    {
        if (lower == upper)
            Ammo = lower;
        else
            Ammo = System.Convert.ToInt32(Random.value * (upper - lower)) + lower;
    }

    protected override void OnPlayerTrigger(GameObject player)
    {
        player.GetComponent<PlayerStats>().LootAmmo(weaponType, Ammo);
        Debug.Log("Looted " + Ammo + " " + weaponType + " ammo.", this.gameObject);
    }
}
