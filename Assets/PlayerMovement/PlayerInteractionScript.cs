using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField] ResourceManager resourceManager;// referance to the resource manager in game scene
    Vector2 boxSize = new Vector2(0.1f, 0.1f); // size of raycast
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            CheckInteraction();
            print("wood = " + resourceManager.GetWood());

        }
    }





    private void CheckInteraction()
    {

        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 CurrentPlayerPos = transform.position;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(MouseWorldPos, boxSize, 0, Vector2.zero); // gets list of objects at mouse postition

        if (CheckRange(CurrentPlayerPos, MouseWorldPos) == true)
        {
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



    int range = 500;
    private bool CheckRange(Vector2 PlayerPos, Vector2 TargetPos) // checks the player is in range of the interactable object 
    {

        float xlength = (PlayerPos.x - TargetPos.x) * (PlayerPos.x - TargetPos.x);
        float ylength = (PlayerPos.y - TargetPos.y) * (PlayerPos.y - TargetPos.y);


        if (xlength <= range)
        {
            if (ylength <= range)
            {
                print("in range");
                return true;

            }
        }
        print("not in range");
        return false;
    }

}
