using System;
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
    [SerializeField] private LayerMask whatIsGround;

    private float bufferValue = 1f; // Moves the player this much to the left/right of the camera to fix a bug
    public float BufferValue { get => bufferValue; set => bufferValue = value; }
    private bool cameraCanMove;
    public bool CameraCanMove
    {
        get { return cameraCanMove; }
        set { cameraCanMove = value; }
    }



    private Camera cam;
    private BoxCollider2D bc2d; // Camera's collider

    private float moveAmountX;
    private float moveAmountY; // might not need this 

    private float sizeY;
    private float sizeX;

    private float defaultSize;
    private float defaultBufferValue;
    public float DefaultBufferValue { get => defaultBufferValue; set => defaultBufferValue = value; }
    private void Start()
    {
        cam = GetComponent<Camera>();
        bc2d = GetComponent<BoxCollider2D>();
        cameraCanMove = true;

        AdjustBounds();
        defaultSize = cam.orthographicSize;
        defaultBufferValue = bufferValue;
        print(bufferValue);

        moveAmountX = sizeX;
        moveAmountY = sizeY;
    }

    // Update is called once per frame
    void Update()
    {
        // print(bufferValue);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cameraCanMove)
        {
            Vector3 newPos;
            bool isRight = other.transform.position.x - transform.position.x > 0;

            if (isRight)
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(other.transform.position, Vector2.right, 100f, whatIsGround);

                if (raycastHit && raycastHit.distance > 1)
                {
                    bufferValue = 1;
                }
                else
                {
                    bufferValue = raycastHit.distance;
                    print(raycastHit.collider);
                }

                newPos = new Vector3(transform.position.x + moveAmountX, transform.position.y, -10f);
                other.transform.position = new Vector2(other.transform.position.x + bufferValue, other.transform.position.y);
                playerExitRight.Invoke();
                bufferValue = defaultBufferValue;
            }

            else
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(other.transform.position, Vector2.left, 100f, whatIsGround);

                if (raycastHit && raycastHit.distance > 1)
                {
                    bufferValue = 1;
                }
                else
                {
                    bufferValue = raycastHit.distance;
                    print(raycastHit.collider);
                }

                newPos = new Vector3(transform.position.x - moveAmountX, transform.position.y, -10f);
                other.transform.position = new Vector2(other.transform.position.x - bufferValue, other.transform.position.y); // Might change the 
                playerExitLeft.Invoke();
                bufferValue = defaultBufferValue;
            }

            if (isActiveAndEnabled) StartCoroutine(MoveCamera(newPos));

        }
    }

    IEnumerator MoveCamera(Vector3 newPos)
    {
        cameraCanMove = false;

        var startPos = transform.position;

        float timeElapsed = 0;
        while (timeElapsed < moveTime)
        {
            transform.position = Vector3.Lerp(startPos, newPos, timeElapsed / moveTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = newPos;
        cameraCanMove = true;
    }


    //Adjusts the camera collider bounds to match the orthographic size
    private void AdjustBounds()
    {

        sizeY = cam.orthographicSize * 2;
        sizeX = sizeY * cam.aspect;

        bc2d.size = new Vector2(sizeX, sizeY);

    }

    public void Zoom(float zoom, float duration) => StartCoroutine(ZoomSequence(zoom, duration));

    public void ZoomOut()
    {
        float req = 6.5f;
        StartCoroutine(ZoomSequence(req, 0.5f));
    }

    public void ZoomIn()
    {
        float req = 6f;
        StartCoroutine(ZoomSequence(req, 0.5f));
    }
    //Changes the zoom of the camera by a certain amount over a certain number of seconds:
    //positive = zoom in, negative = zoom out
    private IEnumerator ZoomSequence(float requiredCamSize, float duration)
    {
        float originalCamSize = cam.orthographicSize;

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float newCamSize = Mathf.Lerp(originalCamSize, requiredCamSize, timeElapsed / duration);
            cam.orthographicSize = newCamSize;

            yield return null;  //Wait for the next frame to pass
        }

        AdjustBounds();
    }

}
