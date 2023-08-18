using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform powerUps;    //Parent object for all power ups
    [SerializeField] private Transform items;       //Parent object for all items
    [SerializeField] private GameObject player;

    private PlayerController playerScript;
    private static UnityEvent respawnEvent = new();

    void Start()
    {
        playerScript = player.GetComponent<PlayerController>();

        SetupRespawnEvent();
        // SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    //Add all power ups and items as listeners for the respawn event
    //NOTE: This makes it unnecessary to do so in the inspector.
    private void SetupRespawnEvent() {

        //Add power ups as listeners
        for (int i = 0; i < powerUps.childCount; i++) {
            PowerUp powerUpScript = powerUps.GetChild(i).GetComponent<PowerUp>();
            respawnEvent.AddListener(powerUpScript.Respawn);
        }

        //Add items as listeners
        for (int i = 0; i < items.childCount; i++) {
            Item itemScript = items.GetChild(i).GetComponent<Item>();
            respawnEvent.AddListener(itemScript.Respawn);
        }

        //Add the player as a listener
        respawnEvent.AddListener(playerScript.Respawn);
    }

    public static void RespawnAll() {
        respawnEvent.Invoke();
    }

    public void ChangeScene(int sceneBuildIndex)
    {
        if(gameObject.name == "Canvas" && gameObject.scene.name == "Statistics Menu") 
        {
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }

    public void ExitApplication()
    {
        EditorApplication.ExitPlaymode();
    }

    public void EnterPauseMenu()
    {
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in allGameObjects) {
            DontDestroyOnLoad(gameObject);
        }
    }
}
