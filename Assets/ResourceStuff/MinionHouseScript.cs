using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinionHouseScript : Interactable
{
    [SerializeField] private GameObject Minion;
    private List<GameObject> housedMinions;
    // Start is called before the first frame update
    private  new void Start()
    {
        housedMinions = new List<GameObject>();
        Init.Instance.resourceManager.SetMaxMinion(5);
        GameObject newMinion = Instantiate(Minion);
        housedMinions.Add(newMinion);
        Init.Instance.resourceManager.AddToMinionList(newMinion);
        
    }

    // Update is called once per frame
    private void Update()
    {
    }
    public override void Interact()
    {
        
    }
}
