using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class pMenu : MonoBehaviour
{
    private GameObject ParentStruture;
    [SerializeField] Slider AMinionSlider;
    private float AMinion;
    private float mMinion;
    
    public void SetParentStructure(GameObject thisParentStruture)
    {
        ParentStruture = thisParentStruture;
    }

    public void setSlider(int newAMinion, int newMMinion)
    {
        AMinion= newAMinion;
        mMinion= newMMinion;
        AMinionSlider.SetValueWithoutNotify(3);
    }

    public void onUpgradeButtonClick()
    {
        ParentStruture.GetComponent<WoodCollectorScript>().upgrade();
    }
    public void onRepairButtonClick()
    {
        ParentStruture.GetComponent<WoodCollectorScript>().repair();
    }
    public float GetMinionSlider()
    {
        return AMinion;
    }
    private void Start()
    {
        //  AMinionSlider = GameObject.Find("MinionSlider").GetComponent<Slider>();
        AMinionSlider.onValueChanged.AddListener(delegate { ValueChanged(); });
        
    }
    private void Update()
    {
   
        

    }
    
    public void ValueChanged() 
    {
        ParentStruture.GetComponent<WoodCollectorScript>().SetMinionAssigned((int)AMinionSlider.value);
    }
        
    }


