using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInit : MonoBehaviour
{

    public Transform tree;
    private void Start()
    {
        Grid grid = new Grid(9, 9, 10f);
        grid.gridArray[2, 2].Build(tree);
        //grid.BuildAtCell(2, 2, tree);
        //grid.BuildAtCell(2, 2, tree);
        //grid.BuildAtCell(5, 4, tree);
        //grid.Demolish(2, 2);
        grid.gridArray[2, 5].Demolish();
    }
}
