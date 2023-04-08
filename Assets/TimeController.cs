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
    public SpriteRenderer m, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10;
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

        m.color = Color.white; m1.color = Color.white; m2.color = Color.white; m3.color = Color.white; m4.color = Color.white; m6.color = Color.white; m7.color = Color.white; m8.color = Color.white;
    }

    public void ActivateNight()
    {
        m.color = Color.grey; m1.color = Color.grey; m2.color = Color.grey; m3.color = Color.grey; m4.color = Color.grey; m6.color = Color.grey; m7.color = Color.grey; m8.color = Color.grey;
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