using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float fanRange;
    [SerializeField] private Vector2 fanDirection;

    [SerializeField] private GameObject particles;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 fanDirectionNormalized = fanDirection.normalized;

        if(Mathf.Abs(player.transform.position[0] - transform.position[0]) <= fanRange) {
            rb2d.velocity += fanDirectionNormalized * 100f * Time.deltaTime;
            StartCoroutine(createParticles());
            
        }
    }
    IEnumerator createParticles(){
        GameObject steamParticles = Instantiate(particles) as GameObject;
        steamParticles.transform.position = player.transform.position;
        yield return new WaitForSeconds(2f);
        Destroy(steamParticles);
    }
}
