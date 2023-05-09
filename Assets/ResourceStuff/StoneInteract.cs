using System.Collections.Generic;
using UnityEngine;

public class StoneInteract : Interactable

{
    [SerializeField] private GameObject resourcePU;
    [SerializeField] private List<WoodCollectorScript> collectors;


    public override void FindCollectionPoints()
    {
        if (destroyed) return;
        var nodePos = gridPos * 3;
        var nodeGrid = Init.Instance.pathfinding.GetGrid();
        var collectionPositions = new List<Vector2>
        {
            new Vector2(nodePos.x + 1, nodePos.y),
        };

        foreach (Vector2 collectionPosition in collectionPositions)
        {
            if (collectionPosition.x >= 149 || collectionPosition.x < 0 || collectionPosition.y >= 149 || collectionPosition.y < 0) continue;
            var node = nodeGrid.gridArray[(int)collectionPosition.x, (int)collectionPosition.y];
            collectionPoints.Add(node);
        }
    }
    public void addCollectors(WoodCollectorScript newcollector) { collectors.Add(newcollector); }



    public override void FindOccupiedSpace()
    {
        var nodePos = gridPos * 3;
        var nodeGrid = Init.Instance.pathfinding.GetGrid();
        for (var x = (int)nodePos.x; x < (int)nodePos.x + 3; x++)
        {
            occupiedSpace.Add(nodeGrid.gridArray[x, (int)nodePos.y + 1]);
        }
        occupiedSpace.Add(nodeGrid.gridArray[(int)nodePos.x + 1, (int)nodePos.y + 2]);
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
        return;
    }



    public override void Demolished()
    {

        foreach (var Collector in collectors) { Collector.unssignTree(gameObject); }



        base.Demolished();
    }
}
