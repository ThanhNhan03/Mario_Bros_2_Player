using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        LoadingScreen.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
