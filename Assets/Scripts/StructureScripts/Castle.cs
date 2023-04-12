using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Structure
{



    public override void Damaged(float amount)
    {

        // Put your TEXTMESHPRO thing here the health is stored in health
        base.Damaged(amount);
    }
}
