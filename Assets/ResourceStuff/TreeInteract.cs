using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class TreeInteract:  Interactable
{
    
    public override void Interact()
    {
        Init.Instance.resourceManager.AddWood(20);
    
    }
}
