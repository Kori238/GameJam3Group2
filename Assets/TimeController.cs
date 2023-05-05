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
    public int clockSpeed = 10;
    public SpriteRenderer[] maptiles = new SpriteRenderer[25];
    public AudioSource dayMusic;
    public AudioSource nightMusic;
    public bool canSpawn = true;

    public List<GameObject> enemies;
    public Vector3 loc;
    public Transform playerLocation;
    public bool isNight = false;
    private int x;
    private int y;

    private float spawnDelay = 1.5f;
    public int enemiesPerRound = 5;
    public int enemiesSpawned = 0;
    public TMP_Text enemiesText;

    bool canRegenBase = true;
    int updateCounter = 0;

    public int passiveBaseRegen = 15;
    public GameObject CastleBuilding;


    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = timeScale;
        timer.text = "Day: " + day;
        enemiesText.text = "Nooms: \n" + enemiesSpawned;
        //StartCoroutine(RegenBase());
    }

    public void ActivateDay()
    {
        for (int i = 0; i < maptiles.Length; i++)
        {
            maptiles[i].color = Color.white;
            
        }
        dayMusic.volume = 0.25f;
        nightMusic.volume = 0f;
        //StartCoroutine(RegenBase());
    }

    

    //public IEnumerator RegenBase()
    //{
    //    while (isNight == false)
    //    {
    //        CastleBuilding.GetComponent<Castle>().Damaged(-passiveBaseRegen);
    //        yield return new WaitForSeconds(1f);
    //    }
    //}

    public void ActivateNight()
    {
        for (int i = 0; i < maptiles.Length; i++)
        {
            maptiles[i].color = Color.gray;
        }
        dayMusic.volume = 0f;
        nightMusic.volume = 0.25f;

        getRandomLocation();
    }

    public void EnemyKilled()
    {
        if (enemiesSpawned > 0)
        {
            enemiesSpawned--;
            enemiesText.text = "Nooms: \n" + enemiesSpawned;
        }
    }

    private void getRandomLocation()
    {
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
    private void spawnEnemy()
    {
        int toSpawn = Random.Range(0, enemies.Count);
        GameObject enemy1 = (GameObject)Instantiate(enemies[toSpawn], loc, Quaternion.identity);
        enemiesSpawned++;
        enemiesText.text = "Nooms: \n" + enemiesSpawned;
        Debug.Log("Spawned : " + enemy1.name.ToString());
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
        

        minutesTime += Time.deltaTime * clockSpeed * 4;
        clockRotation += Time.deltaTime * clockSpeed;

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
            enemiesPerRound += 3;

        }
        else if (hourTime == 6)
        {
            if (minutesTime == 0)
            {
                canRegenBase = true;
                isNight = false;
                updateCounter = 0;
                ActivateDay();
            }
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
        else if (hourTime == 18 && canSpawn) { ActivateNight(); canSpawn = false; }
        //else if (isNight  == true)
       // {
          //  isNight = false;
          //  ActivateNight();
        //}
    }

    

}