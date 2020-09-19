using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Sprite fullHeart, halfHeart, emptyHeart, noWeapon, activeShot, usedShot;

    [SerializeField] private GameObject[] hearts, silencedShots;
    [SerializeField] private GameObject currentWeapon, ammoText, silencedShotsGroup;

    private GameObject player;
    private PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        fullHeart = Resources.Load<Sprite>("Sprites/UI/HeartFull");
        halfHeart = Resources.Load<Sprite>("Sprites/UI/HeartHalf");
        emptyHeart = Resources.Load<Sprite>("Sprites/UI/HeartEmpty");
        activeShot = Resources.Load<Sprite>("Sprites/UI/SilencedActive");
        usedShot = Resources.Load<Sprite>("Sprites/UI/SilencedUsed");
        noWeapon = Resources.Load<Sprite>("Sprites/NoWeapon");

        player = GameObject.Find("Player");
        stats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeartContainers(stats.Health);
        if (stats.CurrentWeapon != null)
            currentWeapon.GetComponent<Image>().sprite = stats.CurrentWeapon.WeaponSprite;
        else
            currentWeapon.GetComponent<Image>().sprite = noWeapon;

        if (player != null)
            UpdateSilencedShots(player.GetComponent<Shooting>().SilencedShots);

        UpdateAmmoCount(stats.CurrentWeapon);
    }

    /*
     * Helper methods.
     */
    private void UpdateHeartContainers(int health)
    {
        foreach (GameObject heartObj in hearts)
        {
            Image heart = heartObj.GetComponent<Image>();
            switch (health)
            {
                case 0:
                    heart.sprite = emptyHeart;
                    break;
                case 1:
                    heart.sprite = halfHeart;
                    health--;
                    break;
                default:
                    heart.sprite = fullHeart;
                    health -= 2;
                    break;
            }
        }
    }

    private void UpdateSilencedShots(int shotsLeft)
    {
        if (shotsLeft <= 0)
        {
            if (silencedShotsGroup != null)
                Destroy(silencedShotsGroup);
            return;
        }

        if (stats.CurrentWeapon != null && stats.CurrentWeapon.WeaponType == Weapon.Type.Pistol)
        {
            silencedShotsGroup.SetActive(true);
            for (int i = 0; i < silencedShots.Length; i++)
            {
                if (i < shotsLeft)
                    silencedShots[i].GetComponent<Image>().sprite = activeShot;
                else
                    silencedShots[i].GetComponent<Image>().sprite = usedShot;
            }
        }
        else
            silencedShotsGroup.SetActive(false);
    }

    private void UpdateAmmoCount(Weapon currentWeapon)
    {
        Text ammoText = this.ammoText.GetComponent<Text>();
        if (currentWeapon != null)
        {
            if (currentWeapon.IsThrown)
                ammoText.text = "" + (currentWeapon.AmmoStandby);
            else
                ammoText.text = currentWeapon.AmmoLoaded + " / " + currentWeapon.AmmoStandby;
        }
        else
            ammoText.text = "";
    }
}
