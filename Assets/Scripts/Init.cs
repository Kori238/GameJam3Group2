using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Init : MonoBehaviour
{
    public Transform tree;
    public Transform wall;
    public Transform home;
    public Transform attackPointPrefab;
    public Vector2 gridDimensions = new Vector2(18, 10);
    public float cellSize = 10f;
    public int nodeCount = 3;
    public bool wallDemo;
    public bool testPathfinding;
    public bool debug = true;
    private PerformanceCounter _cpuCounter;
    public Grid grid;
    public AStar pathfinding;
    private PerformanceCounter _ramCounter;
    public ResourceManager resourceManager;
    public static Init Instance { get; private set; }

    private void FixedUpdate()
    {
        print(Time.deltaTime);
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        pathfinding = new AStar((int)gridDimensions.x * nodeCount, (int)gridDimensions.y * nodeCount, cellSize / nodeCount);

        _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_ Total");
        _ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        grid = new Grid((int)gridDimensions.x, (int)gridDimensions.y, cellSize);
        resourceManager = new ResourceManager();
        grid.BuildAtCell(5, 5, tree);
        grid.BuildAtCell((int)(gridDimensions.x - 1) / 2, (int)(gridDimensions.y - 1) / 2, home);
        if (wallDemo)
        {
            StartCoroutine(BuildWalls());
        }
        if (!debug)
            return;
        StartCoroutine(pathfinding.GetGrid().DrawNodeOutline());
        StartCoroutine(grid.DrawGridOutline());
    }

    public float GetCurrentCPUUsage()
    {
        return _cpuCounter.NextValue();
    }
    public float GetAvailableRam()
    {
        return _ramCounter.NextValue();
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
        grid.BuildAtCell(10, 5, wall);
        grid.BuildAtCell(10, 4, wall);
        grid.BuildAtCell(10, 3, wall);
        grid.BuildAtCell(10, 2, wall);
        grid.BuildAtCell(9, 2, wall);
        grid.BuildAtCell(8, 2, wall);
        grid.BuildAtCell(7, 2, wall);


        yield return null;
    }
}
