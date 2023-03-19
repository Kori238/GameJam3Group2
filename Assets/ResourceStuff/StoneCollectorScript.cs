using UnityEngine;

public class StoneCollectorScript : Interactable
{

    int CurrentWoodCoolDown = 0;
    int MaxWoodCoolDown = 500;

    // Update is called once per frame
    void Update()
    {
        if (CurrentWoodCoolDown >= MaxWoodCoolDown)
        {
            Init.Instance.resourceManager.AddStone(50);


            print("Stone Collector added 50 stone");
            CurrentWoodCoolDown = 0;
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        CurrentWoodCoolDown += 1;

    }
    public override void Interact()
    {
        if (Init.Instance.resourceManager.GetStone() >= 50)
        {
            health = maxHealth;
            Debug.Log("building repaired");
        }
        else
        {
            Debug.Log("failed to repair");
        }

    }
}
