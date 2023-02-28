using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCollectorScript : Interactable
{


    int CollectionAmount= 50;

    int CurrentWoodCoolDown = 0;
    int MaxWoodCoolDown = 500;

    int MAssigned = 3 ; // the ammount of minions assigned to the buiding 
    int MaxMAssigned = 5;

    int CurrentBuildingLevel=0;// building level 1

    

    // Update is called once per frame
    void Update()
    {
        if(CurrentWoodCoolDown >= MaxWoodCoolDown)
        {
            Init.Instance.resourceManager.AddWood((MAssigned/MaxMAssigned)*CollectionAmount);
            //resourceManager.AddWood(50);
            print("Wood Collector added "+ ((MAssigned / MaxMAssigned) * CollectionAmount) + " wood");
            CurrentWoodCoolDown = 0;
        }
        if(health<=0) 
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
        if(Init.Instance.resourceManager.GetWood()>= 50)
        {
            health = maxHealth;
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
