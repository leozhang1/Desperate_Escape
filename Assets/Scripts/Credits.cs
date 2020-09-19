using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject credits, creditsTitle, creditsScroll;

    private GameObject[] creditsObjects;
    // Start is called before the first frame update
    void Start()
    {
        creditsObjects = GameObject.FindGameObjectsWithTag("Credits");
        StartCoroutine(CreditsStart(0.5f));
    }

    IEnumerator CreditsStart(float scrollSpeed)
    {
        yield return new WaitForSeconds(5f);
        for (float colorVal = 0f; colorVal <= 0.5f; colorVal += 0.001f)
        {
            credits.GetComponent<Image>().color = new Color(colorVal, colorVal, colorVal);
            creditsTitle.GetComponent<Image>().color = new Color(colorVal * 2, colorVal * 2, colorVal * 2);
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        while (creditsScroll.transform.childCount > 0)
        {
            creditsScroll.transform.localPosition = new Vector3(creditsScroll.transform.localPosition.x, creditsScroll.transform.localPosition.y + scrollSpeed, creditsScroll.transform.localPosition.z);
            foreach (GameObject obj in creditsObjects)
            {
                if (obj != null)
                {
                    float totalHeight = credits.GetComponent<RectTransform>().rect.height + obj.GetComponent<RectTransform>().rect.height;
                    for (int i = 0; i < obj.transform.childCount; i++)
                    {
                        GameObject child = obj.transform.GetChild(i).gameObject;
                        totalHeight += child.GetComponent<RectTransform>().rect.height - child.transform.localPosition.y;
                    }
                    if (obj.transform.position.y > totalHeight * credits.transform.lossyScale.y)
                        Destroy(obj);
                }
            }
            yield return null;
        }
        GameObject.Find("Game Manager").GetComponent<LevelManager>().RestartGame();
    }
}
