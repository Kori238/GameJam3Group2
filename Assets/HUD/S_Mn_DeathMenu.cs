using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_Mn_DeathMenu : MonoBehaviour
{
    public Slider baseHealth;
    public GameObject deathScreen;

    private void Start()
    {
        
    }

    // Start is called before the first frame update
    private void Update()
    {
        if(baseHealth.value <= 0)
        {
            deathScreen.SetActive(true);
            Time.timeScale = 0.5f;
        }
    }


    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
