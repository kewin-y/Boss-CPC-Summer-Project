using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MenuManager
{
    // Start is called before the first frame update
    void Start()
    {
        menuWillPauseGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }
}
