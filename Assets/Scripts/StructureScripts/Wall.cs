using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wall : Structure
{

    public override void Destroyed() //Overrides Structure method in order to update the connectors sprites
    {
        base.Destroyed();
        gameObject.GetComponent<WallConnectors>().updateAllConnectors();
    }

    public override void Demolished() //Overrides Structure method in order to deconnect adjacent walls when destroyed
    {
        gameObject.GetComponent<WallConnectors>().updateAllConnectors();
        base.Demolished();
    }

}
