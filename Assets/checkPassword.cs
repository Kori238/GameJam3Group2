using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPassword : MonoBehaviour
{
    public GameObject DebugMode;
    public checkPassword cPs;
    public bool adminMode = false;
    public GameObject debugMenu;
    public void checkString(string password)
    {
        if (password == "gamejam3")
        {
            DebugMode.SetActive(true);
            adminMode = true;
        }
        else
        {
            adminMode = false;
        }
    }


    public static checkPassword Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        
    }

}
