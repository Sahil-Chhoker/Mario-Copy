using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;

    public void Pause()
    {
        FindObjectOfType<AudioManager>().Play("Pause");
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void Resume()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void Exit()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void Restart()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }
}
