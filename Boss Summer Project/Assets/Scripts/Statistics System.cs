using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.IO;

public class StatisticsSystem : MonoBehaviour
{
    public TextMeshProUGUI statisticsDisplay;

    public static int NumberOfJumps = 0;
    public static float DistanceTraveled = 0;
    /*
    private string fileLocation = "Assets/Game Data/Statistics.txt";
    private StreamWriter writer = new StreamWriter(fileLocation, true);
    private StreamReader reader = new StreamReader(fileLocation); 
    
    public Rigidbody2D playerRb2d;
    public PlayerController playerController;
    public GameObject player;
    */
    // Start is called before the first frame update
    void Start()
    {
        
        //DontDestroyOnLoad(this.gameObject);
        //playerRb2d = player.GetComponent<Rigidbody2D>();
        //NumberOfJumps = 0;
        //DistanceTraveled = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        statisticsDisplay.text = "Number of Jumps: " + NumberOfJumps + "\nDistance Traveled: " + DistanceTraveled;
        //DistanceTraveled += playerRb2d.velocity.magnitude * Time.deltaTime;
    }
    public void AddJump() {
        NumberOfJumps++;
    }
    /*
    static void WriteString(){
        //Write some text to the test.txt file
        writer.WriteLine(numberOfJumps);
        writer.Close();
        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path); 
        TextAsset asset = Resources.Load("test");
        //Print the text from the file
        Debug.Log(asset.text);
    }
    static double ReadString()
    {
        //Read the text from the statistics file
        int a = reader.ReadLine();
        reader.Close();
        return a;
    }
    */
}
