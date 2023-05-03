using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class Init : MonoBehaviour
{
    public Transform tree;
    public Transform stone;
    public Transform wall;
    public Transform home;
    public Transform tower;
    public Transform gridOutline;
    public Transform attackPointPrefab;
    public Vector2 gridDimensions = new Vector2(18, 10);
    public float cellSize = 10f;
    public int nodeCount = 3;
    public float fps = 0;
    public float runtimeFPS = 0;
    public bool highUsage = false;
    public bool wallDemo;
    public bool testPathfinding;
    public bool debug = true;
    public Grid grid;
    public AStar pathfinding;
    public ResourceManager resourceManager;
    public GameObject resourceUI;
    public static Init Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        pathfinding = new AStar((int)gridDimensions.x * nodeCount, (int)gridDimensions.y * nodeCount, cellSize / nodeCount);
        grid = new Grid((int)gridDimensions.x, (int)gridDimensions.y, cellSize);
        resourceManager = new ResourceManager();
        resourceManager.resourceUI = resourceUI.GetComponent<resourceUIScript>();
        grid.BuildAtCell((int)(gridDimensions.x - 1) / 2, (int)(gridDimensions.y - 1) / 2, home);
        if (wallDemo)
        {
            StartCoroutine(BuildWalls());
        }
        StartCoroutine(GridHighlights());
        if (!debug)
            return;
        StartCoroutine(pathfinding.GetGrid().DrawNodeOutline());
        StartCoroutine(grid.DrawGridOutline());
    }

    private IEnumerator GridHighlights()
    {
        Vector2 previousGridCellPosition = new Vector2(0, 0);
        while (true)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector2 gridCellPosition = grid.GetWorldCellPosition(mouseWorldPos.x, mouseWorldPos.y);
            grid.OutlineCell((int)gridCellPosition.x, (int)gridCellPosition.y, gridOutline);
            if (previousGridCellPosition != gridCellPosition)
            {
                grid.DeOutlineCell((int)previousGridCellPosition.x, (int)previousGridCellPosition.y);
            }
            previousGridCellPosition = gridCellPosition;
            yield return new WaitForSeconds(0.4f);
        }
    }

    private IEnumerator BuildWalls()
    {
        grid.BuildAtCell(23, 24, tower); //LM
        grid.BuildAtCell(24, 25, tower); //TM
        grid.BuildAtCell(24, 23, tower); //BM
        grid.BuildAtCell(25, 24, tower); //RM


        grid.BuildAtCell(23, 25, tower); //LM
        grid.BuildAtCell(25, 25, tower); //TM
        grid.BuildAtCell(23, 23, tower); //BM
        grid.BuildAtCell(25, 23, tower); //RM

        yield return null;
    }
}
