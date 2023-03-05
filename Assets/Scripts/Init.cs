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
    public Transform attackPointPrefab;
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
        grid.BuildAtCell(6, 2, wall);
        grid.BuildAtCell(6, 3, wall);
        grid.BuildAtCell(6, 4, wall);
        grid.BuildAtCell(6, 5, wall);
        grid.BuildAtCell(6, 6, wall);
        grid.BuildAtCell(7, 6, wall);
        grid.BuildAtCell(8, 6, wall);
        grid.BuildAtCell(9, 6, wall);
        grid.BuildAtCell(10, 6, wall);


        yield return null;
    }
}
