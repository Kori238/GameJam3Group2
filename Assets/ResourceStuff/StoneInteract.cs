using System.Collections.Generic;
using UnityEngine;

public class StoneInteract : Interactable

{
    [SerializeField] private GameObject resourcePU;
    [SerializeField] private List<WoodCollectorScript> collectors;



    public void addCollectors(WoodCollectorScript newcollector) { collectors.Add(newcollector); }

    public override void FindOccupiedSpace()
    {
        var nodePos = gridPos * 3;
        var nodeGrid = Init.Instance.pathfinding.GetGrid();
        for (var y = (int)nodePos.x; y < (int)nodePos.y + 3; y++)
        {
            occupiedSpace.Add(nodeGrid.gridArray[(int)nodePos.x + 1, y]);
        }
    }
    public override void Interact()
    {
        int newstone= Random.Range(10, 30);
        Init.Instance.resourceManager.AddStone(newstone);
        
        GameObject instance = Instantiate(resourcePU, transform);
        resourcePopUp instanceScript = instance.GetComponent<resourcePopUp>();
        instanceScript.setImage("stone");

        
        instanceScript.setText("+"+newstone.ToString());


    }
    public override void CreateAttackPoints()
    {
    }



    public override void Demolished()
    {

        foreach (var Collector in collectors) { Collector.unssignTree(gameObject); }



        base.Demolished();
    }
}
