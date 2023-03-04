using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_Pl_PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject HUD;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
