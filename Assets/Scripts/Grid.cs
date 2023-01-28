using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridCell
{
    public GridCell()
    {
        this.Values = new Dictionary<object, object>();
        this.Values.Add("buildable", true);
        this.Values.Add("structure", null);
    }

    public Dictionary<object, object> Values
    {
        get;
        set;
    }
    
    public void Awake()
    {
        
    }

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

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridCell();
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x + 1, y), Color.white, 100f);
                gridArray[x, y].Values.Add("centerPosition", new Vector2(x, y) * cellSize - new Vector2(cellSize/2, cellSize/2));
                Debug.Log(gridArray[x, y].Values["buildable"]);
            }
        }
    }

    private Vector2 GetCellWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }
}

