using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CameraBounds : MonoBehaviour
{
    [SerializeField] private UnityEvent playerExitRight;
    [SerializeField] private UnityEvent playerExitLeft;
    [SerializeField] private float moveTime; 

    private float bufferValue = 1f; // Moves the player this much to the left/right of the camera to fix a bug
    private bool cameraCanMove;
    public bool CameraCanMove 
    {
        get{return cameraCanMove;}
        set{cameraCanMove = value;}
    }

    private Camera cam;
    private BoxCollider2D bc2d; // Camera's collider

    private float moveAmountX;
    private float moveAmountY;

    private float sizeY;
    private float ratio; 
    private float sizeX;
    private void Start()
    {
        cam = GetComponent<Camera>();
        bc2d = GetComponent<BoxCollider2D>();
        cameraCanMove = true;

        AdjustBounds();

        moveAmountX = sizeX;
        moveAmountY = sizeY;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerExit2D(Collider2D other)
    {

        if(other.tag == "Player" && cameraCanMove)
        {
            Vector3 newPos; 
            bool isRight = other.transform.position.x - transform.position.x > 0;

            if(isRight)
            {
                newPos = new Vector3(transform.position.x + moveAmountX, transform.position.y, -10f);
                other.transform.position = new Vector2(other.transform.position.x + bufferValue, other.transform.position.y);
                playerExitRight.Invoke();
            }

            else
            {
                newPos = new Vector3(transform.position.x - moveAmountX, transform.position.y, -10f);
                other.transform.position = new Vector2(other.transform.position.x - bufferValue, other.transform.position.y); // Might change the 
                playerExitLeft.Invoke();
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

        bufferValue -= zoom * 0.5f;

        float timeElapsed = 0f;
        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float newCamSize = Mathf.Lerp(originalCamSize, requiredCamSize, timeElapsed / duration);
            cam.orthographicSize = newCamSize;

            yield return null;  //Wait for the next frame to pass
        }
        
        AdjustBounds();
    }

}
