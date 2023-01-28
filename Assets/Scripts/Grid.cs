using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridCell
{
    private int val;
    private GameObject obj = null;
    private bool buildable = true;
}

public class Grid
{

    private int width;
    private int height;
    private float cellSize;
    private GridCell[,] gridArray;

    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new GridCell[width, height];

        for (int x = (-(gridArray.GetLength(0) - 1)/2); x < (gridArray.GetLength(0)+1)/2; x++)
        {
            for (int y = (-(gridArray.GetLength(0) - 1) / 2); y < (gridArray.GetLength(0)+1)/2; y++)
            {
                Debug.Log(GetCellWorldPosition(x, y));
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
    }

    private Vector2 GetCellWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }
}

