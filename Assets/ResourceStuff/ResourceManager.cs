using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ResourceManager 
{
    private int stone = 50;
    private int wood;
    private int starlight;
    public TMP_Text WoodUI;


    public int GetStone()
    {
        return stone;
    }
    public int GetWood()
    {
        
        return wood;
    }
    public int GetStarlight()
    {
        return starlight;
    }

    public void AddStone(int newmetal)
    {
        stone = newmetal + stone;
    }
    
    public void AddWood(int newwood)
    {
        wood = wood + newwood;
        WoodUI.text = wood.ToString();
    }
   
    public void AddStarLight(int newStarLight)
    {
        starlight = starlight + newStarLight;
    }
  
}
