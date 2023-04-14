using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private int maxMinions;
    private int starlight;
    private int stone = 50;
    private int unassignedMinions;
    private int wood;
    private List<Transform> availableMinions = new List<Transform>();

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