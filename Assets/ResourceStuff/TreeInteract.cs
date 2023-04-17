using UnityEngine;

public class TreeInteract : Interactable
{
   [SerializeField] private GameObject resourcePU;
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
        Init.Instance.resourceManager.AddWood(20);
        GameObject instance =Instantiate(resourcePU,transform);
        resourcePopUp instanceScript = instance.GetComponent<resourcePopUp>();
        instanceScript.setImage("wood");
        instanceScript.setText("+20");
        
    }
    public override void CreateAttackPoints()
    {
        return;
    }
}
