using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    [SerializeField] private float moveTime; 
    private Camera cam;
    private BoxCollider2D bc2d; // Camera's collider

    private float sizeY;
    private float ratio; 
    private float sizeX;
    private void Start()
    {
        cam = GetComponent<Camera>();
        bc2d = GetComponent<BoxCollider2D>();

        sizeY = cam.orthographicSize * 2;
        ratio = (float) Screen.width / (float) Screen.height;
        sizeX = sizeY * ratio;

        bc2d.size = new Vector2(sizeX, sizeY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Player")
        {
            Vector2 newPos; 
            bool isRight = other.transform.position.x - transform.position.x > 0;

            Debug.Log(isRight);

            if(isRight)
            {
                newPos = new Vector3(transform.position.x + sizeX, transform.position.y);
                other.transform.position = new Vector2(other.transform.position.x + 1f, other.transform.position.y);

            }

            else
            {
                newPos = new Vector3(transform.position.x - sizeX, transform.position.y);
                other.transform.position = new Vector2(other.transform.position.x - 1f, other.transform.position.y);
            }
                    
            LeanTween.move(gameObject, newPos, moveTime);   
        }
    }

}
