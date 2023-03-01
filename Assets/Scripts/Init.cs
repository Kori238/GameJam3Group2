using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    private static Init _instance;
    public static Init Instance {  get { return _instance;  } }

    public Transform tree;
    public Transform wall;
    public Transform home;
    public Grid grid;
    public AStar pathfinding;
    [SerializeField] Vector2 gridDimensions = new Vector2(18, 10);
    public float cellSize = 10f;
    public int nodeCount = 3;
    public ResourceManager resourceManager;
    public bool wallDemo = false;
    public bool testPathfinding = false;
    public bool debug = true;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        } else
        _instance = this;
        pathfinding = new AStar((int)gridDimensions.x * nodeCount, (int)gridDimensions.y * nodeCount, cellSize / nodeCount);
        grid = new Grid((int)gridDimensions.x, (int)gridDimensions.y, cellSize);
        resourceManager = new ResourceManager();
        grid.BuildAtCell(5, 5, tree);
        grid.BuildAtCell((int)(gridDimensions.x-1) / 2, (int)(gridDimensions.y-1) / 2, home);
        if (wallDemo)
        {
            StartCoroutine(BuildWalls());
        }
        if (debug)
        {
            StartCoroutine(pathfinding.GetGrid().DrawNodeOutline());
            StartCoroutine(grid.DrawGridOutline());
        }

    }

    private void Start()
    {
        
    }

    private IEnumerator BuildWalls()
    {
        grid.BuildAtCell(3, 3, wall);
        grid.BuildAtCell(3, 4, wall);
        grid.BuildAtCell(3, 5, wall);
        grid.BuildAtCell(3, 6, wall);
        grid.BuildAtCell(3, 7, wall);
        grid.BuildAtCell(3, 8, wall);
        grid.BuildAtCell(4, 8, wall);
        grid.BuildAtCell(5, 8, wall);
        grid.BuildAtCell(5, 7, wall);
        
        yield return new WaitForSeconds(5f);
        grid.DamageAtCell(3, 5, 25);
        grid.DamageAtCell(3, 5, 25);

        yield return new WaitForSeconds(10f);
        grid.DemolishAtCell(3, 8);

        yield return new WaitForSeconds(5f);
        grid.DemolishAtCell(3, 7);
        //grid.DemolishAtCell(3, 5);
        yield return null;
    }
}
