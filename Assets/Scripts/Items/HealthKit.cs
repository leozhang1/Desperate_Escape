using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : Item
{
   protected override void OnPlayerTrigger(GameObject player)
    {
        player.GetComponent<PlayerStats>().RestoreHealth(DataDriven.HealthKitHealValue);
        Debug.Log("Restored " + DataDriven.HealthKitHealValue + " health to player.", this.gameObject);
    }
}
