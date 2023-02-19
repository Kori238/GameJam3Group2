using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCollectorScript : Interactable
{
    



    int CurrentWoodCoolDown = 0;
    int MaxWoodCoolDown = 500;

    int currentHealth =50;
    int maxHealth = 100;






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentWoodCoolDown >= MaxWoodCoolDown)
        {
            GridInit.Instance.resourceManager.AddWood(50);
            //resourceManager.AddWood(50);
            print("Wood Collector added 50 wood");
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
        currentHealth = 100;
        print(currentHealth);

    }
}
