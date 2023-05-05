using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaybyDayTips : MonoBehaviour
{

    public TimeController tC;
    public GameObject day1; public GameObject day2; public GameObject day3;
    private bool hasDisplayed = false;

    void Start()
    {
        tC.GetComponent<TimeController>();
    }

    private int day = 0;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("isnight: " + tC.isNight); 
        Debug.Log("has displayed: "+hasDisplayed);
        if(tC.isNight == true && hasDisplayed == false)
        {
            hasDisplayed = true;
            Time.timeScale = 0f;
            if (day == 0)
            {
                day1.SetActive(true);
            }
            else if (day == 1)
            {
                day2.SetActive(true);
            }
            else if (day == 2)
            {
                day3.SetActive(true);
            }
        }
        
    }

    public void Continue()
    {
        if (tC.day == 0)
        {
            day1.SetActive(false);
        }
        else if (tC.day == 1)
        {
            day2.SetActive(false);
        }
        else if (tC.day == 2)
        {
            day3.SetActive(false);
        }
        Time.timeScale = 1f;
        day++;
        hasDisplayed = false;
        gameObject.SetActive(false);
    }
}
