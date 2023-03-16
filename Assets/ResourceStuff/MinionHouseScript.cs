using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHouseScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init.Instance.resourceManager.SetMaxMinion(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
