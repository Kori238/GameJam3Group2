using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneInteract : Interactable

{
    [SerializeField] ResourceManager resourceManager;
    // Start is called before the first frame update
    public override void Interact()
    {
        GridInit.Instance.resourceManager.AddStone(10);
    }

}
