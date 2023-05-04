using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Castle : Structure
{
    [SerializeField] private static Slider healthSlider;

    public void Awake()
    {
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        //base.Start();
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

    public override void SetHealth(float amount, bool fullyHeal = false)
    {
        base.SetHealth(health + amount);
        healthSlider.GetComponent<Slider>().value = health;
    }
}
