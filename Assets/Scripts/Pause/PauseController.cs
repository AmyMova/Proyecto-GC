using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    GameObject pausePanel;

    bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0.0F;
        AudioManager.Instance.PauseTheme();
    }

    public void Home()
    {
        SceneManager.LoadScene("Welcome");
    }

    public void Resume()
    {
        AudioManager.Instance.ResumeTheme();
        pausePanel.SetActive(false);
        Time.timeScale = 1.0F;
    }
}
