using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoroutineUtils : MonoBehaviour
{
    private const float fadeTime = 0.02f;

    public static IEnumerator FadeText(Text text, bool fadeIn)
    {
        if (fadeIn)
        {
            // Debug.Log("Fading in: " + text.gameObject.name);
            while (text.color.a < 1)
            {
                text.color = new Color(1f, 1f, 1f, text.color.a + fadeTime);
                yield return new WaitForSeconds(fadeTime);
            }
        }
        else
        {
            // Debug.Log("Fading out: " + text.gameObject.name);
            while (text.color.a > 0)
            {
                text.color = new Color(1f, 1f, 1f, text.color.a - fadeTime);
                yield return new WaitForSeconds(fadeTime);
            }
        }
    }
}
