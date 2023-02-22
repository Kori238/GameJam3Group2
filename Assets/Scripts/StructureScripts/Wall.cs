using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wall : Structure
{

    public override void Destroyed()
    {
        base.Destroyed();
        gameObject.GetComponent<WallConnectors>().updateAllConnectors();
    }

    public override void Demolished()
    {
        gameObject.GetComponent<WallConnectors>().updateAllConnectors();
        base.Demolished();
    }

}
