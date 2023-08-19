using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMenuManager : MenuManager
{
    void Start()
    {
        menuWillPauseGame = false;
    }
    void Update()
    {
        GetInput();
    }
}
