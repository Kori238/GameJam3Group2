using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Drawing.Printing;

public class ResourceManager
{
    private int maxMinions;
    private int starlight;
    private int stone;
    private int unassignedMinions;
    private int wood;
    private List<Transform> availableMinions = new List<Transform>();
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

    public void AddToMinionsList(List<Transform> MinionToAdd)

    {
        int i = MinionToAdd.Count;
         
        for(int j=0 ; j <i; j++)
        {

            
            Transform temp= MinionToAdd[j];
            availableMinions.Add(temp);
            Debug.Log( " minionList");
            
            
        }
        Debug.Log(availableMinions[0]);
    }
    public void addSingleMinionToList(Transform minionToAdd)
    {
        availableMinions.Add(minionToAdd);
    }

    public Transform GetMinionList()
    {


        
        Transform temp = availableMinions[availableMinions.Count - 1];
        availableMinions.RemoveAt(availableMinions.Count - 1);
        return temp;
    }
    public int getAvailableMinionLength()
    {
        return availableMinions.Count;
    }
}