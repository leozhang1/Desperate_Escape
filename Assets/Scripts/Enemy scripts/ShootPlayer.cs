using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : MonoBehaviour
{
    private float timeBetweenShots;
    public float startTimeBetweenShots = .5f;
    public GameObject enemyBullet;

    private AudioManager audioManager;

    public Transform firePoint;
    float bulletForce = 20f;

    // Start is called before the first frame update
    void Start()
    {
        timeBetweenShots = startTimeBetweenShots;
        audioManager = GameObject.Find("Game Manager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // add a check for being able to see the player or not in this if-block
        if (timeBetweenShots <= 0)
        {
            if (enemyBullet != null)
            {
                Debug.Log("SHOOTING PLAYER");
                GameObject bullet = Instantiate(enemyBullet, firePoint.position, firePoint.transform.rotation);
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bulletForce * firePoint.up * DataDriven.TerroristBulletSpeed, ForceMode2D.Impulse);

                audioManager.PlaySound(AudioManager.SoundEffect.Gunfire);
            }
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }
}
