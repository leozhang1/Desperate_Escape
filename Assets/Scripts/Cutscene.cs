using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private GameObject skipCS;
    [SerializeField] private GameObject[] texts;
    [SerializeField] private bool isSkippable;
    [SerializeField] private float textDisplayTime, textDelayTime;

    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("Game Manager").GetComponent<LevelManager>();
        if (levelManager.State != LevelManager.LevelState.Cutscene)
            return;

        StartCoroutine(PlayCutscene());
        if (skipCS.activeSelf)
            StartCoroutine(TurnOffSkipCSPrompt());
    }

    public void SkipCutscene()
    {
        if (isSkippable)
        {
            if (skipCS.activeSelf)
            {
                levelManager.NextLevel();
            }
            else
            {
                skipCS.SetActive(true);
                StartCoroutine(TurnOffSkipCSPrompt());
            }
        }
    }

    private IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Textboxes to display in cutscene: " + texts.Length);
        foreach (GameObject textObj in texts)
        {
            Text text = textObj.GetComponent<Text>();
            yield return StartCoroutine(CoroutineUtils.FadeText(text, true));
            yield return new WaitForSeconds(textDisplayTime);
            yield return StartCoroutine(CoroutineUtils.FadeText(text, false));
            yield return new WaitForSeconds(textDelayTime);
        }
        levelManager.NextLevel();
    }

    private IEnumerator TurnOffSkipCSPrompt()
    {
        if (skipCS.activeSelf)
        {
            yield return new WaitForSeconds(5);
            skipCS.SetActive(false);
        }
    }
}
