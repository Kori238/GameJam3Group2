using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Drawing.Printing;

public class ResourceManager
{
    private int maxMinions;
    private int starlight;
    private int stone=250;
    private int unassignedMinions;
    private int wood=500;
    public List<MinionScript> availableMinions = new List<MinionScript>();
    public resourceUIScript resourceUI;
   
  
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
        resourceUI.setStone(stone);
    }

    public void AddWood(int newwood)
    {
        wood = wood + newwood;
        resourceUI.setWood(wood);
        
    }

    public void AddStarLight(int newStarLight)
    {
        starlight = starlight + newStarLight;
    }
    public void SetMaxMinion(int newMinion)
    {
        maxMinions = maxMinions + newMinion;
        assignMinion(newMinion);
    }
    public int GetMaxMinion()
    {
        return maxMinions;
    }
    public void assignMinion(int newMinionToAssign)
    {
        unassignedMinions = unassignedMinions + newMinionToAssign;
    }

  



    public void addSingleMinionToList(MinionScript minionToAdd)
    {
        availableMinions.Add(minionToAdd);
    }

    public Transform GetMinionList()
    {

    
        
        Transform temp = availableMinions[availableMinions.Count - 1].transform;
        availableMinions.RemoveAt(availableMinions.Count - 1);
        return temp;
    }
    public int getAvailableMinionLength()
    {
        return availableMinions.Count;
    }
    public void removeFromAvailibleMinionList(MinionScript tempMinion)
    {
        availableMinions.Remove(tempMinion);
    }
}