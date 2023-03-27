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

    private int MAssigned; // the ammount of minions assigned to the buiding 
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

    public void SetMinionAssigned(int newMinionAssigned)
    {
        if (MAssigned < MaxMAssigned) 
        {

            MAssigned = newMinionAssigned + MAssigned;
            if(MAssigned< MaxMAssigned) { MAssigned=MaxMAssigned; }
        }
        SetMinion();
    }
    public int GetMinionAssigned() { return MAssigned; }

    public void SetMinion()
    {
        print("Setting minion");
        Transform newMinion = Init.Instance.resourceManager.GetMinionList();
        Debug.Log(newMinion.ToString());
        MinionList.Add(newMinion);
        newMinion.GetComponent<MinionScript>().setJobLocation(this);
    }


   
}
