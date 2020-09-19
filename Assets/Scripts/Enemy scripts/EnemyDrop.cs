using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] GameObject pistolPrefab, grenadePrefab;
    [SerializeField] private float pistolDropWeight, grenadeDropWeight, noDropWeight;
    private float pistolDropIndex, grenadeDropIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (pistolDropWeight < 0)
        {
            Debug.LogWarning("Pistol drop weight cannot be below 0. Setting it to 0!");
            pistolDropWeight = 0;
        }

        if (grenadeDropWeight < 0)
        {
            Debug.LogWarning("Grenade drop weight cannot be below 0. Setting it to 0!");
            grenadeDropWeight = 0;
        }

        if (noDropWeight < 0)
        {
            Debug.LogWarning("Drop nothing weight cannot be below 0. Setting it to 0!");
            noDropWeight = 0;
        }
    }

    public void OnDeath()
    {
        CalculateWeights();

        //Get terrorist's position.
        Vector3 terroristPosition = gameObject.GetComponent<Transform>().position;
        Vector3 itemDropPosition = new Vector3(terroristPosition.x, terroristPosition.y, -1);
        Transform itemsBranch = GameObject.Find("Items").GetComponent<Transform>();

        bool isGuaranteedDrop = GameObject.Find("Player").GetComponent<PlayerStats>().CurrentWeapon == null && GameObject.FindGameObjectsWithTag("Drop").Length == 0;

        float rng = Random.value;
        if (isGuaranteedDrop || rng < pistolDropIndex)
        {
            GameObject pistol = Instantiate(pistolPrefab, itemDropPosition, pistolPrefab.GetComponent<Transform>().rotation, itemsBranch);
            Debug.Log("Dropped a pistol with " + pistol.GetComponent<WeaponDrop>().Ammo + " ammo.");
        }
        else if (rng < grenadeDropIndex)
        {
            GameObject grenade = Instantiate(grenadePrefab, itemDropPosition, grenadePrefab.GetComponent<Transform>().rotation, itemsBranch);
            Debug.Log("Dropped a grenade.");
        }
        else
            Debug.Log("No weapon has been dropped.");
    }

    private void CalculateWeights()
    {
        PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        Dictionary<Weapon.Type, int> ammoCollection = playerStats.GetAmmoCollection();

        if (ammoCollection == null)
        {
            return;
        }

        //Calculate total ammo based on player's current inventory and dropped pistols.
        GameObject[] drops = GameObject.FindGameObjectsWithTag("Drop");
        int totalAmmo = (ammoCollection.ContainsKey(Weapon.Type.Pistol) ? ammoCollection[Weapon.Type.Pistol] : 0);
        if (drops.Length > 0)
        {
            foreach (GameObject drop in drops)
            {
                if (drop.GetComponent<WeaponDrop>().GetWeaponType() == Weapon.Type.Pistol)
                    totalAmmo += drop.GetComponent<WeaponDrop>().Ammo;
            }
        }

        //Calculate weight adjustment based on player ammo.
        float pistolAmmoAdjust = DataDriven.PistolMaxAmmoPouch / (1f + 2f * Mathf.Pow(totalAmmo, 17f/18f));
        pistolDropWeight *= pistolAmmoAdjust;
        grenadeDropWeight /= (pistolAmmoAdjust / 3f);
        noDropWeight /= (pistolAmmoAdjust / 3f);

        //Normalize weights.
        float totalWeight = pistolDropWeight + grenadeDropWeight + noDropWeight;
        pistolDropWeight /= totalWeight;
        grenadeDropWeight /= totalWeight;
        noDropWeight /= totalWeight;

        Debug.Log("Pistol Drop Weight: " + pistolDropWeight, this);
        Debug.Log("Grenade Drop Weight: " + grenadeDropWeight, this);
        Debug.Log("Drop Nothing Weight: " + noDropWeight, this);

        //Set drop indexes for RNG drops.
        pistolDropIndex = pistolDropWeight;
        grenadeDropIndex = grenadeDropWeight + pistolDropWeight;
    }
}
