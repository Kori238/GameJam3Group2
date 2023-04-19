using System.Collections.Generic;
using UnityEngine;

public class StoneCollectorScript : WoodCollectorScript
{
    private readonly int MaxWoodCoolDown = 500;
    private int CurrentWoodCoolDown;


    [SerializeField] private GameObject PopMenu;



    private readonly int CollectionAmount = 50;


    private readonly int CurrentBuildingLevel = 0; // building level 1
  

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




    public override void Start()
    {

        collectZone.GetComponent<Collider2D>().OverlapCollider(treeFilter, overlapingObjects);
        if (overlapingObjects.Equals(null)) { }
        else
        {
            for (int i = 0; i < overlapingObjects.Count; i++)
            {
                if (overlapingObjects[i].GetComponent<StoneInteract>() is StoneInteract)
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

