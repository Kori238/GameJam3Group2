using System.Collections.Generic;
using UnityEngine;

public class StoneCollectorScript : WoodCollectorScript
{

   


 



    public override void Start()
    {
        resourceType = "stone";
        UpgradeDescription = ("Upgrade to level 2 and increase production to " + (CollectionAmount + 25));
        woodUpgradeCost = 50;
        stoneUpgradeCost = 100;
        CollectionAmount = 25;

        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
      
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
    public override bool upgrade()
    {
        if (woodUpgradeCost <= Init.Instance.resourceManager.GetWood() && stoneUpgradeCost <= Init.Instance.resourceManager.GetStone())
        {
            Init.Instance.resourceManager.AddWood(-woodUpgradeCost);
            Init.Instance.resourceManager.AddStone(-stoneUpgradeCost);
            switch (CurrentBuildingLevel)
            {
                case 1: // upgades to level 2
                    {
                        CollectionAmount = 50;
                        CurrentBuildingLevel = 2;
                        UpgradeDescription = "Upgrade to level 3 and increase production to " + (CollectionAmount + 25);
                        woodUpgradeCost = 100;
                        stoneUpgradeCost = 200;
                        foreach (MinionScript minion in MinionList) { minion.setCollectionAmount(CollectionAmount); }

                        break;
                    }
                case 2: // upgades to level 3
                    {
                        CollectionAmount = 75;
                        CurrentBuildingLevel = 3;
                        UpgradeDescription = ("Upgrade to level 4 and increase production to " + (CollectionAmount + 25));
                        woodUpgradeCost = 200;
                        stoneUpgradeCost = 400;
                        foreach (MinionScript minion in MinionList) { minion.setCollectionAmount(CollectionAmount); }

                        break;
                    }
                case 3: // upgades to level 4
                    {
                        CollectionAmount = 100;
                        CurrentBuildingLevel = 4;
                        UpgradeDescription = "Max Level";
                        woodUpgradeCost = 0;
                        stoneUpgradeCost = 0;
                        foreach (MinionScript minion in MinionList) { minion.setCollectionAmount(CollectionAmount); }

                        break;
                    }
            }

            return true;
        }
        return false;
    }
}

