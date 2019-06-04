using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void PlayPracticeMode()
    {
        SceneManager.LoadScene("MainPracticeMode");
    }

    public void PlayEasyMode()
    {
        SceneManager.LoadScene("MainEasyMode");
    }

    public void PlayHardMode()
    {
        SceneManager.LoadScene("MainHardMode");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
