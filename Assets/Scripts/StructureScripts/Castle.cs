using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Castle : Structure
{
    public Slider healthSlider;

    public override void Start()
    {
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        base.Start();
    }

    //public override void CreateAttackPoints()
    //{
    //    return;
    //}
    public override void Damaged(float amount)
    {
        base.Damaged(amount);
        healthSlider.GetComponent<Slider>().value = health;
    }

    public void gainHealth(float amount)
    {
        Healed(health + amount);
        healthSlider.GetComponent<Slider>().value = health;
    }
}
