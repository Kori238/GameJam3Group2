using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPassword : MonoBehaviour
{
    public GameObject DebugMode;
    public void checkString(string password)
    {
        if (password == "gamejam3")
        {
            DebugMode.SetActive(true);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(DebugMode);
    }

}
