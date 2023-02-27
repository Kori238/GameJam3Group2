using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor;
using UnityEngine;

public class S_Pl_Movement : MonoBehaviour
{
    public float Pl_Speed = 16f; //Player Speed
    public Rigidbody2D RigidBody; //Reference to RigidBody2D
    Vector2 Movement;
    public int Health = 100;
    public AudioSource hitSound;
    private bool canDash = true;
    public GameObject SoundControllerScript;
    public Transform PlayerTransform;

    void Update() // Update is called once per frame
    {
        //Input:

        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");
      

        if (Input.GetButtonDown("LeftShift"))
        {
            if (canDash)
            {
                SoundControllerScript.GetComponent<S_SoundController>().Dash();
                StartCoroutine(DashDelay());
            }
        }

        if (Health <= 0)
        {
            PlayerDeath();
        }
    }

    void PlayerDeath()
    {
        PlayerTransform.position = new Vector3(2.154672f, 13.90528f, -0.4391842f);
        Health = 100;
        Debug.Log("YOU DIED"); //REPLACE HERE FOR PLAYER DEATH SCREEN, REMOVE ALL RESOURCES
    }

    private IEnumerator DashDelay()
    {
        S_Pl_Abilities abilities = GetComponent<S_Pl_Abilities>();

        canDash = false;
        Pl_Speed = 45f;
        abilities.DashZoom();
        yield return new WaitForSeconds(0.3f);
        Pl_Speed = 16f;
        abilities.DashunZoom();
        yield return new WaitForSeconds(3f);
        canDash = true;
    }

        private void FixedUpdate() //Executed on a fixed timer (Not on framerate)
    {
        //Movement:

        RigidBody.MovePosition(RigidBody.position + Movement * Pl_Speed * Time.fixedDeltaTime);

        if (Movement.x > 0)
        {
            gameObject.transform.localScale = new Vector3(6.401755f, 7.14f, 0.62f);
        }else if (Movement.x < 0)
        {
           gameObject.transform.localScale = new Vector3(-6.401755f, 7.14f, 0.62f);
        }


    }


    
    
}
