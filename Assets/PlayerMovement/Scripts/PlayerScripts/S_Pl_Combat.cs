using UnityEngine;

public class S_Pl_Combat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
