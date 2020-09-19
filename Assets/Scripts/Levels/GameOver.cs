using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOver, gameOverButtons, terroristsChasingText, descriptionText, quitPrompt;
    private LevelManager levelMgr;

    public void TriggerGameOver(int numTerroristsChasing)
    {
        StartCoroutine(GameObject.Find("Game Manager").GetComponent<GameOver>().GameOverCoroutine(numTerroristsChasing));
    }

    public IEnumerator GameOverCoroutine(int numTerroristsChasing)
    {
        yield return new WaitForSeconds(2f);
        levelMgr = GameObject.Find("Game Manager").GetComponent<LevelManager>();
        levelMgr.State = LevelManager.LevelState.Results;
        terroristsChasingText.GetComponent<Text>().text = "Number of terrorists chasing you:     " + numTerroristsChasing;

        //Set the Game Over description based on the amount of terrorists chasing the player.
        Text desc = descriptionText.GetComponent<Text>();
        switch (numTerroristsChasing)
        {
            case 0:
                desc.text = "You have committed suicide!\nBe careful where you place those grenades!";
                break;
            case 1:
                desc.text = "One terrorist. Not too bad.\nYou should be able to evade this terrorist easily.";
                break;
            case 2:
                desc.text = "Try to be careful around here.\nThese terrorists grant no mercy.";
                break;
            case 3:
                desc.text = "You're playing a dangerous game here!\nTry not to draw too much attention.";
                break;
            default:
                desc.text = "You drew too much attention!\nWe highly recommend not to use brute force!";
                break;
        }

        gameOver.SetActive(true);
        SetActiveView(gameOverButtons);
    }

    public void SetActiveView(GameObject obj)
    {
        gameOverButtons.SetActive(obj == gameOverButtons);
        quitPrompt.SetActive(obj == quitPrompt);
    }
}
