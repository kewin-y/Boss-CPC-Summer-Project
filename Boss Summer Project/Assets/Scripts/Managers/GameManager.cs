using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject background;

    private Transform powerUps; //Parent object for all power ups
    private Transform items; //Parent object for all items
    private PlayerController playerScript;
    private Enemy enemyScript;
    private BackgroundTiler backgroundTiler;
    private static UnityEvent respawnEvent = new();

    void Awake()
    {
        StatisticsSystem.LoadStatistics();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnSceneUnloaded(Scene current)
    {
        StatisticsSystem.SerializeJson();
    }

    void OnApplicationQuit()
    {
        StatisticsSystem.SerializeJson();
    }
    void Start()
    {
        powerUps = GameObject.Find("Power Ups").transform;
        items = GameObject.Find("Items").transform;

        playerScript = player.GetComponent<PlayerController>();
        enemyScript = enemy.GetComponent<Enemy>();
        backgroundTiler = background.GetComponent<BackgroundTiler>();

        SetupRespawnEvent();
    }

    //Add all power ups and items as listeners for the respawn event
    //NOTE: This makes it unnecessary to do so in the inspector.
    private void SetupRespawnEvent()
    {

        //Add power ups as listeners
        for (int i = 0; i < powerUps.childCount; i++)
        {
            PowerUp powerUpScript = powerUps.GetChild(i).GetComponent<PowerUp>();
            respawnEvent.AddListener(powerUpScript.Respawn);
        }

        //Add items as listeners
        for (int i = 0; i < items.childCount; i++)
        {
            Item itemScript = items.GetChild(i).GetComponent<Item>();
            respawnEvent.AddListener(itemScript.Respawn);
        }

        //Add the player as a listener
        respawnEvent.AddListener(playerScript.Respawn);

        //Add the enemy as a listener
        // WHY IS THERE ONLY ONE ENEMY 
        respawnEvent.AddListener(enemyScript.Respawn);


        respawnEvent.AddListener(backgroundTiler.ResetTiles);
    }

    public static void RespawnAll()
    {
        respawnEvent.Invoke();
    }

    public void ChangeScene(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }

    public void ExitApplication()
    {
        EditorApplication.ExitPlaymode();
    }

    public void EnterPauseMenu()
    {
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in allGameObjects)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
