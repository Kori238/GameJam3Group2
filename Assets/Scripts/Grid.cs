using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class GridCell
{
    public GridCell()
    {
        this.Values = new Dictionary<object, object>();
        this.Values.Add("buildable", true);
        this.Values.Add("structure", null);
        this.Values.Add("center", new Vector3(0, 0, 0));
    }

    public Dictionary<object, object> Values
    {
        get;
        set;
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
                gridArray[x, y].Values["center"] = new Vector3(x, y) * cellSize - new Vector3(cellSize/2, cellSize/2);
            }
        }
    }

    public void Build(int x, int y, Transform obj)
    {
        if ((bool)gridArray[x, y].Values["buildable"] && gridArray[x, y].Values["structure"] == null) {
            Transform instantiatedObj = GameObject.Instantiate(obj, (Vector3)gridArray[x, y].Values["center"], Quaternion.identity);
            gridArray[x, y].Values["structure"] = instantiatedObj;
        } else
        {
            Debug.Log("Unable to build " + obj + " at position " + x + " " + y + " as there is already a structure or this is not buildable");
        }

    }
        

    private Vector2 GetCellWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }
}

