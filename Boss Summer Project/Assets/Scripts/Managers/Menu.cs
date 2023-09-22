using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    [SerializeField] private KeyCode menuKey;
    public KeyCode MenuKey {
        get { return menuKey; }
        set { menuKey = value; }
    }

    [SerializeField] private GameObject panel;
    public GameObject Panel {
        get { return panel; }
        set { panel = value; }
    }

    [SerializeField] private bool menuWillPauseGame;
    public bool MenuWillPauseGame {
        get { return menuWillPauseGame; }
        set { menuWillPauseGame = value; }
    }

    public Menu(KeyCode menuKey, GameObject panel, bool menuWillPauseGame) {
        this.menuKey = menuKey;
        this.panel = panel;
        this.menuWillPauseGame = menuWillPauseGame;
    }
}