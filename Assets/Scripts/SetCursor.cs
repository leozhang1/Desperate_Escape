using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursor : MonoBehaviour
{
    [SerializeField] public float xspot = 10f;

    [SerializeField] public float yspot = 10f;

    // Start is called before the first frame update
    void Start()
    {
        CursorSet(GetComponent<SpriteRenderer>().sprite.texture);
    }

    void CursorSet(Texture2D tex)
    {
        CursorMode mode = CursorMode.Auto;
        xspot = tex.width / 2;
        yspot = tex.height / 2;
        Vector2 hotSpot = new Vector2(xspot, yspot);
        Cursor.SetCursor(tex, hotSpot, mode);
    }
}
