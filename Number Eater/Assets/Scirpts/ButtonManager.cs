using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public void ToMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        GameManager.Instance.GameRetry();
    }

    
}
