using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class resourceUIScript : MonoBehaviour
{
    [SerializeField] private TMP_Text wood;
    [SerializeField] private TMP_Text stone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void setStone(int newstone)
    {
        stone.text=newstone.ToString();
    }
    public void setWood(int newwood) { wood.text = newwood.ToString(); }
}
