using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    private static Init _instance;
    public static Init Instance {  get { return _instance;  } }

    public Transform tree;
    public Transform wall;
    public Grid grid;
    public AStar pathfinding;
    [SerializeField] Vector2 gridDimensions = new Vector2(18, 10);
    public float cellSize = 10f;
    public int nodeCount = 3;
    public ResourceManager resourceManager;
    public bool wallDemo = false;
    public bool testPathfinding = false;

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
        if (wallDemo)
        {
            StartCoroutine(BuildWalls());
        }
        
    }
    
    private void Start()
    {
        
    }

    private IEnumerator BuildWalls()
    {
        grid.BuildAtCell(0, 0, wall);
        grid.BuildAtCell(3, 3, wall);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.BuildAtCell(3, 4, wall);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.BuildAtCell(4, 3, wall);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.BuildAtCell(4, 4, wall);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.DemolishAtCell(4, 4);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.BuildAtCell(3, 5, wall);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.BuildAtCell(3, 2, wall);
        yield return new WaitForSeconds(3);
        grid.SetHealthAtCell(3, 3, 0, true);
        grid.BuildAtCell(2, 3, wall);
        yield return new WaitForSeconds(3);
        grid.BuildAtCell(2, 2, wall);
        grid.SetHealthAtCell(2, 2, 0);
    }
}
