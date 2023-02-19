using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class S_Pl_Abilities : MonoBehaviour
{
    private bool flag = false;
    private GameObject enemy1;
    private int CameraZoomValue = 40;

    public GameObject MovementScript;

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
                CameraZoomValue = 40;
                flag= true;
                MovementScript.GetComponent<S_Pl_Movement>().Pl_Speed = 5;
            }
            else
            {
                CameraZoomValue = 18;
                flag = false;
                MovementScript.GetComponent<S_Pl_Movement>().Pl_Speed = 16;
            }
            Camera.main.orthographicSize = CameraZoomValue;
        }
        
       
    }

    
}
