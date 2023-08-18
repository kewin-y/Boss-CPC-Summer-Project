using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    /*
    void Start()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    */
    public void ChangeScene(int sceneBuildIndex)
    {
        if(this.gameObject.name == "Canvas" && this.gameObject.scene.name == "Statistics Menu") 
        {
            DontDestroyOnLoad(this.gameObject);
        }
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }
    public void ExitApplication()
    {
        EditorApplication.ExitPlaymode();
    }
    public void enterPauseMenu()
    {
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in allGameObjects) {
            DontDestroyOnLoad(gameObject);
        }
    }
}
