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
    public float timeScale = 1f;
    public int timeMultiplyer = 10;
    public SpriteRenderer[] maptiles = new SpriteRenderer[25];

    public bool canSpawn = true;

    public List<GameObject> enemies;
    public Vector3 loc;
    public Transform playerLocation;
    public bool isNight = false;
    private int x;
    private int y;

    private float spawnDelay = 1.5f;
    public int enemiesPerRound = 10;
    public int enemiesSpawned = 0;
    public TMP_Text enemiesText;

    int updateCounter = 0;

    // Start is called before the first frame update
    private void Start()
    {
        timer.text = "Day: " + day;
        enemiesText.text = "Nooms: \n" + enemiesSpawned;
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

        for (int i = 0; i < enemiesPerRound; i++)
        {
            int r = Random.Range(0, 3);

            if (r == 0)
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


            spawnEnemy();
        }

    }

    public void EnemyKilled()
    {
        enemiesSpawned--;
        enemiesText.text = "Nooms: \n" + enemiesSpawned;
    }


    private void spawnEnemy()
    {
        int toSpawn = Random.Range(0, enemies.Count);
        GameObject enemy1 = (GameObject)Instantiate(enemies[toSpawn], loc, Quaternion.identity);
        enemiesSpawned++;
        enemiesText.text = "Nooms: \n" + enemiesSpawned;
    }

    public void ForceSpawnEnemy()
    {
        loc.Set(playerLocation.position.x, playerLocation.position.y, playerLocation.position.z);
        int toSpawn = Random.Range(0, enemies.Count);
        GameObject enemy1 = (GameObject)Instantiate(enemies[toSpawn], loc, Quaternion.identity);
    }
    // Update is called once per frame
    private void Update()
    {
        Time.timeScale = timeScale;

        minutesTime += Time.deltaTime * timeMultiplyer * 4;
        clockRotation += Time.deltaTime * timeMultiplyer;

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
            if(spawnDelay > 0.3f)
            {
                spawnDelay -= 0.1f;
            }
            enemiesPerRound += 5;

        }
        else if (hourTime == 6)
        {
            isNight = false;
            updateCounter = 0;
            ActivateDay();
        }
        else if (hourTime == 17)
        {
            if (updateCounter == 0)
            {
                updateCounter++;
                isNight = true;
            }
            canSpawn = true;
        }
        else if (isNight  == true)
        {
            isNight = false;
            ActivateNight();
        }
    }

    

}