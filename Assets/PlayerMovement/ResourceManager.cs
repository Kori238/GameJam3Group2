using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private int metal;
    private int wood;
    private int starlight;




    // Start is called before the first frame update
    void Awake()
    {
        metal = 0;
        wood = 0;
        starlight = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public int GetMetal()
    {
        return metal;
    }
    public int GetWood()
    {
        return wood;
    }
    public int GetStarlight()
    {
        return starlight;
    }

    public void AddMetal(int newmetal)
    {
        metal = newmetal + metal;
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
