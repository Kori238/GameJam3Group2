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
    public override void Damaged(float amount)
    {
        base.Damaged(amount);
        healthSlider.GetComponent<Slider>().value = health;
    }

    public void gainHealth(float amount)
    {
        base.SetHealth(health + amount, false);
        healthSlider.GetComponent<Slider>().value = health;
    }
}
