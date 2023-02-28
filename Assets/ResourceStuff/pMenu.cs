using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class pMenu : MonoBehaviour
{
  private GameObject ParentStruture;

    public void SetParentStructure(GameObject thisParentStruture)
    {
        ParentStruture = thisParentStruture;
    }
    public void onUpgradeButtonClick()
    {
        ParentStruture.GetComponent<WoodCollectorScript>().upgrade();
    }
    public void onRepairButtonClick()
    {
        ParentStruture.GetComponent<WoodCollectorScript>().repair();
    }
    public void GetMinionSlider()
    {
        print(gameObject.GetComponent<Slider>());
    }
  
}
