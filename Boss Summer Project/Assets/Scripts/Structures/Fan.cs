using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float fanRange;
    [SerializeField] private float fanRangeVertical;
    [SerializeField] private Vector2 fanDirection;
    [SerializeField] ParticleSystem particles;

    private float rotation;

    private Rigidbody2D playerRB;

    // Start is called before the first frame update
    void Start()
    {
        rotation = 0f;
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 fanDirectionNormalized = fanDirection.normalized;

        if (Mathf.Abs(player.transform.position.x - transform.position.x) <= fanRange &&
        player.transform.position.y - transform.position.y <= fanRangeVertical)
        {
            playerRB.velocity += fanDirectionNormalized * 75f * Time.deltaTime;

        }

    }
    void FixedUpdate()
    {

    }

}
