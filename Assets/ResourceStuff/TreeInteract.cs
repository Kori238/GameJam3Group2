using UnityEngine;

public class TreeInteract : Interactable
{
    public override void FindOccupiedSpace()
    {
        Vector2 nodePos = gridPos * 3;
        NodeGrid nodeGrid = Init.Instance.pathfinding.GetGrid();
        for (int y = (int)nodePos.x; y < (int)nodePos.y + 3; y++)
        {
            occupiedSpace.Add(nodeGrid.gridArray[(int)nodePos.x + 1, y]);
        }
    }

    public override void Interact()
    {
        Init.Instance.resourceManager.AddWood(20);

    }
    public override void CreateAttackPoints()
    {
        return;
    }
}
