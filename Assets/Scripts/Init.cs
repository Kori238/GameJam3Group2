using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    private static Init _instance;
    public static Init Instance {  get { return _instance;  } }

    public Transform tree;
    public Transform wall;
    public Grid grid;
    public ResourceManager resourceManager;
    public bool wallDemo = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        } else
        _instance = this;
        grid = new Grid(9, 9, 10f);
        resourceManager = new ResourceManager();
        if (wallDemo)
        {
            StartCoroutine(BuildWalls());
        }
        
    }
    
    private void Start()
    {
        
    }

    private IEnumerator BuildWalls()
    {
        grid.BuildAtCell(0, 0, wall);
        grid.BuildAtCell(3, 3, wall);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.BuildAtCell(3, 4, wall);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.BuildAtCell(4, 3, wall);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.BuildAtCell(4, 4, wall);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.DemolishAtCell(4, 4);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.BuildAtCell(3, 5, wall);
        yield return new WaitForSeconds(3);
        grid.DamageAtCell(3, 3, 5);
        grid.BuildAtCell(3, 2, wall);
        yield return new WaitForSeconds(3);
        grid.SetHealthAtCell(3, 3, 0, true);
        grid.BuildAtCell(2, 3, wall);
        yield return new WaitForSeconds(3);
        grid.BuildAtCell(2, 2, wall);
        grid.SetHealthAtCell(2, 2, 0);
    }
}
