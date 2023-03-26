using System.Collections;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{

    public float minutesTime;
    public float hourTime = 12;
    public int day = 0;
    public GameObject clock;
    public TMP_Text timer;
    float clockRotation = 0;
    public int timeMultiplyer = 10;
    public SpriteRenderer map;
    private bool canSpawn = true;

    public GameObject enemy;
    public Vector3 loc;
    private bool isNight = false;

    // Start is called before the first frame update
    private void Start()
    {
    }

    public void ActivateDay()
    {

        map.color = Color.white;
    }

    public void ActivateNight()
    {
        map.color = Color.grey;
        if (canSpawn == true)
        {
            StartCoroutine(SpawnEnemy());
        }

    }

    private IEnumerator SpawnEnemy()
    {
        canSpawn = false;
        GameObject enemy1 = (GameObject)Instantiate(enemy, loc, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        canSpawn = true;
    }

    // Update is called once per frame
    private void Update()
    {
        minutesTime += Time.deltaTime * timeMultiplyer * 4;
        clockRotation += Time.deltaTime * timeMultiplyer;

        if (minutesTime <= 9)
        {
            timer.text = hourTime + ": 0" + (int)minutesTime + " | Day: " + day;
        }
        else
        {
            timer.text = hourTime + ": " + (int)minutesTime + " | Day: " + day;
        }

        var rotateClock = new Vector3(0, 0, (float)(clockRotation));
        clock.transform.eulerAngles = rotateClock;
        if (minutesTime >= 60)
        {
            hourTime++;
            minutesTime = 0;
        }

        if (hourTime >= 24)
        {
            day++;
            hourTime = 0;
        }
        else if (hourTime == 6)
        {
            isNight = false;
            ActivateDay();
        }
        else if (hourTime == 17)
        {
            isNight = true;
        }
        else if (isNight  == true)
        {
            ActivateNight();
        }
    }

    

}