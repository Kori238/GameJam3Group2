using UnityEngine;
using UnityEngine.UI;

public class pMenu : MonoBehaviour
{
    [SerializeField] private Slider AMinionSlider;
    private float AMinion;
    private float mMinion;
    private GameObject ParentStruture;
    private void Start()
    {
        //  AMinionSlider = GameObject.Find("MinionSlider").GetComponent<Slider>();
        AMinionSlider.onValueChanged.AddListener(delegate { ValueChanged(); });
    }
    private void Update()
    {
    }

    public void SetParentStructure(GameObject thisParentStruture)
    {
        ParentStruture = thisParentStruture;
    }

    public void setSlider(int newAMinion, int newMMinion)
    {
        AMinion = newAMinion;
        mMinion = newMMinion;
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

    public void ValueChanged()
    {
        ParentStruture.GetComponent<WoodCollectorScript>().SetMinionAssigned((int)AMinionSlider.value);
    }
}
