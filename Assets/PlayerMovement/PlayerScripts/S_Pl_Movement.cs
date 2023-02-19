using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class S_Pl_Movement : MonoBehaviour
{
    public float Pl_Speed = 5f; //Player Speed
    public Rigidbody2D RigidBody; //Reference to RigidBody2D
    Vector2 Movement;
    public int Health = 100;
    public AudioSource hitSound;

    void Update() // Update is called once per frame
    {
        //Input:

        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");
      
    }

    private void FixedUpdate() //Executed on a fixed timer (Not on framerate)
    {
        //Movement:

        RigidBody.MovePosition(RigidBody.position + Movement * Pl_Speed * Time.fixedDeltaTime);

        if (Movement.x > 0)
        {
            gameObject.transform.localScale = new Vector3(0.3f, 0.3922362f, 0.02614908f);
        }else if (Movement.x < 0)
        {
           gameObject.transform.localScale = new Vector3(-0.3f, 0.3922362f, 0.02614908f);
        }


    }
    
    
}
