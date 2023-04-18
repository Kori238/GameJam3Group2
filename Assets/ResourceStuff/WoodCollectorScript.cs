using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WoodCollectorScript : Interactable
{
    [SerializeField] private GameObject PopMenu;



    private readonly int CollectionAmount = 50;


    private readonly int CurrentBuildingLevel = 0; // building level 1
    private readonly int MaxWoodCoolDown = 500;

    private int CurrentWoodCoolDown;

    private GameObject InstanceMenu;

    [SerializeField] private int MAssigned; // the ammount of minions assigned to the buiding 
    private int MaxMAssigned = 5;
    private pMenu pmenu;
    private bool toOpen = true;
    [SerializeField] private List<Transform> MinionList;
    [SerializeField] private List<GameObject> LocalTrees;
    [SerializeField] private Collider2D collectZone;
    [SerializeField] private List<Collider2D> overlapingObjects;
    [SerializeField] ContactFilter2D treeFilter;


    private int repairCost=25;

    private string resourceType = "wood";
    
    public override void Start()
    {

       collectZone.GetComponent<Collider2D>().OverlapCollider(treeFilter,overlapingObjects);
        if (overlapingObjects.Equals(null)) { }
        else
        {
            for (int i = 0; i < overlapingObjects.Count; i++)
            {
                if (overlapingObjects[i].GetComponent<TreeInteract>() is TreeInteract)
                {
                    LocalTrees.Add(overlapingObjects[i].gameObject);
                }

            }
        }
        overlapingObjects.Clear();
      
        base.Start();
    }





    // Update is called once per frame
    private void Update()
    {
       // if (CurrentWoodCoolDown >= MaxWoodCoolDown)
       // {
         //   Init.Instance.resourceManager.AddWood(MAssigned / MaxMAssigned * CollectionAmount);
            //resourceManager.AddWood(50);
          //  print("Wood Collector added " + MAssigned / MaxMAssigned * CollectionAmount + " wood");
          //  CurrentWoodCoolDown = 0;
       // }
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

    public void repair()
    {
        if(Init.Instance.resourceManager.GetWood()== repairCost)
        {
            Debug.Log("repairing");
            SetHealth(0f, true);
        }
        else { Debug.Log("Not enough resources to repair"); }
       
    }
    public bool upgrade()
    {
        switch (CurrentBuildingLevel)
        {
            case 0: // upgades to level 2
            {
                MaxMAssigned = 10;
                break;
            }
        }

        print("succces");
        return true;
    }

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
        Transform newMinion = Init.Instance.resourceManager.GetMinionList();
        Debug.Log(newMinion.ToString());
        MinionList.Add(newMinion);
        newMinion.GetComponent<MinionScript>().setJobLocation(this);
        MAssigned= MinionList.Count;
    }

    public void unassignMinion()// unassings minions and adds them to available minion list
    {
       
            Transform temp = MinionList[MinionList.Count - 1];
            MinionList.RemoveAt(MinionList.Count - 1);
            temp.GetComponent<MinionScript>().setJobLocation(null);
            Init.Instance.resourceManager.addSingleMinionToList(temp);
            MAssigned = MinionList.Count;
       
       
    }
   
    
    private void OnDestroy()// temp may change how damgaged building works 
    {
        Destroy(InstanceMenu);
        //for(int i = 0;i< MinionList.Count;i++) { MinionList[i].GetComponent<MinionScript>().setJobLocation(null); }
        //MinionList.Clear();
    }
   public Structure GetLocalTree()
    {
        if (LocalTrees.Count > 0) {
            GameObject temp = LocalTrees[(int)Random.Range(0, LocalTrees.Count - 1)];
            return temp.GetComponent<TreeInteract>();
        }
        return null;
    }

    public string getResourceType()
    {
        return "wood";
    }


    public override void CreateAttackPoints()
    {
        return;
    }
    public int getRepairCost()
    {
        return repairCost;
    }
}
