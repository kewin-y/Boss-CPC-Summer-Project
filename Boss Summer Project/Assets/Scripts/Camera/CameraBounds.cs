using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    private Camera cam;
    private BoxCollider2D bc2d; // Camera's collider
    void Start()
    {
        cam = GetComponent<Camera>();
        bc2d = GetComponent<BoxCollider2D>();

        float sizeY = cam.orthographicSize * 2;
        float ratio = (float) Screen.width / (float) Screen.height;
        float sizeX = sizeY * ratio;

        bc2d.size = new Vector2(sizeX, sizeY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
