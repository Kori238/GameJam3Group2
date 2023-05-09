using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Tilemaps;
using UnityEngine;

public class WoodCollectorScript : Interactable
{
    public GameObject PopMenu;

    public GameObject BuildingNotificationPrefab;

    public  int CollectionAmount = 25;


    public int CurrentBuildingLevel = 1;
   

    public int CurrentWoodCoolDown;

    public GameObject InstanceMenu;

    public int MAssigned; // the ammount of minions assigned to the buiding 
    public int MaxMAssigned = 5;
    public pMenu pmenu;
    public bool toOpen = true;
    public List<MinionScript> MinionList;
    public List<GameObject> LocalTrees;
    public Collider2D collectZone;
     public List<Collider2D> overlapingObjects;
     public ContactFilter2D treeFilter;

    public int woodUpgradeCost =100;
    public int stoneUpgradeCost =20;
    public string UpgradeDescription;

    


    public string resourceType = "wood";
    public int woodB = 100;
    public int stoneB = 0;

    public override void Start()
    {
       
        UpgradeDescription = ("Upgrade to level 2 and increase production to "+ (CollectionAmount+25));

        collectZone.GetComponent<Collider2D>().OverlapCollider(treeFilter,overlapingObjects);
        if (overlapingObjects.Equals(null)) { }
        else
        {
            for (int i = 0; i < overlapingObjects.Count; i++)
            {
               if(resourceType == "wood") {
                    if (overlapingObjects[i].GetComponent<TreeInteract>() is TreeInteract)
                    {
                        LocalTrees.Add(overlapingObjects[i].gameObject);
                        overlapingObjects[i].gameObject.GetComponent<TreeInteract>().addCollectors(this);
                    }
               }
               else
               {
                   if (overlapingObjects[i].GetComponent<StoneInteract>() is StoneInteract)
                   {
                       LocalTrees.Add(overlapingObjects[i].gameObject);
                       overlapingObjects[i].gameObject.GetComponent<StoneInteract>().addCollectors(this);
                   }
               }
               

            }
        }

        overlapingObjects.Clear();
        if(LocalTrees.Count == 0) 
        { 
            GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity);
            temp.GetComponent<resourcePopUp>().setText("INVALID LOCATION, NO RESOURCES NEAR BY");

            Vector2 pos=transform.position;
           Vector2 cellPos =Init.Instance.grid.GetWorldCellPosition(pos.x,pos.y);
            Init.Instance.grid.DemolishAtCell((int)cellPos.x, (int)cellPos.y);


            
            Init.Instance.resourceManager.AddWood(woodB); 
            Init.Instance.resourceManager.AddStone(stoneB);


        
        }
        Debug.Log(CollectionAmount.ToString());
        base.Start();
    }

    public override void FindOccupiedSpace()
    {
        var nodePos = gridPos * 3;
        var nodeGrid = Init.Instance.pathfinding.GetGrid();
        occupiedSpace.Add(nodeGrid.gridArray[(int)nodePos.x + 1, (int)nodePos.y + 1]);
        }


    public override void FindCollectionPoints()
    {
        if (destroyed) return;
        var nodePos = gridPos * 3;
        var nodeGrid = Init.Instance.pathfinding.GetGrid();
        var collectionPositions = new List<Vector2>
        {
            new Vector2(nodePos.x + 1, nodePos.y),
            new Vector2(nodePos.x, nodePos.y + 1),
            new Vector2(nodePos.x + 1, nodePos.y + 2),
            new Vector2(nodePos.x + 2, nodePos.y + 1),
        };

        foreach (Vector2 collectionPosition in collectionPositions)
        {
            if (collectionPosition.x >= 149 || collectionPosition.x < 0 || collectionPosition.y >= 149 || collectionPosition.y < 0) continue;
            var node = nodeGrid.gridArray[(int)collectionPosition.x, (int)collectionPosition.y];
            collectionPoints.Add(node);
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
       


     
    }


    public virtual  bool upgrade()
    {
        if(woodUpgradeCost<=Init.Instance.resourceManager.GetWood() && stoneUpgradeCost <= Init.Instance.resourceManager.GetStone()) 
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
                        woodUpgradeCost = 200;
                        stoneUpgradeCost = 50;
                        foreach (MinionScript minion in MinionList) { minion.setCollectionAmount(CollectionAmount); }

                        break;
                    }
                case 2: // upgades to level 3
                    {
                        CollectionAmount = 75;
                        CurrentBuildingLevel = 3;
                        UpgradeDescription = ("Upgrade to level 4 and increase production to " + (CollectionAmount + 25));
                        woodUpgradeCost = 400;
                        stoneUpgradeCost = 100;
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
    public int getWoodUpgradeCost() { return woodUpgradeCost; }
    public int getStoneUpgradeCost() { return stoneUpgradeCost; }
    public string getUpgradeDescription() { return UpgradeDescription; }
    public int getWoodRepairCost() { return woodRepairCost; }
    public int getStoneRepairCost() { return stoneRepairCost; }
   public int getBuilidingLevel() { return CurrentBuildingLevel; }   
    

    public bool SetMinionAssigned()// checks if minion can be assinged
    {
        
        if(Init.Instance.resourceManager.getAvailableMinionLength() > 0) 
        { 
            SetMinion();
           
            return true;
        }
        else { Debug.Log("no minions available");
            return false;
        }
    }
    public bool SetMinionUnAssigned()// checks if minion can be unassinged
    {

        if (MinionList.Count != 0)
        {
            unassignMinion();

            return true;
        }
        else
        {
            Debug.Log("no minions to unassign");
            return false;
        }
    }
    public int GetMinionAssigned() { return MAssigned; }

    public void SetMinion()// assigns a minion from available minion list
    {
        print("Setting minion");
        MinionScript newMinion = Init.Instance.resourceManager.GetMinionList().GetComponent<MinionScript>();
       
        MinionList.Add(newMinion);
        newMinion.setJobLocation(this);
        MAssigned= MinionList.Count;
        newMinion.setCollectionAmount(CollectionAmount);
    }

    public void unassignMinion()// unassings minions and adds them to available minion list
    {
       
            MinionScript temp = MinionList[MinionList.Count - 1];
            MinionList.RemoveAt(MinionList.Count - 1);
            temp.setJobLocation(null);
            Init.Instance.resourceManager.addSingleMinionToList(temp);
            MAssigned = MinionList.Count;
       
       
    }
    public void unassignDeadMinion(MinionScript newMinion)
    {

        MinionList.Remove(newMinion);
        MAssigned= MinionList.Count;
      

        


    }
    public void unssignTree(GameObject newTree) 
    { 
        LocalTrees.Remove(newTree);

    }


    private void OnDestroy()// temp may change how damgaged building works 
    {
        Destroy(InstanceMenu);
       
    }
   public virtual Structure GetLocalTree()
    {
        if (LocalTrees.Count > 0) {
            GameObject temp = LocalTrees[(int)Random.Range(0, LocalTrees.Count - 1)];
            return temp.GetComponent<TreeInteract>();
        }
        return null;
    }
    public int getLocalTreeLength() { return LocalTrees.Count; }

    public virtual string getResourceType()
    {
        return "wood";
    }


    public override void CreateAttackPoints()
    {
        return;
    }
    public override void Demolished()
    {
        foreach(var Minion in MinionList)
        {
            Minion.setJobLocation(null);
        }




        base.Demolished();
    }
}
