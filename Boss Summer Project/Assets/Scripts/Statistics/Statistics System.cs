using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.IO;

public class StatisticsSystem : MonoBehaviour
{
    public TextMeshProUGUI statisticsDisplay;

    private Statistics playerStats;
    public Statistics PlayerStats {
        get { return playerStats; }
        set { playerStats = value; }
    }

    private string fileLocation = "Assets/Game Data/Statistics.txt";
    private StreamWriter writer;
    private StreamReader reader;

    // Start is called before the first frame update
    void Start()
    {
        writer = new StreamWriter(fileLocation, true);
        reader = new StreamReader(fileLocation);

        playerStats = JsonUtility.FromJson<Statistics>(ReadString());
        if (playerStats == null) {
            playerStats = new Statistics(playerStats.Jumps, playerStats.DistanceTraveled);
        }
        //DontDestroyOnLoad(this.gameObject);
        //playerRb2d = player.GetComponent<Rigidbody2D>();
        //NumberOfJumps = 0;
        //DistanceTraveled = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        statisticsDisplay.text = "Number of Jumps: " + playerStats.Jumps + "\nDistance Traveled: " + playerStats.DistanceTraveled;
        //DistanceTraveled += playerRb2d.velocity.magnitude * Time.deltaTime;
    }
    public void AddJump() {
        playerStats.Jumps++;
    }

    void OnApplicationQuit() {
        string json = JsonUtility.ToJson(playerStats, true);
        WriteString(json);
    }
    void WriteString(string text){
        //Write some text to the Statistics.txt file
        writer.WriteLine(text);
        writer.Close();

        /*
        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path); 
        TextAsset asset = Resources.Load("Statistics");
        //Print the text from the file
        Debug.Log(asset.text);
        */
    }
    string ReadString()
    {
        //Read the text from the Statistics file
        string text = reader.ReadLine();
        reader.Close();
        return text;
    }
}
