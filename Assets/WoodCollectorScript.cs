using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCollectorScript : MonoBehaviour
{
    [SerializeField] ResourceManager resourceManager; 



    int CurrentWoodCoolDown = 0;
    int MaxWoodCoolDown = 1000;








    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentWoodCoolDown >= MaxWoodCoolDown)
        {
            resourceManager.AddWood(50);
            print("Wood Collector added 50 wood");
            CurrentWoodCoolDown = 0;
        }
    }
    private void FixedUpdate()
    {
        CurrentWoodCoolDown +=  1;

    }
}
