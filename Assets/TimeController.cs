using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float secoundsTime;
    public float minutesTime;
    public float hourTime = 12;
    public GameObject clock;
    public TMP_Text timer;
    float clockRotation = 0;
    public int timeMultiplyer = 10;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        minutesTime += Time.deltaTime * timeMultiplyer * 4;
        clockRotation += Time.deltaTime * timeMultiplyer;

        if(minutesTime <= 9)
        {
            timer.text = hourTime + ": 0" + (int)minutesTime;
        }
        else
        {
            timer.text = hourTime + ": " + (int)minutesTime;
        }
        
        var rotateClock = new Vector3(0, 0, (float)(clockRotation));
        clock.transform.eulerAngles = rotateClock;
        if (minutesTime >= 60)
        {
            hourTime++;
            minutesTime = 0;
        }
    }
}
