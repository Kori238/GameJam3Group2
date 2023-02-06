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
    
    [SerializeField] ResourceManager resourceManager;// referance to the resource manager in game scene
    Vector2 boxSize = new Vector2(0.1f, 0.1f); // size of raycast

    void Update() // Update is called once per frame
    {
        //Input:

        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");

        //if (Input.GetButtonDown("Fire1"))
        //{
          //  Attack();
        //}

        if (Input.GetMouseButtonDown(0))
        {
            
            CheckInteraction();
            print("wood = " + resourceManager.GetWood());

        }
    }

    private void FixedUpdate() //Executed on a fixed timer (Not on framerate)
    {
        //Movement:

        RigidBody.MovePosition(RigidBody.position + Movement * Pl_Speed * Time.fixedDeltaTime);
    }

    void Attack()
    {
        hitSound = GetComponent<AudioSource>();
        hitSound.Play();
    }
    private void CheckInteraction()
    {
        
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(MouseWorldPos, boxSize, 0, Vector2.zero); // gets list of objects at mouse postition

        
        if (hits.Length > 0) // checks if object is present
        {

            foreach (RaycastHit2D i in hits)
            {
                if (i.transform.GetComponent<Interactable>()) // checks object is interactable
                {
                    i.transform.GetComponent<Interactable>().Interact(); // calls interaction script
                    
                    return; // stops multiple objects from being interacted with
                    
                }
            }
        }
    }
}
