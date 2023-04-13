using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionScript : MonoBehaviour
{
    private bool occupation = false;
    [SerializeField] private Structure jobLocation;
    [SerializeField] private Structure House;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool setoccupation(bool newOccupation)
    {
        occupation= newOccupation;


        return true;
    }
    public bool setJobLocation(Structure newJobLocation)
    {
        jobLocation = newJobLocation;
        Debug.Log(jobLocation.ToString());
        Debug.Log("Setting job location");
        return true;
    }

    public bool setHouse(Structure newHouse) 
    {
        House= newHouse;
        return true;
    }
}
