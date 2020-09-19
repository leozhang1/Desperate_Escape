using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDisable : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        //Disable exit button when game is running on a web browser or the Unity Editor.
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            gameObject.SetActive(false);

            //Shift the other two buttons down as a result of the above button being disabled.
            Vector3 shift = new Vector3(0, -40, 0);
            foreach (GameObject button in buttons)
                button.transform.localPosition += shift;
        }

        
    }
}
