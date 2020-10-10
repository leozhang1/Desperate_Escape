using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public Image[] hearts;

    // Actual health (this variable determines the number of empty hearts we want)
    [SerializeField] public int healthForHearts;

    private int maxHearts;

    [SerializeField] public Sprite fullHeart;
    [SerializeField] public Sprite emptyHeart;



    private void Start()
    {
        maxHearts = hearts.Length;
    }

    // Update is called once per frame
    void Update()
    {
        // healthForHearts shouldn't be greater than currentHearts,
        // so we enforce that here
        if (healthForHearts > maxHearts)
        {
            healthForHearts = maxHearts;
        }

        if (fullHeart != null && emptyHeart != null)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                //if (i < maxHearts)
                //{
                //    hearts[i].enabled = true;
                //}
                //else
                //{
                //    hearts[i].enabled = false;
                //}

                if (i < healthForHearts)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }
            }
        }
        else
        {
            // Debug.Log("Please set a sprite for fullHeart and/or emptyHeart in the inspector");
            return;
        }
    }
}
