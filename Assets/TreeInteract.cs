using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class TreeInteract:  Interactable
{
    [SerializeField] ResourceManager resourceManager;
    public override void Interact()
{
    resourceManager.AddWood(20);
    
 }
}
