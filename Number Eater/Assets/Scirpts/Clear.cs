using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    [SerializeField]
    GameObject resumeButton;

    [SerializeField]
    GameObject mainMenuButton;

    public void ClearButtonOn()
    {
        resumeButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    public void ClearButtonOff()
    {
        resumeButton.SetActive(false);
        mainMenuButton.SetActive(false);
    }

    public void StarSFXOn(int num)
    {
        //switch(num)
        //{
        //    case 1:
        //        SFXManager.Instance.PlayShot(SFXType.Star1);
        //        break;
        //    case 2:
        //        SFXManager.Instance.PlayShot(SFXType.Star2);
        //        break;
        //    case 3:
        //        SFXManager.Instance.PlayShot(SFXType.Star3);
        //        break;

        //}
    }

    public void GameClearTextOn()
    {
        //SFXManager.Instance.PlayShot(SFXType.GameClearText);
    }
}
