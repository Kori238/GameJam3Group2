using TMPro;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField] private Transform WoodCollector;
    [SerializeField] private Transform StoneCollector;
    [SerializeField] private Transform Wall;
    [SerializeField] private Transform minionHouse;

    public TMP_Text WoodUI;
    [SerializeField] private Vector2[] pathfindingTestNodes = new Vector2[2];
    private readonly Vector2 boxSize = new Vector2(0.1f, 0.1f); // size of raycast

    public Transform select;
    public Transform H1; public Transform H2; public Transform H3; public Transform H4; public Transform H5; public Transform H6;

    private readonly int range = 500;
    private string currentTool = "Interact";

    [SerializeField] private ResourceManager resourceManager; // referance to the resource manager in game scene
    //options:
    //Interact
    //WoodCollector
    //StoneCollector


    private void Start()
    {
        pathfindingTestNodes[0] = -Vector2.one;
        pathfindingTestNodes[1] = -Vector2.one;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PlayerInterct();
            print("wood = " + Init.Instance.resourceManager.GetWood());
            WoodUI.text = Init.Instance.resourceManager.GetWood().ToString();
        }
        if (Input.GetKeyDown("2"))
        {
            currentTool = "Interact";
            print("interact Tool Equiped");
            select.transform.position = H2.transform.position;
        }
        if (Input.GetKeyDown("3"))
        {
            currentTool = "WoodCollector";
            print("Build Wood Collector equiped");
            select.transform.position = H3.transform.position;
        }
        if (Input.GetKeyDown("4"))
        {
            currentTool = "StoneCollector";
            Debug.Log("Build Stone Collector equiped");
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
            Debug.Log("build minion house equiped");
            select.transform.position = H6.transform.position;
        }
        if (Input.GetKeyDown("5"))
        {
            currentTool = "wall";
            Debug.Log("build wall equipped");
            select.transform.position = H5.transform.position;
        }
    }
    private void PlayerInterct() //allows mutilple function to be called from mouse button 2
    {
        switch (currentTool)
        {
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
                print("in range");
                return true;
            }
        }
        print("not in range");
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
        }
    }

    private void PlaceWall()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
        var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, Wall);
    }


    private void PlaceStoneCollector()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        if (Init.Instance.resourceManager.GetStone() >= 50)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y, StoneCollector);

            if (valid)
            {
                Init.Instance.resourceManager.AddStone(-50);
            }
        }
    }
    private void placeMinionHouse()
    {


        Vector2 MousePos = Input.mousePosition;
        Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        Vector2 gridPos;
        if (Init.Instance.resourceManager.GetWood() >= 0)
        {
            gridPos = Init.Instance.grid.GetWorldCellPosition(MouseWorldPos.x, MouseWorldPos.y);
            var valid = Init.Instance.grid.BuildAtCell((int)gridPos.x, (int)gridPos.y,minionHouse);

            if (valid)
            {
              //  Init.Instance.resourceManager.AddStone(0);
            }
        }

    }
}