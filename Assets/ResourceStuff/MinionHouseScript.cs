using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinionHouseScript : Interactable
{
    [SerializeField] private Transform Minion;
    [SerializeField] private List<Transform> housedMinions;
    private Vector3 Location;
    
    // Start is called before the first frame update
    private  new void Start()
    {
        
        Init.Instance.resourceManager.SetMaxMinion(5);
        Location = transform.position;
        Transform newMinion = Instantiate(Minion, Location , Quaternion.identity);
        housedMinions.Add(newMinion);
        
        Debug.Log(housedMinions);
        //Init.Instance.resourceManager.availableMinions.Add(newMinion);
        Init.Instance.resourceManager.AddToMinionList(housedMinions);
       

        
    }

    // Update is called once per frame
    private void Update()
    {
    }
    public override void Interact()
    {
        
    }
}
