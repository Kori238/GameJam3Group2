using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInit : MonoBehaviour
{

    public Transform tree;
    private void Start()
    {
        Grid grid = new Grid(9, 9, 10f);
        grid.Build(2, 2, tree);
        grid.Build(2, 2, tree);
        grid.Build(5, 4, tree);
    }
}
