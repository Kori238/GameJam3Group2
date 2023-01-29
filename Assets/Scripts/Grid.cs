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
        this.Values.Add("gridPos", new Vector2(0, 0));
    }

    public Dictionary<object, object> Values
    {
        get;
        set;
    }

    public bool Build(Transform structure)
    {
        if ((bool)Values["buildable"] && Values["structure"] == null)
        {
            Transform obj = GameObject.Instantiate(structure, (Vector3)Values["center"], Quaternion.identity);
            Values["structure"] = obj.gameObject;
            Debug.Log("Successfully built " + structure + " at position " + Values["gridPosition"]);
            return true;
        }
        else
        {
            Debug.Log("Unable to build " + structure + " at position " + Values["gridPosition"] + " as there is already a structure or this is not buildable");
            return false;
        } 
    }

    public bool Demolish()
    {
        if (Values["structure"] != null)
        {
            string identifier = ((GameObject)Values["structure"]).name;
            GameObject.Destroy(((GameObject)Values["structure"]));
            Values["structure"] = null;
            Debug.Log("Successfully destroyed " + identifier + " at position " + Values["gridPosition"]);
            return true;
        }
        else
        {
            Debug.Log("Unable to demolish at position " + Values["gridPosition"] + " as there is nothing here to destroy");
            return false;
        }
    }

    public bool Damage(int amount)
    {
        return false;
        //if (Values["structure"] != null && //((Transform)Values["structure"]) = true)
        //{
//
        //} else
        //{
        //    Debug.Log("Unable to damage at position " + Values["gridPosition"] + " as there is nothing here to damage");
        //}
    }

}

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    public GridCell[,] gridArray;

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
                gridArray[x, y].Values["gridPosition"] = new Vector2(x, y); 
            }
        }
    }

    public bool BuildAtCell(int x, int y, Transform structure)
    {
        if (x > width || y > height || x < 0 || y < 0)
        {
            Debug.Log("Could not build at position " + x + " " + y + " as these co-ordinates are invalid");
            return false;
        } else
        {
            return gridArray[x, y].Build(structure);
        }
    }

    public bool DemolishAtCell(int x, int y)
    {
        if (x > width || y > height || x < 0 || y < 0)
        {
            Debug.Log("Could not destroy at position " + x + " " + y + " as these co-ordinates are invalid");
            return false;
        } else
        {
            return gridArray[x, y].Demolish();
        }
    }


    private Vector2 GetCellWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }
}

