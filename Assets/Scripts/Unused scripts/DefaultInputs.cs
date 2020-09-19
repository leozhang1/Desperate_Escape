using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultInputs : MonoBehaviour
{
    /*
     * Class that contains default inputs for the keyboard/moouse and controller.
     * This file should only contain constant or static readonly variables.
     */

    public static readonly Dictionary<string, string> keyboard = InitKeyDefault(InputType.Keyboard);
    public static readonly Dictionary<string, string> keyboardAlt = InitKeyDefault(InputType.KeyboardAlt);
    public static readonly Dictionary<string, string> joystick = InitKeyDefault(InputType.Joystick);

    private static Dictionary<string, string> InitKeyDefault(InputType type)
    {
        Dictionary<string, string> keys = new Dictionary<string, string>();

        switch (type)
        {
            case InputType.Keyboard:
                //Movement
                keys.Add(InputManager.upText, "w");
                keys.Add(InputManager.downText, "s");
                keys.Add(InputManager.leftText, "a");
                keys.Add(InputManager.rightText, "d");

                //Sneaking
                keys.Add(InputManager.sneakText, "left shift");

                //Shooting Mechanics and Menu Controls
                keys.Add(InputManager.actionText, "mouse 0");
                keys.Add(InputManager.cancelText, "mouse 1");
                keys.Add(InputManager.reloadText, "r");

                //Pause Menu
                keys.Add(InputManager.pauseText, "escape");

                break;
            case InputType.KeyboardAlt:
                //Movement
                keys.Add(InputManager.upText, "up");
                keys.Add(InputManager.downText, "down");
                keys.Add(InputManager.leftText, "left");
                keys.Add(InputManager.rightText, "right");

                //Sneaking
                keys.Add(InputManager.sneakText, "right shift");

                //Shooting Mechanics and Menu Controls
                keys.Add(InputManager.actionText, null);
                keys.Add(InputManager.cancelText, null);
                keys.Add(InputManager.reloadText, null);

                //Pause Menu
                keys.Add(InputManager.pauseText, null);

                break;
        }

        return keys;
    }

    private enum InputType
    {
        Keyboard,
        KeyboardAlt,
        Joystick
    }
}
