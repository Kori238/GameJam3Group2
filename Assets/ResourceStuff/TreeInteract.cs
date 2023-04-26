using System.Security.Cryptography;
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
        int newWood = Random.Range(10, 30);
        Init.Instance.resourceManager.AddWood(newWood);
        GameObject instance =Instantiate(resourcePU,transform);
        resourcePopUp instanceScript = instance.GetComponent<resourcePopUp>();
        instanceScript.setImage("wood");
        
        instanceScript.setText("+" + newWood.ToString());
        
    }
    public override void CreateAttackPoints()
    {
        return;
    }
}
