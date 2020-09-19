using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float bulletForce = 20f;

    public Transform firePoint;

    public GameObject bulletPrefab;

    public AudioClip loudSound;

    public AudioClip silentSound;

    private AudioSource bulletSound;

    public int SilencedShots { get; private set; }

    public bool IsFiring { get; private set; }

    public bool SilencerBroken { get; private set; }

    private void Start()
    {
        bulletSound = GameObject.Find("Gunfire Silenced").GetComponent<AudioSource>();
        SilencedShots = DataDriven.PistolMaxSilencedShots;
        SilencerBroken = false;
    }

    public bool Shoot()
    {
        if (!IsFiring)
        {
            IsFiring = true;
            StartCoroutine(SetFiringFalse());

            if (SilencedShots > 0)
            {
                SilencedShots--;
            }
            else if (!SilencerBroken)
            {
                SilencerBroken = true;
                bulletSound = GameObject.Find("Gunfire").GetComponent<AudioSource>();
            }

            GameObject bullet =
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletSound.Play();
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(),
                GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());

            // get the instantiated bullet's rigidbody component, so we can
            // add physics to it such as more force
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

            Destroy(bullet, .5f);
            return true;
        }

        return false;
    }

    IEnumerator SetFiringFalse()
    {
        yield return new WaitForSeconds(0.2f);
        IsFiring = false;
    }
}






