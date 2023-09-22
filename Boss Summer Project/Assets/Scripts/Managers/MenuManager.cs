using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{   
    [SerializeField] private Menu[] menuObjects;
    [SerializeField] private KeyCode exitMenuKey;

    private static bool isPaused;
    public static bool IsPaused {
        get { return isPaused; }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Menu menu in menuObjects) {
            menu.Panel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }
    public void GetInput()
    {
        Menu[] activeMenus = new Menu[2];

        int i = 0;
        foreach (Menu menu in menuObjects) {
            if (menu.Panel.activeSelf) {
                activeMenus[i] = menu;
                i++;
            }
        }

        foreach (Menu menu in menuObjects) {
            if (Input.GetKeyDown(exitMenuKey) && i >= 1) {
                CloseMenu(menu);
            } else if (Input.GetKeyDown(menu.MenuKey)) {
                if (menu.Panel.activeSelf) {
                    CloseMenu(menu);     
                } else {
                    OpenMenu(menu);
                }
            }
        }
    }
    public void OpenMenu(Menu menu) {
        if (menu.MenuWillPauseGame) {
            PauseGame(menu.Panel);
        } else {
            menu.Panel.SetActive(true);
        }
    }
    public void CloseMenu(Menu menu) {
        if (menu.MenuWillPauseGame) {
            ResumeGame(menu.Panel);
        } else {
            menu.Panel.SetActive(false);
        }
    }

    public void PauseGame(GameObject panel) 
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame(GameObject panel) 
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMenu() {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}