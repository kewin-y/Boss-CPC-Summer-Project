using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailFlip : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float direction = (transform.position.x - player.transform.position.x) / Mathf.Abs(transform.position.x - player.transform.position.x) * -1;
        transform.localScale = new(direction, 1f, 1f);
    }
}
