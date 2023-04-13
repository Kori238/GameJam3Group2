using System.Collections.Generic;
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


 

    // Update is called once per frame
    private void Update()
    {
        if (CurrentWoodCoolDown >= MaxWoodCoolDown)
        {
            Init.Instance.resourceManager.AddWood(MAssigned / MaxMAssigned * CollectionAmount);
            //resourceManager.AddWood(50);
            print("Wood Collector added " + MAssigned / MaxMAssigned * CollectionAmount + " wood");
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

    public void repair()
    {
        print("repairing");
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

    public bool SetMinionAssigned(int newMinionAssigned)// checks if minion can be assinged
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
    public int GetMinionAssigned() { return MAssigned; }

    public void SetMinion()// assigns a minion from available minion list
    {
        print("Setting minion");
        Transform newMinion = Init.Instance.resourceManager.GetMinionList();
        Debug.Log(newMinion.ToString());
        MinionList.Add(newMinion);
        newMinion.GetComponent<MinionScript>().setJobLocation(this);
        MAssigned= MAssigned +1;
    }

    public void unassignMinion()// unassings minions and adds them to available minion list
    {
        Transform temp = MinionList[MinionList.Count - 1];
        MinionList.RemoveAt(MinionList.Count - 1);
        temp.GetComponent<MinionScript>().setJobLocation(null);
        Init.Instance.resourceManager.addSingleMinionToList(temp);
        MAssigned = MAssigned - 1;
    }
    private void OnDestroy()// temp may change how damgaged building works 
    {
        Destroy(InstanceMenu);
        for(int i = 0;i< MinionList.Count;i++) { MinionList[i].GetComponent<MinionScript>().setJobLocation(null); }
        MinionList.Clear();
    }
}
