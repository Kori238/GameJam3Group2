using System.Collections;
using System.Collections.Generic;
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
    public SpriteRenderer[] maptiles = new SpriteRenderer[25];
    private bool canSpawn = true;

    public List<GameObject> enemies;
    public Vector3 loc;
    public bool isNight = false;
    private int x;
    private int y;


    // Start is called before the first frame update
    private void Start()
    {
        timer.text = "Day: " + day;
    }

    public void ActivateDay()
    {
        for (int i = 0; i < maptiles.Length; i++)
        {
            maptiles[i].color = Color.white;
        }

    }

    public void ActivateNight()
    {
        for (int i = 0; i < maptiles.Length; i++)
        {
            maptiles[i].color = Color.gray;
        }

        if (canSpawn == true)
        {
            StartCoroutine(SpawnEnemy());
        }

    }

    private IEnumerator SpawnEnemy()
    {
        canSpawn = false;
        int r = Random.Range(0, 3);

        if(r == 0)
        {
            y = 475;
            x = Random.Range(25, 475);
        } 
        else if (r == 1)
        {
            y = 25;
            x = Random.Range(25, 475);
        }
        else if (r == 2)
        {
            x = 25;
            y = Random.Range(25, 475);
        }
        else if (r == 3)
        {
            x = 475;
            y = Random.Range(25, 475);
        }


        loc.Set(x, y, -1);


        int toSpawn = Random.Range(0, enemies.Count) ;
        //GameObject enemy1 = (GameObject)Instantiate(enemies[toSpawn], loc, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        canSpawn = true;
    }

    // Update is called once per frame
    private void Update()
    {
        minutesTime += Time.deltaTime * timeMultiplyer * 4;
        clockRotation += Time.deltaTime * timeMultiplyer;

        

        //if (minutesTime <= 9)
        //{
        //    timer.text = hourTime + ": 0" + (int)minutesTime + " | Day: " + day;
        //}
        //else
        //{
        //    timer.text = hourTime + ": " + (int)minutesTime + " | Day: " + day;
        //}

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
            timer.text = "Day: " + day;
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