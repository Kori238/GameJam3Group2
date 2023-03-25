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


        //if(Init.Instance.resourceManager.GetWood()>= 50)
        //{
        // currentHealth = maxHealth;
        //   Debug.Log("building repaired");
        // }
        // else
        // {
        //    Debug.Log("failed to repair");
        //  }
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

    public void SetMinionAssigned(int newminion)
    {
        MAssigned = newminion;
    }
    public int GetMinionAssigned() { return MAssigned; }
}
