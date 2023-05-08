using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField] private Transform WoodCollector;
    [SerializeField] private Transform StoneCollector;
    [SerializeField] private Transform Wall;
    [SerializeField] private Transform minionHouse;
    [SerializeField] private GameObject resourcePU;
    [SerializeField] private TMP_Text WhatsEquiped;
    [SerializeField] private Transform Tower;
    [SerializeField] private Transform TowerArcher;
    [SerializeField] private Transform TowerArtillery;
    [SerializeField] private GameObject buildingCostPanel;
    [SerializeField] private TextMeshProUGUI woodCost;
    [SerializeField] private TextMeshProUGUI stoneCost;

    [SerializeField] private GameObject BuildingNotificationPrefab;

    public TMP_Text WoodUI;
    [SerializeField] private Vector2[] pathfindingTestNodes = new Vector2[2];
    private readonly Vector2 boxSize = new Vector2(0.1f, 0.1f); // size of raycast

    public Transform select;
    public Transform H1; public Transform H2; public Transform H3; public Transform H4; public Transform H5; public Transform H6; public Transform H7; public Transform H8; public Transform H9; public Transform B1;
    public Slider AxeSlider;
    public GameObject AxeSliderObject;
    public GameObject DemoBorder;
    public S_SoundController sounder;

    private readonly int range = 5000;
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
    private bool tryingToCollect=false;
  


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
            
            if(tryingToCollect) {
                AxeSliderObject.SetActive(true);
                animator.Play("Axe Swing");
                resourceCollectionTime();
            }
        }
        if (Input.GetMouseButton(0) && currentTool == "repair") { repairTool(); }
        if (Input.GetMouseButtonUp(0) && currentTool == "Interact")
        {
            AxeSliderObject.SetActive(false);
            animator.Play("Idle");
            tryingToCollect = false;
        }

        if (Input.GetKeyDown("1"))
        {
            DemoBorder.SetActive(false);
            currentTool = "Sword";
            WhatsEquiped.SetText("Sword Equipped");
            select.transform.position = H1.transform.position;
            buildingCostPanel.SetActive(false);

        }
        if (Input.GetKeyDown("2"))
        {
            DemoBorder.SetActive(false);
            currentTool = "Interact";
            WhatsEquiped.SetText("Interact/Axe Tool Equipped");
            select.transform.position = H2.transform.position;
            buildingCostPanel.SetActive(false);
        }
        if (Input.GetKeyDown("3"))
        {
            DemoBorder.SetActive(false);
            currentTool = "WoodCollector";
            WhatsEquiped.SetText("Wood Collector Equipped");
            select.transform.position = H3.transform.position;
            buildingCostPanel.SetActive(true);
            woodCost.SetText("100    <sprite=0>");
            stoneCost.SetText("");
        }
        if (Input.GetKeyDown("4"))
        {
            DemoBorder.SetActive(false);
            currentTool = "StoneCollector";
            WhatsEquiped.SetText("Stone Collector Equipped");
            select.transform.position = H4.transform.position;
            buildingCostPanel.SetActive(true);
            woodCost.SetText("50    <sprite=0>");
            stoneCost.SetText("100  <sprite=0>");
        }
        if (Init.Instance.testPathfinding && Input.GetKeyDown(KeyCode.T))
        {
            DemoBorder.SetActive(false);
            currentTool = "PathfindingTester";
            Debug.Log("Pathfinding Tester equipped");
           
        }
        if (Input.GetKeyDown("6"))
        {
            DemoBorder.SetActive(false);
            currentTool = "minionHouse";
            WhatsEquiped.SetText("Minion House Equipped");
            select.transform.position = H6.transform.position;
            buildingCostPanel.SetActive(true);
            woodCost.SetText("100   <sprite=0>");
            stoneCost.SetText("");
            
        }
        if (Input.GetKeyDown("5"))
        {
            DemoBorder.SetActive(false);
            currentTool = "wall";
            WhatsEquiped.SetText("Wall Equipped");
            select.transform.position = H5.transform.position;
            buildingCostPanel.SetActive(true);
            woodCost.SetText("20    <sprite=0>");
            stoneCost.SetText("");
            
        }
        if (Input.GetKeyDown("7"))
        {
            DemoBorder.SetActive(false);
            currentTool = "tower";   
            WhatsEquiped.SetText("Magic Tower Equipped");
            select.transform.position = H7.transform.position;
            buildingCostPanel.SetActive(true);
            woodCost.SetText("50    <sprite=0>");
            stoneCost.SetText("100  <sprite=0>");
            
        }
        if (Input.GetKeyDown("8"))
        {
            DemoBorder.SetActive(false);
            currentTool = "towerArcher";
            WhatsEquiped.SetText("Archer Tower Equipped");
            select.transform.position = H8.transform.position;
            buildingCostPanel.SetActive(true);
            woodCost.SetText("50    <sprite=0>");
            stoneCost.SetText("100  <sprite=0>");
            
        }
        if (Input.GetKeyDown("9"))
        {
            DemoBorder.SetActive(false);
            currentTool = "towerArtillery";
            WhatsEquiped.SetText("Artillery Tower Equipped");
            select.transform.position = H9.transform.position;
            buildingCostPanel.SetActive(true);
            woodCost.SetText("50    <sprite=0>");
            stoneCost.SetText("100  <sprite=0>");
           
        }
        if (Input.GetKeyDown("b"))
        {
            DemoBorder.SetActive(true);
            sounder.GetComponent<S_SoundController>().DemolishSound();
            select.transform.position = B1.transform.position;
            currentTool = "demo";
            WhatsEquiped.SetText("Demolish Tool Equipped");
            Debug.Log("demo tool equiped");
            buildingCostPanel.SetActive(false);
            

        }
        if (Input.GetKeyDown("r")) 
        {
            DemoBorder.SetActive(false);
            currentTool = "repair";
            WhatsEquiped.SetText("Repair Tool Equipped");
            buildingCostPanel.SetActive(false);
           

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
            case "tower":
                {
                    PlaceTower(Tower);
                    break;
                }
            case "demo":
                {
                    demolish();
                    break;
                }
            case "towerArcher":
                {
                    PlaceTower(TowerArcher);
                    break;
                }
            case "towerArtillery":
                {
                    PlaceTower(TowerArtillery);
                    break;
                }
            case "repair":
                {
                    repairTool();
                        break;
                }
        }
    }

    //private void Axe()
    //{
    //    animator.Play("Axe Swing");
    //    animator.StopPlayback();
        
    //}

    private void resourceCollectionTime()
    {
        currentCollectionTime += Time.deltaTime;
        AxeSlider.value = currentCollectionTime / collectTime;
      
        if (currentCollectionTime >= collectTime) 
        {
            AxeSlider.value = currentCollectionTime / collectTime;
            collecting = true; 
            CheckInteraction();
            currentCollectionTime = 0f; 
            
                                                                                                                                                                              }
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
                            tryingToCollect= true;
                            if (collecting) { i.transform.GetComponent<Interactable>().Interact(); }
                            collecting = false;

                            sounder.WoodMine();

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
        if (Init.Instance.resourceManager.GetWood() >= 100)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, WoodCollector);

            if (valid)
            {

                Init.Instance.resourceManager.AddWood(-100);

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
        if (Init.Instance.resourceManager.GetWood() >= 20)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, Wall);
            if (valid)
            {

                Init.Instance.resourceManager.AddWood(-20);

            }
            else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("INVALID LOCATION"); }
        }
        else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("NOT ENOUGH RESOURCES"); }


    }

    private void PlaceTower(Transform newTower)
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        if (Init.Instance.resourceManager.GetWood() >= 50 && Init.Instance.resourceManager.GetStone()>= 100)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, newTower);

            if (valid)
            {

                Init.Instance.resourceManager.AddWood(-50);
                Init.Instance.resourceManager.AddStone(-100);

            }
            else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("INVALID LOCATION"); }
        }
        else { GameObject temp = Instantiate(BuildingNotificationPrefab, transform.position, Quaternion.identity); temp.GetComponent<resourcePopUp>().setText("NOT ENOUGH RESOURCES"); }
    }



    private void PlaceStoneCollector()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        if (Init.Instance.resourceManager.GetWood()>=50 && Init.Instance.resourceManager.GetStone() >= 100)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, StoneCollector);

            if (valid)
            {
                Init.Instance.resourceManager.AddWood(-50);
                Init.Instance.resourceManager.AddStone(-100);
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
        if (Init.Instance.resourceManager.GetWood() >= 100)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, minionHouse);

            if (valid)
            {
                  Init.Instance.resourceManager.AddWood(-100);
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
                if (tempnumber > 0)
                {
                    Init.Instance.resourceManager.AddWood(tempnumber);
                    GameObject instance = Instantiate(resourcePU, transform.position, Quaternion.identity);
                    resourcePopUp instanceScript = instance.GetComponent<resourcePopUp>();
                    instanceScript.setImage("wood");
                    instanceScript.setText("+" + tempnumber.ToString());
                }
                    

                tempnumber = temp.GetComponent<Structure>().GetStoneRefund();
                if(tempnumber>0) 
                {
                    Init.Instance.resourceManager.AddStone(tempnumber);
                    GameObject instance = Instantiate(resourcePU, new Vector3(transform.position.x, transform.position.y - 10, transform.position.z), Quaternion.identity);
                    resourcePopUp instanceScript = instance.GetComponent<resourcePopUp>();
                    instanceScript.setImage("stone");
                    instanceScript.setText("+" + tempnumber.ToString());
                }
             

                Init.Instance.grid.DemolishAtCell((int)gridPos.x, (int)gridPos.y);
            }                                                                
           
        }
    }

    private void repairTool()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 CurrentPlayerPos = transform.position;
        var hits = Physics2D.BoxCastAll(MouseWorldPos, boxSize, 0, Vector2.zero); // gets list of objects at mouse postition
        if (hits.Length > 0) // checks if object is present
        {
            foreach (var i in hits)
            {
                if (i.transform.GetComponent<Interactable>()) // checks object is interactable
                
                {
                    Structure temp = i.transform.GetComponent<Structure>();
                    temp.repair();
                }
            }
        }
    }

}