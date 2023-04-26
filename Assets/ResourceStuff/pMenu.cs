using System.Collections;
using TMPro;
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
    WoodCollectorScript ParentStructureScript;

    [SerializeField] TextMeshProUGUI repairText;

    private void Start()
    {
        // AMinionSlider = GameObject.Find("MinionSlider").GetComponent<Slider>();
        AMinionSlider.onValueChanged.AddListener(delegate { ValueChanged(); });
        setSlider();
    }
    private void Update()
    {
    }
    private void setMenucosts()
    {
       
      repairText.SetText(ParentStructureScript.getRepairCost().ToString());
    }

    public void SetParentStructure(GameObject thisParentStruture)
    {
        ParentStruture = thisParentStruture;
        ParentStructureScript= ParentStruture.GetComponent<WoodCollectorScript>();
        setMenucosts();
    }

    public void setSlider()
    {
         minionsAssigned = ParentStructureScript.GetMinionAssigned();
        
        AMinionSlider.SetValueWithoutNotify(minionsAssigned);
    }

    public void onUpgradeButtonClick()
    {
        ParentStructureScript.upgrade();
    }
    public void onRepairButtonClick()
    {
        ParentStructureScript.repair();
    }
    public float GetMinionSlider()
    {
        return AMinion;
    }
    public void onCloseButton()
    {
        Destroy(gameObject);
    }
    

    public void ValueChanged()
    {
        AMinionSlider.interactable =false;
        StartCoroutine(disableTime());
        if (AMinionSlider.value>minionsAssigned)
        {
            if (ParentStructureScript.SetMinionAssigned()) { minionsAssigned = (int)AMinionSlider.value; }
            else { setSlider(); }
           
        }
        else if (ParentStructureScript.SetMinionUnAssigned()){ minionsAssigned = (int)AMinionSlider.value; }
        else { setSlider(); }
        
       

        
    }
    private IEnumerator disableTime()
    {
        yield return new WaitForSeconds(0.5f);
        AMinionSlider.interactable = true;
    }


}
