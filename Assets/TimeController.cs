using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float secoundsTime;
    public float minutesTime = 0;
    public float hourTime = 6;
    public GameObject clock;
    public TMP_Text timer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        minutesTime += Time.deltaTime * 2;
        timer.text = hourTime + ":" + (int)minutesTime;
        Vector3 rotateClock = new Vector3(0, 0, minutesTime);
        clock.transform.eulerAngles = rotateClock;
        if (minutesTime >= 60)
        {
            hourTime++;
            minutesTime = 0;
        }
        
    }
}
