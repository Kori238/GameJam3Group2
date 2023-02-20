using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCollectorScript : Interactable
{

    int CurrentWoodCoolDown = 0;
    int MaxWoodCoolDown = 500;

    int currentHealth = 50;
    int maxHealth = 100;








    // Update is called once per frame
    void Update()
    {
        if (CurrentWoodCoolDown >= MaxWoodCoolDown)
        {
            GridInit.Instance.resourceManager.AddStone(50);
            
            print("Stone Collector added 50 stone");
            CurrentWoodCoolDown = 0;
        }
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        CurrentWoodCoolDown += 1;

    }
    public override void Interact()
    {
        if (GridInit.Instance.resourceManager.GetStone() >= 50)
        {
            currentHealth = maxHealth;
            Debug.Log("building repaired");
        }
        else
        {
            Debug.Log("failed to repair");
        }

    }
}
