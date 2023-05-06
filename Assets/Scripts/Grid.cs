using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public GridCell()
    {
        Values = new Dictionary<string, object>
        {
            { "buildable", true },
            { "structure", null },
            { "center", new Vector3(0, 0, 0) },
            { "gridPos", new Vector2(0, 0) },
            { "pathfindable", true },
            { "outline", null}
        }; // Creates a dictionary and initial values for a the grid cell class
    }

    public Dictionary<string, object> Values { get; set; }

    public bool Build(Transform structure)
    {
        if ((bool)Values["buildable"] && Values["structure"] == null)
        {
            var obj = Object.Instantiate(structure, (Vector3)Values["center"], Quaternion.identity);
            Values["structure"] = obj.gameObject;
            Debug.Log("Successfully built " + structure + " at position " + Values["gridPos"]);
            obj.gameObject.GetComponent<Structure>().gridPos = (Vector2)Values["gridPos"];
            Values["pathfindable"] = false;
            return true;
        }
        Debug.Log("Unable to build " + structure + " at position " + Values["gridPos"] +
                  " as there is already a structure or this is not buildable");
        return false;
    }

    public bool Demolish()
    {
        if (Values["structure"] != null)
        {
            var tempObj =
                (GameObject)Values["structure"]; //Creates a temporary object so that the demolished function can be called after the dictionary entry being emptied
            Values["structure"] = null;
            tempObj.GetComponent<Structure>().Demolished();
            Debug.Log("Successfully destroyed " + tempObj.name + " at position " + Values["gridPos"]);
            Values["pathfindable"] = true;
            return true;
        }
        Debug.Log("Unable to demolish at position " + Values["gridPos"] + " as there is nothing here to destroy");
        return false;
    }

    public bool Damage(float amount)
    {
        if (Values["structure"] != null && ((GameObject)Values["structure"]).GetComponent<Structure>().damageable)
        {
            ((GameObject)Values["structure"]).GetComponent<Structure>().Damaged(amount);
            Debug.Log("Dealt " + amount + " damage to " + ((GameObject)Values["structure"]).name + " at position " +
                      Values["gridPos"]);
            return true;
        }
        Debug.Log("Unable to damage at position " + Values["gridPos"] +
                  " as there is nothing here to damage or this is undamageable");
        return false;
    }

    public bool Heal(float amount)
    {
        if (Values["structure"] != null)
        {
            ((GameObject)Values["structure"]).GetComponent<Structure>().Healed(amount);
            Debug.Log("Restored " + amount + " health to " + ((GameObject)Values["structure"]).name + " at position " +
                      Values["gridPos"]);
            return true;
        }
        Debug.Log("Unable to heal at position " + Values["gridPos"] + " as there is nothing here to heal");
        return false;
    }

    public bool SetHealth(float amount, bool fullyHeal = false)
    {
        if (Values["structure"] != null)
        {
            ((GameObject)Values["structure"]).GetComponent<Structure>().SetHealth(amount, fullyHeal);
            if (fullyHeal)
            {
                Debug.Log("Fully healed " + ((GameObject)Values["structure"]).name + " at position " + Values["gridPos"]);
            }
            else
            {
                Debug.Log("Set health of  " + ((GameObject)Values["structure"]).name + "  to " + amount + " at position " +
                          Values["gridPos"]);
            }
            return true;
        }
        Debug.Log("Unable to set health at position " + Values["gridPos"] + " as there is nothing here");
        return false;
    }

    public void Outline(Transform outline)
    {
        if ((Transform)Values["outline"] == null)
        {
            var obj = Object.Instantiate(outline, (Vector3)Values["center"] - new Vector3(5, 5), Quaternion.identity);
            Values["outline"] = obj;
        }
        else
        {
            ((Transform)Values["outline"]).GetComponent<GridHighlight>().hovering = true;
        }
    }

    public void DeOutline()
    {
        if (Values["outline"] == null) return;
        ((Transform)Values["outline"]).GetComponent<GridHighlight>().hovering = false;
    }

}

public class Grid
{
    private readonly float _cellSize;
    private readonly int _height;
    private readonly int _width;
    public readonly GridCell[,] gridArray;

    public Grid(int width, int height, float cellSize)
    {
        _width = width; // this is called by Init to define the width, height and cellSize there
        _height = height;
        _cellSize = cellSize;


        gridArray = new GridCell[width, height]; // creates a 2D array of GridCell type

        for (var x = 0; x < gridArray.GetLength(0); x++) // iterates through each cell
        {
            for (var y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridCell
                {
                    Values =
                    {
                        ["center"] = new Vector3(x, y) * cellSize +
                                     new Vector3(cellSize / 2,
                                         cellSize / 2), // calculates and stores the center position of the cell to the Dictionary
                        ["gridPos"] = new Vector2(x, y)
                    }
                }; // creates a GridCell object at this cell
            }
        }
    }
    public IEnumerator DrawGridOutline()
    {
        for (var x = 0; x < gridArray.GetLength(0); x++) // iterates through each cell
        {
            for (var y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x, y + 1), Color.white,
                    10f); // visual outline of cell gizmos
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x + 1, y), Color.white, 10f);
            }
        }
        yield return new WaitForSeconds(10f);
        yield return DrawGridOutline();
    }

    public void OutlineCell(int x, int y, Transform outline)
    {
        if (x > _width-1 || y > _height-1 || x < 0 || y < 0)
        {
            Debug.Log("Could not outline cell at position " + x + " " + y + "as these co-ordinates are invalid");
            return;
        }
        gridArray[x, y].Outline(outline);
    }

    public void DeOutlineCell(int x, int y)
    {
        if (x > _width-1 || y > _height-1 || x < 0 || y < 0)
        {
            Debug.Log("Could not deoutline cell at position " + x + " " + y + "as these co-ordinates are invalid");
            return;
        }
        gridArray[x, y].DeOutline();
    }
    public bool BuildAtCell(int x, int y, Transform structure)
    {
        if (x > _width || y > _height || x < 0 || y < 0)
        {
            Debug.Log("Could not build at position " + x + " " + y + " as these co-ordinates are invalid");
            return false;
        }
        return gridArray[x, y].Build(structure);
    }

    public bool DemolishAtCell(int x, int y)
    {
        if (x > _width || y > _height || x < 0 || y < 0)
        {
            Debug.Log("Could not destroy at position " + x + " " + y + " as these co-ordinates are invalid");
            return false;
        }
        return gridArray[x, y].Demolish();
    }

    public bool DamageAtCell(int x, int y, float amount)
    {
        if (x > _width || y > _height || x < 0 || y < 0)
        {
            Debug.Log("Could not damage at position " + x + " " + y + " as these co-ordinates are invalid");
            return false;
        }
        return gridArray[x, y].Damage(amount);
    }

    public GameObject GetStructureAtCell(int x, int y)
    {
        if (x <= _width && y <= _height && x >= 0 && y >= 0)
            return (GameObject)gridArray[x, y].Values["structure"];
        Debug.Log("Attempted to find structure at " + x + " " + y + " but these co-ordinates are out of range");
        return null;
    }

    public bool HealAtCell(int x, int y, float amount)
    {
        if (x > _width || y > _height || x < 0 || y < 0)
        {
            Debug.Log("Could not heal at position " + x + " " + y + " as these co-ordinates are invalid");
            return false;
        }
        return gridArray[x, y].Heal(amount);
    }

    public bool SetHealthAtCell(int x, int y, float amount, bool fullyHeal = false)
    {
        if (x > _width || y > _height || x < 0 || y < 0)
        {
            Debug.Log("Could not set health at position " + x + " " + y + " as these co-ordinates are invalid");
            return false;
        }
        return gridArray[x, y].SetHealth(amount, fullyHeal);
    }


    private Vector2 GetCellWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * _cellSize;
    }

    public Vector2 GetWorldCellPosition(float x, float y)
    {
        return new Vector2(Mathf.FloorToInt(x / _cellSize), Mathf.FloorToInt(y / _cellSize));
    }
}
