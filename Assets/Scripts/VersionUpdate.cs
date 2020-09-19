using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionUpdate : MonoBehaviour
{
    [SerializeField] private GameObject versionText;

    // Start is called before the first frame update
    void Start()
    {
        versionText.GetComponent<Text>().text = "Version " + Application.version;
    }
}
