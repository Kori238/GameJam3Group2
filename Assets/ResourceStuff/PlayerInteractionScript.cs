using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField] private Transform WoodCollector;
    [SerializeField] private Transform StoneCollector;
    [SerializeField] private Transform Wall;
    [SerializeField] private Transform minionHouse;
    [SerializeField] private GameObject resourcePU;
    [SerializeField] private TMP_Text WhatsEquiped;

    [SerializeField] private GameObject BuildingNotificationPrefab;

    public TMP_Text WoodUI;
    [SerializeField] private Vector2[] pathfindingTestNodes = new Vector2[2];
    private readonly Vector2 boxSize = new Vector2(0.1f, 0.1f); // size of raycast

    public Transform select;
    public Transform H1; public Transform H2; public Transform H3; public Transform H4; public Transform H5; public Transform H6; public Transform H7; public Transform H8; public Transform H9;
    public Slider AxeSlider;
    public GameObject AxeSliderObject;

    private readonly int range = 1000;
    private string currentTool = "Sword";
    [SerializeField] private S_Pl_Abilities abilitiesScript;

    [SerializeField] private ResourceManager resourceManager; // referance to the resource manager in game scene
    //options:
    //Interact
    //WoodCollector
    //StoneCollector

    public Animator animator;
    private float currentCollectionTime = 0f;
    private float collectTime = 2f;
    private bool collecting = false;


    private void Start()
    {
        pathfindingTestNodes[0] = -Vector2.one;
        pathfindingTestNodes[1] = -Vector2.one;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            PlayerInterct();
            currentCollectionTime = 0f;

        }
        if (Input.GetMouseButton(0) && currentTool == "Interact") 
        {
            AxeSliderObject.SetActive(true);
            resourceCollectionTime();
        }
        if (Input.GetMouseButtonUp(0))
        {
            AxeSliderObject.SetActive(false);
        }

        if (Input.GetKeyDown("1"))
        {
            currentTool = "Sword";
            WhatsEquiped.SetText("Sword Equipped");
            select.transform.position = H1.transform.position;
        }
        if (Input.GetKeyDown("2"))
        {
            currentTool = "Interact";
            WhatsEquiped.SetText("Interact/Axe Tool Equipped");
            select.transform.position = H2.transform.position;
        }
        if (Input.GetKeyDown("3"))
        {
            currentTool = "WoodCollector";
            WhatsEquiped.SetText("Wood Collector Equipped");
            select.transform.position = H3.transform.position;
        }
        if (Input.GetKeyDown("4"))
        {
            currentTool = "StoneCollector";
            WhatsEquiped.SetText("Stone Collector Equipped");
            select.transform.position = H4.transform.position;
        }
        if (Init.Instance.testPathfinding && Input.GetKeyDown(KeyCode.T))
        {
            currentTool = "PathfindingTester";
            Debug.Log("Pathfinding Tester equipped");
        }
        if (Input.GetKeyDown("6"))
        {
            currentTool = "minionHouse";
            WhatsEquiped.SetText("Minion House Equipped");
            select.transform.position = H6.transform.position;
        }
        if (Input.GetKeyDown("5"))
        {
            currentTool = "wall";
            WhatsEquiped.SetText("Wall Equipped");
            select.transform.position = H5.transform.position;
        }
        if (Input.GetKeyDown("7"))
        {
            currentTool = "tower";   // NOT IMPLEMENTED
            WhatsEquiped.SetText("Tower Equipped");
            select.transform.position = H7.transform.position;
        }
        if (Input.GetKeyDown("b"))
        {
            currentTool = "demo";
            WhatsEquiped.SetText("Demolish Tool Equipped");
            Debug.Log("demo tool equiped");
        }
    }
    private void PlayerInterct() //allows mutilple function to be called from mouse button 2
    {
        if (abilitiesScript.digging) return;
        switch (currentTool)
        {
            case "Sword":
                {
                    abilitiesScript.Ability1();
                    break;
                }
            case "Interact":
                {
                    CheckInteraction();
                    break;
                }
            case "WoodCollector":
                {
                    PlaceWoodCollector();
                    break;
                }
            case "StoneCollector":
                {
                    PlaceStoneCollector();
                    break;
                }
            case "PathfindingTester":
                {
                    PathfindingTester();
                    break;
                }
            case "minionHouse":
                {
                    placeMinionHouse();
                    break;
                }
            case "wall":
                {
                    PlaceWall();
                    break;
                }
            case "demo":
                {
                    demolish();
                    break;
                }
        }
    }

    private void Axe()
    {
        animator.Play("Axe Swing");
        animator.StopPlayback();
        animator.Play("Idle");
        
    }

    private void resourceCollectionTime()
    {
        //if (currentCollectionTime == 0) { AxeSliderObject.SetActive(true); }
        currentCollectionTime += Time.deltaTime;
        AxeSlider.value = currentCollectionTime / collectTime;
        if (currentCollectionTime >= collectTime * 3 / 4) { Axe(); }
        else if (currentCollectionTime >= collectTime * 2 / 4) { Axe(); }
        else if (currentCollectionTime >= collectTime * 1 / 4) { Axe(); }
        if (currentCollectionTime >= collectTime) { AxeSlider.value = currentCollectionTime / collectTime; collecting = true; CheckInteraction(); currentCollectionTime = 0f; Axe(); }
    }

   

    private void CheckInteraction()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 CurrentPlayerPos = transform.position;
        var hits = Physics2D.BoxCastAll(MouseWorldPos, boxSize, 0, Vector2.zero); // gets list of objects at mouse postition

        if (CheckRange(CurrentPlayerPos, MouseWorldPos))
        {
            if (hits.Length > 0) // checks if object is present
            {
                foreach (var i in hits)
                {
                    if (i.transform.GetComponent<Interactable>()) // checks object is interactable
                    {
                        if (i.transform.GetComponent<TreeInteract>() || i.transform.GetComponent<StoneInteract>())
                        {
                            if (collecting) { i.transform.GetComponent<Interactable>().Interact(); }
                            collecting = false;



                            return;
                        }

                        i.transform.GetComponent<Interactable>().Interact(); // calls interaction script

                        return; // stops multiple objects from being interacted with
                    }
                }
            }
        }
    }
    private bool
        CheckRange(Vector2 PlayerPos, Vector2 TargetPos) // checks the player is in range of the interactable object 
    {
        var xlength = (PlayerPos.x - TargetPos.x) * (PlayerPos.x - TargetPos.x);
        var ylength = (PlayerPos.y - TargetPos.y) * (PlayerPos.y - TargetPos.y);


        if (xlength <= range)
        {
            if (ylength <= range)
            {

                return true;
            }
        }

        return false;
    }

    private void PathfindingTester()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        var gridPos = Init.Instance.pathfinding.GetGrid().GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
        if (pathfindingTestNodes[0] == -Vector2.one)
        {
            pathfindingTestNodes[0] = gridPos;
        }
        else
        {
            pathfindingTestNodes[1] = gridPos;
            var path = Init.Instance.pathfinding.FindPath((int)pathfindingTestNodes[0].x, (int)pathfindingTestNodes[0].y,
                (int)pathfindingTestNodes[1].x, (int)pathfindingTestNodes[1].y);
            if (path != null)
            {
                for (var i = 0; i < path.nodes.Count - 1; i++)
                {
                    var nodeSpacing = Init.Instance.cellSize / Init.Instance.nodeCount;
                    Debug.DrawLine(
                        new Vector3(path.nodes[i].x, path.nodes[i].y) * nodeSpacing + Vector3.one * nodeSpacing / 2,
                        new Vector3(path.nodes[i + 1].x, path.nodes[i + 1].y) * nodeSpacing + Vector3.one * nodeSpacing / 2,
                        Color.yellow, 5f);
                }
            }
            pathfindingTestNodes[0] = -Vector2.one;
            pathfindingTestNodes[1] = -Vector2.one;
        }
    }


    private void PlaceWoodCollector()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        if (Init.Instance.resourceManager.GetWood() >= 0)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, WoodCollector);

            if (valid)
            {

                //Init.Instance.resourceManager.AddWood(-50);

            }
            else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("INVALID LOCATION"); }
        }
        else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("NOT ENOUGH RESOURCES"); }
    }

    private void PlaceWall()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        if (Init.Instance.resourceManager.GetWood() >= 0)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, Wall);
            if (valid) { }
            else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("INVALID LOCATION"); }
        }
        else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("NOT ENOUGH RESOURCES"); }


    }


    private void PlaceStoneCollector()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        if (Init.Instance.resourceManager.GetStone() >= 0)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, StoneCollector);

            if (valid)
            {
                Init.Instance.resourceManager.AddStone(0);
            }
            else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("INVALID LOCATION"); }
        }
        else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("NOT ENOUGH RESOURCES"); }
    }
    private void placeMinionHouse()
    {


        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        if (Init.Instance.resourceManager.GetWood() >= 0)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, minionHouse);

            if (valid)
            {
                //  Init.Instance.resourceManager.AddStone(0);
            }
            else
            {
                GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity);
                temp.GetComponent<resourcePopUp>().setText("INVALID LOCATION");
            }
        }
        else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("NOT ENOUGH RESOURCES"); }

    }

    private void demolish()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
        if (gridPos.x == 24 && gridPos.y == 24)
        {
            GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("CAN'T DEMOLISH!");
        }
        else
        {
            GameObject temp = Init.Instance.grid.GetStructureAtCell((int)gridPos.x, (int)gridPos.y);
            if(temp!= null) 
            {
                int tempnumber = temp.GetComponent<Structure>().GetWoodRefund();
                Init.Instance.resourceManager.AddWood(tempnumber);
                GameObject instance = Instantiate(resourcePU, transform.position, Quaternion.identity);
                resourcePopUp instanceScript = instance.GetComponent<resourcePopUp>();
                instanceScript.setImage("wood");
                instanceScript.setText("+" + tempnumber.ToString());

                tempnumber = temp.GetComponent<Structure>().GetStoneRefund();
                Init.Instance.resourceManager.AddStone(tempnumber);
                instance = Instantiate(resourcePU, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), Quaternion.identity);
                instanceScript = instance.GetComponent<resourcePopUp>();
                instanceScript.setImage("stone");
                instanceScript.setText("+" + tempnumber.ToString());

                Init.Instance.grid.DemolishAtCell((int)gridPos.x, (int)gridPos.y);
            }                                                                
           
        }
    }
}