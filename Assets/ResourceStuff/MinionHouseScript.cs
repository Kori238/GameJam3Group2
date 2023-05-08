using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinionHouseScript : Interactable
{
    [SerializeField] private Transform Minion;
    [SerializeField] private List<Transform> housedMinions;
    private Vector3 Location;
    
    // Start is called before the first frame update
    public override void Start()
    {
        Init.Instance.resourceManager.SetMaxMinion(5);
        Location = transform.position;
        for(int x=0;x<5;x++) {
            Transform newMinion = Instantiate(Minion, Location, Quaternion.identity);
            newMinion.GetComponent<MinionScript>().setHouse(this);
            housedMinions.Add(newMinion);
           

        }
      
        base.Start();
        
    }

    public override void FindCollectionPoints()
    {
        if (destroyed) return;
        var nodePos = gridPos * 3;
        var nodeGrid = Init.Instance.pathfinding.GetGrid();
        var node = nodeGrid.gridArray[(int)nodePos.x + 1, (int)nodePos.y + 1]; 
        collectionPoints.Add(node);
        }



    /*public override void OccupySpace()
    {
        return;
    }*/

    // Update is called once per frame
    private void Update()
    {
    }
    public override void Interact()
    {
        
    }
    public override void Demolished()
    {

        foreach (var Minion in housedMinions) { Minion.GetComponent<MinionScript>().deHouse(); }


        base.Demolished();
    }
}
