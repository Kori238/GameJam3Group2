using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneInteract : Interactable

{
    
    public override void Interact()
    {
        GridInit.Instance.resourceManager.AddStone(10);
    }

}
