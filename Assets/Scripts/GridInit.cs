using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInit : MonoBehaviour
{
    private void Start()
    {
        Grid grid = new Grid(9, 9, 10f);
    }
}
