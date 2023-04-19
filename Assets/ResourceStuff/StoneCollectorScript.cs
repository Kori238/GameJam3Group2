using System.Collections.Generic;
using UnityEngine;

public class StoneCollectorScript : WoodCollectorScript
{
  
   

    



    public override void Start()
    {
        resourceType = "stone";
       

        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        if (CurrentWoodCoolDown >= MaxWoodCoolDown)
        {
            Init.Instance.resourceManager.AddStone(50);


            print("Stone Collector added 50 stone");
            CurrentWoodCoolDown = 0;
        }
        if (health <= 0)
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
        if (toOpen)
        {
            InstanceMenu = Instantiate(PopMenu);
            toOpen = false;
            InstanceMenu.GetComponent<pMenu>().SetParentStructure(gameObject);

        }
        else if (!toOpen)
        {
            Destroy(InstanceMenu);
            toOpen = true;
        }
    }

        public override string  getResourceType()
        {
            return "stone";
        }
    public override Structure GetLocalTree()
    {
        if (LocalTrees.Count > 0)
        {
            GameObject temp = LocalTrees[(int)Random.Range(0, LocalTrees.Count - 1)];
            return temp.GetComponent<StoneInteract>();
        }
        return null;
    }
}

