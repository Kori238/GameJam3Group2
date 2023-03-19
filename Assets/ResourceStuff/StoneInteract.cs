public class StoneInteract : Interactable

{

    public override void Interact()
    {
        Init.Instance.resourceManager.AddStone(10);
    }
    public override void CreateAttackPoints()
    {
        return;
    }
}
