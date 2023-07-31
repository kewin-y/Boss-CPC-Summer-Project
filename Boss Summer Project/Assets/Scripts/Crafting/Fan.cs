using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public float fanRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(player.transform.position[0] - transform.position[0]) < fanRange){
            PlayerController.isAffectedByFan = true;
        }
    }
}
