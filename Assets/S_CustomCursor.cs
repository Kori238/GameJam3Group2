using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CustomCursor : MonoBehaviour
{
    public Vector3 position;
    public Transform Cursor;
    
    void Start()
    {
        UnityEngine.Cursor.visible = false;
    }

    void Update()
    {
        Cursor.position = Input.mousePosition - position;

    }
}
