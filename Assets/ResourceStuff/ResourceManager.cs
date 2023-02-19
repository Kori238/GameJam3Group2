using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ResourceManager 
{
    private int stone;
    private int wood;
    private int starlight;




    // Start is called before the first frame update
    void Awake()
    {
        stone = 0;
        wood = 0;
        starlight = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
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
    }
   
    public void AddStarLight(int newStarLight)
    {
        starlight = starlight + newStarLight;
    }
  
}
