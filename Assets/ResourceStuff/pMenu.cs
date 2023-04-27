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
    [SerializeField] private TextMeshProUGUI woodUpgradeCost;
    [SerializeField] private TextMeshProUGUI stoneUpgradeCost;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private TextMeshProUGUI woodRepairCost;
    [SerializeField] private TextMeshProUGUI stoneRepairCost;


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
        int temp = ParentStructureScript.getWoodUpgradeCost();
        if (temp == 0) { woodUpgradeCost.SetText(""); } else { woodUpgradeCost.text = temp.ToString() + "  <sprite=0>"; }
        temp =ParentStructureScript.getStoneUpgradeCost();
        if (temp == 0) { stoneUpgradeCost.SetText(""); } else { stoneUpgradeCost.text = temp.ToString() + "  <sprite=0>"; }
           
        description.text =ParentStructureScript.getUpgradeDescription().ToString();
         temp = ParentStructureScript.getWoodRepairCost();
        if (temp == 0) { woodRepairCost.SetText(""); } else { woodRepairCost.text = temp.ToString() + "  <sprite=0>"; }
        temp = ParentStructureScript.getStoneRepairCost();
        if (temp == 0) { stoneRepairCost.SetText(""); } else { stoneRepairCost.text = temp.ToString() + "  <sprite=0>"; }
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
        if (ParentStructureScript.upgrade())
        {
            int temp = ParentStructureScript.getWoodUpgradeCost();
            if (temp == 0) { woodUpgradeCost.SetText(""); } else { woodUpgradeCost.text = ParentStructureScript.getWoodUpgradeCost().ToString() + "  <sprite=0>"; }
            temp = ParentStructureScript.getStoneUpgradeCost();
            if (temp == 0) { stoneUpgradeCost.SetText(""); } else { stoneUpgradeCost.text = ParentStructureScript.getStoneUpgradeCost().ToString() + "  <sprite=0>"; }
            description.text = ParentStructureScript.getUpgradeDescription().ToString();
        }
        
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
