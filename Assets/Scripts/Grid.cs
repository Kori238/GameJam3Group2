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

    public void Build(Transform structure)
    {
        if ((bool)Values["buildable"] && Values["structure"] == null)
        {
            Transform obj = GameObject.Instantiate(structure, (Vector3)Values["center"], Quaternion.identity);
            Values["structure"] = obj.gameObject;
            Debug.Log("Successfully built " + structure + " at position " + Values["gridPosition"]);
        }
        else
        {
            Debug.Log("Unable to build " + structure + " at position " + Values["gridPosition"] + " as there is already a structure or this is not buildable");
        } 
    }

    public void Demolish()
    {
        if (Values["structure"] != null)
        {
            string identifier = ((GameObject)Values["structure"]).name;
            GameObject.Destroy(((GameObject)Values["structure"]));
            Values["structure"] = null;
            Debug.Log("Successfully destroyed " + identifier + " at position " + Values["gridPosition"]);
        }
        else
        {
            Debug.Log("Unable to demolish at position " + Values["gridPosition"] + " as there is nothing here to destroy");
        }
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

    //public void BuildAtCell(int x, int y, Transform obj)
    //{
        //gridArray[x, y].Build(obj);

        //if ((bool)gridArray[x, y].Values["buildable"] && gridArray[x, y].Values["structure"] == null) { // if this cell can be built on
        //    Transform instantiatedObj = GameObject.Instantiate(obj, (Vector3)gridArray[x, y].Values["center"], Quaternion.identity); // instantiate the object and store it in the dictionary
        //    gridArray[x, y].Values["structure"] = instantiatedObj.gameObject;
        //    Debug.Log("Successfully built " + obj + " at position " + x + " " + y);
        //} else
        //{
        //    Debug.Log("Unable to build " + obj + " at position " + x + " " + y + " as there is already a structure or this is not buildable");
        //}
    //}
    //public void Demolish(int x, int y)
    //
        //if (gridArray[x, y].Values["structure"] != null)
        //{
           //string identifier = ((GameObject)gridArray[x, y].Values["structure"]).name;
            //GameObject.Destroy(((GameObject)gridArray[x, y].Values["structure"]));
            //gridArray[x, y].Values["structure"] = null;
            //Debug.Log("Successfully destroyed " + identifier + " at position " + x + " " + y);
        //} else
        //{
        //    Debug.Log("Unable to demolish at position " + x + " " + y + " as there is nothing here to destroy");
        //}
    //} 

    private Vector2 GetCellWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }
}

