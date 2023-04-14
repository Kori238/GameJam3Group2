using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class pMenu : MonoBehaviour
{
    [SerializeField] private Slider AMinionSlider;
    private float AMinion;
    private float mMinion;
    private GameObject ParentStruture;
    private float currentSliderValue;
    int minionsAssigned;
    private void Start()
    {
        // AMinionSlider = GameObject.Find("MinionSlider").GetComponent<Slider>();
        AMinionSlider.onValueChanged.AddListener(delegate { ValueChanged(); });
        setSlider();
    }
    private void Update()
    {
    }

    public void SetParentStructure(GameObject thisParentStruture)
    {
        ParentStruture = thisParentStruture;
    }

    public void setSlider()
    {
         minionsAssigned = ParentStruture.GetComponent<WoodCollectorScript>().GetMinionAssigned();
        
        AMinionSlider.SetValueWithoutNotify(minionsAssigned);
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

    public void ValueChanged()
    {

        if (AMinionSlider.value>minionsAssigned)
        {
            if (ParentStruture.GetComponent<WoodCollectorScript>().SetMinionAssigned()) { minionsAssigned = (int)AMinionSlider.value; }
            else { setSlider(); }
           
        }
        else if ( ParentStruture.GetComponent<WoodCollectorScript>().SetMinionUnAssigned()){ minionsAssigned = (int)AMinionSlider.value; }
        else { setSlider(); }
        
       

        
    }
    
    
}
