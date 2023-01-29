using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInit : MonoBehaviour
{

    public Transform tree;
    public Transform wall;
    private void Start()
    {
        Grid grid = new Grid(9, 9, 10f);
        grid.BuildAtCell(2, 2, wall);
        grid.BuildAtCell(2, 2, wall);
        grid.BuildAtCell(5, 4, tree);
        grid.DemolishAtCell(2, 2);
        grid.DemolishAtCell(2, 2);
        grid.BuildAtCell(2, 2, wall);
        grid.DemolishAtCell(10, 14);
        grid.BuildAtCell(20, -100, tree);
        grid.DamageAtCell(2, 2, 10f);
        grid.DamageAtCell(5, 4, 10f);
        grid.DamageAtCell(20, -100, 10f);
    }
}
