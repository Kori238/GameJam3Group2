using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Castle : Structure
{

    public TMP_Text bh;

    public override void Start()
    {
        bh = GameObject.Find("BaseHealth").GetComponent<TMP_Text>();
        base.Start();
    }
    public override void Damaged(float amount)
    {
        base.Damaged(amount);
        bh.GetComponent<TMP_Text>().SetText((health).ToString());
    }
}
