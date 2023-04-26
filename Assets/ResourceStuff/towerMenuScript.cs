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


    

    private Tower ParentStruture;
    
   

    [SerializeField] TextMeshProUGUI repairText;





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
        ParentStruture.setTargetPriority((int)targetSlider.value);
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
        int temp = ParentStruture.getWoodUpgradecost();
        if(temp== 0) { woodUpgradeCost.SetText(""); } else { woodUpgradeCost.SetText(temp.ToString() + "  <sprite=0>"); }
        temp=ParentStruture.getStoneUpgradecost();
        if(temp== 0) { stoneUpgradeCost.SetText(""); } else { stoneUpgradeCost.SetText(temp.ToString() + "  <sprite=0>"); }
      
        description.SetText("Upgrade to the next level");
        setSlider(ParentStruture.getTargetPriority());
    }
    public void SetParentStructure(Tower thisParentStruture)
    {
        ParentStruture = thisParentStruture;
       
        setMenucosts();
        
    }
    public void onUpgradeButtonClick()
    {
        ParentStruture.upgradeTower();
    }
    public void onRepairButtonClick()
    {
       // ParentStructureScript.repair();
    }
    public void onCloseButton()
    {
        Destroy(gameObject);
    }
}
