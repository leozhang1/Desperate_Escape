using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public const string upText = "Up";
    public const string downText = "Down";
    public const string leftText = "Left";
    public const string rightText = "Right";

    public const string sneakText = "Sneak";

    public const string actionText = "Fire Weapon/Confirm";
    public const string cancelText = "Cancel";
    public const string reloadText = "Reload";
    public const string pauseText = "Pause";

    public const string prevWeaponText = "Previous Weapon";
    public const string nextWeaponText = "Next Weapon";

    private GameObject player;
    private PlayerStats pStats;

    private LevelManager levelMgr;
    private PauseManager pauseMgr;
    private UIManager uiMgr;

    // Start is called before the first frame update
    void Start()
    {
        levelMgr = GameObject.Find("Game Manager").GetComponent<LevelManager>();
        pauseMgr = GameObject.Find("Game Manager").GetComponent<PauseManager>();
        if (levelMgr.State == LevelManager.LevelState.Gameplay)
        {
            player = GameObject.Find("Player");
            pStats = player.GetComponent<PlayerStats>();
            uiMgr = GameObject.Find("UI").GetComponent<UIManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (levelMgr.State)
        {
            // Game is paused. Gameplay is frozen, and player selects buttons from the pause menu.
            case LevelManager.LevelState.Pause:

                if (Input.GetButtonDown("Cancel"))
                {
                    if (pauseMgr.IsActiveView(pauseMgr.GetView(PauseManager.View.Main)))
                        pauseMgr.SetPaused(false);
                    else
                        pauseMgr.SetActiveView(pauseMgr.GetView(PauseManager.View.Main));
                }

                if (Input.GetButtonDown("Pause"))
                {
                    pauseMgr.SetPaused(false);
                }
                break;

            case LevelManager.LevelState.Gameplay:
                if (player != null)
                {
                    //Player movement call that utilizes Input.GetAxis("Horizontal") and Input.GetAxis("Vertical")
                    player.GetComponent<MovementForShooting>().Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), DataDriven.PlayerSpeed / (Input.GetButton("Sneak") ? 2 : 1), Input.mousePosition);

                    if (Input.GetButtonDown("Fire"))
                    {
                        pStats.Fire();
                    }

                    if (Input.GetButtonDown("Melee"))
                    {
                        if (!pStats.IsAnimating)
                            StartCoroutine(pStats.Melee());
                    }

                    if (Input.GetButtonDown("Reload"))
                    {
                        //Call function to reload weapon.
                        if (pStats.CanReload())
                            StartCoroutine(pStats.Reload());
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        if (!pStats.IsAnimating)
                            pStats.CycleWeapon(false);
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        if (!pStats.IsAnimating)
                            pStats.CycleWeapon(true);
                    }

                    if (Input.GetButtonDown("Pause"))
                    {
                        pauseMgr.SetPaused(true);
                    }
                }
                break;
            case LevelManager.LevelState.Cutscene:
                if (Input.GetButtonDown("Pause"))
                {
                    GameObject.Find("Cutscene").GetComponent<Cutscene>().SkipCutscene();
                }
                break;
        }

    }
}
