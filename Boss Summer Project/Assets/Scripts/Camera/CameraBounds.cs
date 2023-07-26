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

        AdjustBounds();
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

    //Adjusts the camera collider bounds to match the orthographic size
    private void AdjustBounds() {

        sizeY = cam.orthographicSize * 2;
        ratio = (float) Screen.width / (float) Screen.height;
        sizeX = sizeY * ratio;

        bc2d.size = new Vector2(sizeX, sizeY);

    }

    public void Zoom(float zoom, float duration) => StartCoroutine(ZoomSequence(zoom, duration));

    //Changes the zoom of the camera by a certain amount over a certain number of seconds:
    //positive = zoom in, negative = zoom out
    private IEnumerator ZoomSequence(float zoom, float duration) {
        float originalCamSize = cam.orthographicSize;
        float requiredCamSize = originalCamSize - zoom;

        float timeElapsed = 0f;
        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float newCamSize = Mathf.Lerp(originalCamSize, requiredCamSize, timeElapsed / duration);
            Debug.Log(newCamSize);
            cam.orthographicSize = newCamSize;

            yield return null;  //Wait for the next frame to pass
        }
        
        AdjustBounds();
    }

}
