using UnityEngine;
using UnityEngine.SceneManagement;

public class S_Mn_Buttons : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Game Exit.");
        Application.Quit();
    }
}
