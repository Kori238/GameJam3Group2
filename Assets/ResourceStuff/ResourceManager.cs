public class ResourceManager
{
    private int stone = 50;
    private int wood;
    private int starlight;
    private int unassignedMinions;
    private int maxMinions;


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
}
