using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class GridCell
{
    public GridCell()
    {
        this.Values = new Dictionary<object, object>(); // Creates a dictionary and initial values for a the grid cell class
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
        this.width = width; // this is called by GridInit to define the width, height and cellSize there
        this.height = height;
        this.cellSize = cellSize;
        

        gridArray = new GridCell[width, height]; // creates a 2D array of GridCell type

        for (int x = 0; x < gridArray.GetLength(0); x++) // iterates through each cell
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridCell(); // creates a GridCell object at this cell
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x, y + 1), Color.white, 100f); // visual outline of cell gizmos
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x + 1, y), Color.white, 100f);
                gridArray[x, y].Values["center"] = new Vector3(x, y) * cellSize - new Vector3(cellSize/2, cellSize/2); // calculates and stores the center position of the cell to the Dictionary
            }
        }
    }

    public void Build(int x, int y, Transform obj)
    {
        if ((bool)gridArray[x, y].Values["buildable"] && gridArray[x, y].Values["structure"] == null) { // if this cell can be built on
            Transform instantiatedObj = GameObject.Instantiate(obj, (Vector3)gridArray[x, y].Values["center"], Quaternion.identity); // instantiate the object and store it in the dictionary
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

