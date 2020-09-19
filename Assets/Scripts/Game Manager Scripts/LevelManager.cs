using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelState state;

    public static int PlayerHealth { get; private set; }

    public static Dictionary<Weapon.Type, int> PlayerWeapons { get; private set; }

    public LevelState State
    {
        get => state;
        set { state = value; }
    }

    public void NewGame()
    {
        PlayerHealth = DataDriven.PlayerInitHealth;
        PlayerWeapons = null;
        NextLevel();
    }

    public void NextLevel()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            PlayerHealth = player.GetComponent<PlayerStats>().Health;
            PlayerWeapons = player.GetComponent<PlayerStats>().GetAmmoCollection();
        }
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(int level)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(level);
    }

    public void RestartGame()
    {
        LoadLevel(0);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public enum LevelState
    {
        Menu,
        Gameplay,
        Pause,
        Cutscene,
        Results,
        Credits
    }
}
