using UnityEngine;

public class MinionHouseScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Init.Instance.resourceManager.SetMaxMinion(5);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
