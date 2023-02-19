using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class S_Pl_Abilities : MonoBehaviour
{
    private bool flag = false;
    private GameObject enemy1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Ability1"))
        {
            Debug.Log("MainAttack");
            
        }
        else if (Input.GetButtonDown("Ability2"))
        {
            Debug.Log("SideAttack");
        }
        else if (Input.GetButtonDown("Camera"))
        {
            if (!flag)
            {
                Camera.main.orthographicSize = 40;
                flag= true;
            }
            else
            {
                Camera.main.orthographicSize = 18;
                flag= false;
            }
            
        }

       
    }

    
}
