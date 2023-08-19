using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{   
    [SerializeField] private GameObject menu;
    [SerializeField] private KeyCode menuKey;

    protected bool menuWillPauseGame;

    private static bool isPaused;
    public static bool IsPaused {
        get { return isPaused; }
    }

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetInput()
    {
        if (Input.GetKeyDown(menuKey)) {
            if(menu.activeSelf && menuWillPauseGame) {
                ResumeGame();
            } else if(menu.activeSelf){
                menu.SetActive(false);
            } else if(menuWillPauseGame) {
                PauseGame();
            } else {
                menu.SetActive(true);
            }
        }
    }

    public void PauseGame() 
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame() 
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home Screen");
    }
}
