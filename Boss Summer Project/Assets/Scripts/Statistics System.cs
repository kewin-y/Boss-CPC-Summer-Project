using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class StatisticsSystem : MonoBehaviour
{
    public TextMeshProUGUI statisticsDisplay;

    public float NumberOfJumps;
    public float DistanceTraveled;
    /*
    public Rigidbody2D playerRb2d;
    public PlayerController playerController;
    public GameObject player;
    */
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        //playerRb2d = player.GetComponent<Rigidbody2D>();
        NumberOfJumps = DistanceTraveled = 0;
    }

    // Update is called once per frame
    void Update()
    {
        statisticsDisplay.text = "Number of Jumps:" + NumberOfJumps;
        //DistanceTraveled += playerRb2d.velocity.magnitude * Time.deltaTime;
    }
    public void AddJump() {
        NumberOfJumps++;
    }
    
}
