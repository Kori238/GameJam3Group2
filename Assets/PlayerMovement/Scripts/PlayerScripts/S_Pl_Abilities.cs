using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI;

public class S_Pl_Abilities : MonoBehaviour
{
    private bool flag = false;
    private GameObject enemy1;
    private int CameraZoomValue = 40;
    Animator animator;
    public GameObject MovementScript;
    public GameObject SoundControllerScript;
    public Collider2D AttackCollider;
    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
        
    }

    

    void Ability1()
    {
        if (canAttack == true)
        {
            Debug.Log("MainAttack");
            SoundControllerScript.GetComponent<S_SoundController>().AttackSound();
            StartCoroutine(AttackDelay());
        }
    }

    private IEnumerator AttackDelay()
    {
        canAttack = false;
        AttackCollider.enabled = true;
        animator.Play("Attack");
        yield return new WaitForSeconds(1f);
        animator.Play("Idle");
        AttackCollider.enabled = false;
        canAttack = true;
    }

        // Update is called once per frame
        void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ability1();
           
        }
        else if (Input.GetButtonDown("Ability2"))
        {
            animator.Play("Idle");
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

    public void DashZoom()
    {
        CameraZoomValue = 16;
        Camera.main.orthographicSize = CameraZoomValue;
    }
    public void DashunZoom()
    {
        CameraZoomValue = 18;
        Camera.main.orthographicSize = CameraZoomValue;
    }

}
