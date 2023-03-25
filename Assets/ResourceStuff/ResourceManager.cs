using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private int maxMinions;
    private int starlight;
    private int stone = 50;
    private int unassignedMinions;
    private int wood;
    private List<GameObject> housedMinions;

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

    public void AddToMinionList(GameObject newMinion)
    {
        housedMinions.Add(newMinion);
    }
}