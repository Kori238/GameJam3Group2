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
            Time.timeScale = 0f;
            deathScreen.SetActive(true);
            
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
