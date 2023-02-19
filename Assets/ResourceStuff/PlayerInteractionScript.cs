using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.Mathematics;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField] ResourceManager resourceManager;// referance to the resource manager in game scene
    [SerializeField] Transform WoodCollector;
    Vector2 boxSize = new Vector2(0.1f, 0.1f); // size of raycast

    string currentTool = "Interact";
    //options:
    //Interact
    //WoodCollector
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            PlayerInterct();
            print("wood = " + GridInit.Instance.resourceManager.GetWood());

        }
        if (Input.GetKeyDown("1")){
            currentTool = "Interact";
            print("interact Tool Equiped");
        }
        if (Input.GetKeyDown("2"))
        {
            currentTool = "WoodCollector";
            print("Build Wood Collector equiped");
        }
    }
    private void PlayerInterct() //allows mutilple function to be called from mouse button 1
    {
        switch (currentTool)
        {
            case "Interact":
                {
                    CheckInteraction();
                    break;
                }
            case "WoodCollector":
                {
                    PlaceWoodCollector();
                    break;
                }
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
    private void PlaceWoodCollector()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        if(GridInit.Instance.resourceManager.GetWood()>= 50)
        {
            gridPos = GridInit.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            bool valid = GridInit.Instance.grid.BuildAtCell((int)gridPos.x + 1, (int)gridPos.y + 1, WoodCollector);

            if (valid)
            {
                GridInit.Instance.resourceManager.AddWood(-50);
            }
        }
       

    }
}