using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor;
public class HomeScreenManager : MonoBehaviour
{
    [SerializeField] private KeyCode level1Key;
    [SerializeField] private KeyCode level2Key;
    [SerializeField] private KeyCode level3Key;
    [SerializeField] private KeyCode level4Key;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(level1Key)) {
            ChangeScene(1);
        } else if (Input.GetKeyDown(level2Key)) {
            ChangeScene(2);
        } else if (Input.GetKeyDown(level3Key)) {
            ChangeScene(3);
        } else if (Input.GetKeyDown(level4Key)) {
            ChangeScene(4);
        }
    }
    public void ChangeScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }
    /*
    public void ExitApplication()
    {
        EditorApplication.ExitPlaymode();
    }
    */
}
