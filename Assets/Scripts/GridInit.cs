using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInit : MonoBehaviour
{
    private static GridInit _instance;
    public static GridInit Instance {  get { return _instance;  } }

    public Transform tree;
    public Transform wall;
    public Grid grid;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        } else
        _instance = this;
        grid = new Grid(9, 9, 10f);
        StartCoroutine(BuildWalls());
    }
    
    private void Start()
    {
        
    }

    private IEnumerator BuildWalls()
    {
        grid.BuildAtCell(3, 3, wall);
        yield return new WaitForSeconds(3);
        grid.BuildAtCell(3, 4, wall);
        yield return new WaitForSeconds(3);
        grid.BuildAtCell(4, 3, wall);
        yield return new WaitForSeconds(3);
        grid.BuildAtCell(4, 4, wall);
        yield return new WaitForSeconds(3);
        grid.BuildAtCell(3, 5, wall);
        yield return new WaitForSeconds(3);
        grid.BuildAtCell(3, 2, wall);
        yield return new WaitForSeconds(3);
        grid.BuildAtCell(2, 3, wall);
        yield return new WaitForSeconds(3);
        grid.BuildAtCell(2, 2, wall);
        yield return new WaitForSeconds(3);
    }
}
