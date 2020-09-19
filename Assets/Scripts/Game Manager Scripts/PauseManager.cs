using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen, pauseButtons, restartPrompt, options, quitPrompt;
    private LevelManager levelMgr;

    void Start()
    {
        levelMgr = GameObject.Find("Game Manager").GetComponent<LevelManager>();
    }

    void Update()
    {
        pauseScreen.SetActive(levelMgr.State == LevelManager.LevelState.Pause);
    }

    public void SetPaused(bool isPaused)
    {
        if (isPaused)
        {
            levelMgr.State = LevelManager.LevelState.Pause;
            Time.timeScale = 0f;
            SetActiveView(pauseButtons);
        }
        else
        {
            levelMgr.State = LevelManager.LevelState.Gameplay;
            Time.timeScale = 1f;
        }
    }

    public GameObject GetView(View view)
    {
        switch (view)
        {
            case View.Main:
                return pauseButtons;
            case View.Restart:
                return restartPrompt;
            case View.Options:
                return options;
            case View.Quit:
                return quitPrompt;
            default:
                return null;
        }
    }

    public void SetActiveView(GameObject view)
    {
        pauseButtons.SetActive(pauseButtons == view);
        restartPrompt.SetActive(restartPrompt == view);
        options.SetActive(options == view);
        quitPrompt.SetActive(quitPrompt == view);
    }

    public bool IsActiveView(GameObject view)
    {
        return view.activeSelf;
    }

    public enum View
    {
        Main,
        Restart,
        Options,
        Quit
    }
}
