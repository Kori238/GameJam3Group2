using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wall : Structure
{

    public override void Destroyed() //Overrides Structure method in order to update the connectors sprites
    {
        base.Destroyed();
        gameObject.GetComponent<WallConnectors>().updateAllConnectors();
    }

    public override void Demolished() //Overrides Structure method in order to deconnect adjacent walls when destroyed
    {
        gameObject.GetComponent<WallConnectors>().updateAllConnectors();
        base.Demolished();
    }

    public override void FindOccupiedSpace()
    {
        Vector2 nodePos = gridPos * 3;
        NodeGrid nodeGrid = Init.Instance.pathfinding.GetGrid();
        occupiedSpace.Add(nodeGrid.gridArray[(int)nodePos.x+1, (int)nodePos.y+1]);
        bool[] adjacentWalls = gameObject.GetComponent<WallConnectors>().adjacent;
        if (adjacentWalls[0]) occupiedSpace.Add(nodeGrid.gridArray[(int)nodePos.x + 1, (int)nodePos.y + 2]); //North Connector
        if (adjacentWalls[1]) occupiedSpace.Add(nodeGrid.gridArray[(int)nodePos.x + 2, (int)nodePos.y + 1]); //East Connector
        if (adjacentWalls[2]) occupiedSpace.Add(nodeGrid.gridArray[(int)nodePos.x + 1, (int)nodePos.y]); //South Connector
        if (adjacentWalls[3]) occupiedSpace.Add(nodeGrid.gridArray[(int)nodePos.x, (int)nodePos.y + 1]); //West Connector
    }

    public void UpdateFindOccupiedSpace()
    {
        if (destroyed) return;
        DeoccupySpace();
        occupiedSpace = new();
        FindOccupiedSpace();
        OccupySpace();
    }
}
