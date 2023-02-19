using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class TreeInteract:  Interactable
{
    
    public override void Interact()
    {
        GridInit.Instance.resourceManager.AddWood(20);
    
    }
}
