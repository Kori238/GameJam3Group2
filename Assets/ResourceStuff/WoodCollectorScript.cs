using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCollectorScript : Interactable
{


    int CollectionAmount= 50;

    int CurrentWoodCoolDown = 0;
    int MaxWoodCoolDown = 500;

    int currentHealth =50;
    int maxHealth = 100;

    int MAssigned = 3 ; // the ammount of minions assigned to the buiding 
    int MaxMAssigned = 5;

    int CurrentBuildingLevel=0;// building level 1

    

    // Update is called once per frame
    void Update()
    {
        if(CurrentWoodCoolDown >= MaxWoodCoolDown)
        {
            GridInit.Instance.resourceManager.AddWood((MAssigned/MaxMAssigned)*CollectionAmount);
            //resourceManager.AddWood(50);
            print("Wood Collector added "+ ((MAssigned / MaxMAssigned) * CollectionAmount) + " wood");
            CurrentWoodCoolDown = 0;
        }
        if(currentHealth<=0) 
        { 
        Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        CurrentWoodCoolDown +=  1;

    }
    public override void Interact()
    {
        if(GridInit.Instance.resourceManager.GetWood()>= 50)
        {
            currentHealth = maxHealth;
            Debug.Log("building repaired");
        }
        else
        {
            Debug.Log("failed to repair");
        }

    }
    private void upgrade()
    {
        switch (CurrentBuildingLevel)
        {
            case 0:// upgades to level 2
                {
                    MaxMAssigned = 10;
                    break;
                }
               
        }
    }
}
