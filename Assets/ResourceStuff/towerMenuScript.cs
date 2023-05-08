using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class towerMenuScript : MonoBehaviour
{

    [SerializeField] private Slider targetSlider;
    [SerializeField] private TextMeshProUGUI targetText;

    [SerializeField] private TextMeshProUGUI woodUpgradeCost;
    [SerializeField] private TextMeshProUGUI stoneUpgradeCost;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI buildingLevelText;


    

    private Tower ParentStructure;
    
   

    [SerializeField] TextMeshProUGUI woodRepairCost;
    [SerializeField] TextMeshProUGUI stoneRepairCost;





    // Start is called before the first frame update
    void Start()
    {
        targetSlider.onValueChanged.AddListener(delegate { ValueChanged(); });
         

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ValueChanged()
    {
        ParentStructure.setTargetPriority((int)targetSlider.value);
        switch (targetSlider.value)
        {
            case 0: { targetText.SetText("Target: Closest"); break; }
            case 1: { targetText.SetText("Target: Furthest"); break; }
            case 2: { targetText.SetText("Target: Strongest"); break; }
            case 3: { targetText.SetText("Target: Weakest"); break; }
        }
        
    }
    private void setSlider(int newvalue) 
    {
        targetSlider.value = newvalue;
        switch (targetSlider.value)
        {
            case 0: { targetText.SetText("Target: Closest"); break; }
            case 1: { targetText.SetText("Target: Furthest"); break; }
            case 2: { targetText.SetText("Target: Strongest"); break; }
            case 3: { targetText.SetText("Target: Weakest"); break; }
        }
    }


    private void setMenucosts()
    {

        // repairText.SetText(ParentStructureScript.getRepairCost().ToString());
        int temp = ParentStructure.getWoodUpgradecost();
        if(temp== 0) { woodUpgradeCost.SetText(""); } else { woodUpgradeCost.SetText(temp.ToString() + "  <sprite=0>"); }
        temp=ParentStructure.getStoneUpgradecost();
        if(temp== 0) { stoneUpgradeCost.SetText(""); } else { stoneUpgradeCost.SetText(temp.ToString() + "  <sprite=0>"); }
        temp = ParentStructure.getWoodRepairCost();
        if(temp== 0) { woodRepairCost.SetText(""); } else { woodRepairCost.SetText(temp.ToString() + "  <sprite=0>"); }
        temp = ParentStructure.getStoneRepairCost();
        if (temp == 0) { stoneRepairCost.SetText(""); } else { stoneRepairCost.SetText(temp.ToString() + "  <sprite=0>"); }
        description.SetText(ParentStructure.getUpgradeDescription());
        buildingLevelText.SetText("Level "+ParentStructure.getBuildingLevel().ToString());
        setSlider(ParentStructure.getTargetPriority());
    }
    public void SetParentStructure(Tower thisParentStruture)
    {
        ParentStructure = thisParentStruture;
       
        setMenucosts();
        
    }
    public void onUpgradeButtonClick()
    {
        if (ParentStructure.upgradeTower())
        {
            int temp = ParentStructure.getWoodUpgradecost();
            if (temp == 0) { woodUpgradeCost.SetText(""); } else { woodUpgradeCost.SetText(temp.ToString() + "  <sprite=0>"); }
            temp = ParentStructure.getStoneUpgradecost();
            if (temp == 0) { stoneUpgradeCost.SetText(""); } else { stoneUpgradeCost.SetText(temp.ToString() + "  <sprite=0>"); }
            temp = ParentStructure.getWoodRepairCost();
            if (temp == 0) { woodRepairCost.SetText(""); } else { woodRepairCost.SetText(temp.ToString() + "  <sprite=0>"); }
            temp = ParentStructure.getStoneRepairCost();
            if (temp == 0) { stoneRepairCost.SetText(""); } else { stoneRepairCost.SetText(temp.ToString() + "  <sprite=0>"); }
            description.SetText(ParentStructure.getUpgradeDescription());
            buildingLevelText.SetText("Level " + ParentStructure.getBuildingLevel().ToString());
        }
    }
    public void onRepairButtonClick()
    {
        if (ParentStructure.repair()) { }
    }
    public void onCloseButton()
    {
        ParentStructure.toOpen = true;
        Destroy(gameObject);
    }
}
