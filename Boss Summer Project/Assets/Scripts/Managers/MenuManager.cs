using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{   
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private KeyCode menuKey;

    private static bool isPaused;
    public static bool IsPaused {
        get { return isPaused; }
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(menuKey)) {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame() 
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame() 
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home Screen");
    }
}
