using UnityEngine;

public class S_Pl_Combat : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Ability1"))
        {
            Debug.Log("MainAttack");
        }
        else if (Input.GetButtonDown("Ability2"))
        {
            Debug.Log("SideAttack");
        }
    }
}
