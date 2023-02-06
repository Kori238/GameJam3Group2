using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metalOreInteract : Interactable

{
    [SerializeField] ResourceManager resourceManager;
    // Start is called before the first frame update
    public override void Interact()
    {
        resourceManager.AddMetal(10);
    }

}
