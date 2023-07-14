using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCamera : MonoBehaviour
{
    [SerializeField] private float aimRadius;
    [SerializeField] private GameObject laserEye;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float laserEyeFadeInDuration;
    [SerializeField] private float cameraHiddenTint;
    [SerializeField] private float laserDelay;

    private bool playerDetected;
    private bool openFire = true;

    // Start is called before the first frame update
    void Start()
    {
        SetAlpha(laserEye, 0);
    }

    // Update is called once per frame
    void Update()
    {
        playerDetected = Physics2D.OverlapCircle(transform.position, aimRadius, whatIsPlayer);

        if (playerDetected) {

            //If the laser eye is hidden, reveal the camera and laser eye
            if (GetAlpha(laserEye) == 0) {
                StartCoroutine(FadeIn(laserEye, laserEyeFadeInDuration));
                StartCoroutine(FadeBrighten(gameObject, laserEyeFadeInDuration));
            }

            //Make the laser point to the player
            laserEye.transform.right = playerTransform.position - laserEye.transform.position;
            laserEye.transform.Rotate(new Vector3(0, 0, 90));    //offset 90 degrees in z axis (fix)

            if (openFire)
                StartCoroutine(ShootSequence());
        } 
        else {

            //If the laser eye is revealed, hide the camera and laser eye
            if (GetAlpha(laserEye) == 1) {
                StartCoroutine(FadeOut(laserEye, laserEyeFadeInDuration));
                StartCoroutine(FadeDarken(gameObject, laserEyeFadeInDuration));
            }
        }
    }

    private IEnumerator ShootSequence() {
        openFire = false;
        yield return new WaitForSeconds(laserDelay);
        
        Debug.Log("Shoot");
        openFire = true;
    }

    private IEnumerator FadeBrighten(GameObject obj, float duration) {
        float timeElapsed = 0f;
        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;

            //Restore "white tint" by turning up all r, g, and b values
            float newTint = Mathf.Lerp(cameraHiddenTint, 1, timeElapsed / duration);
            SetTint(obj, newTint);

            yield return null;  //Wait for next frame to pass
        }
    }

    private IEnumerator FadeDarken(GameObject obj, float duration) {
        float timeElapsed = 0f;
        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;

            //Add slight black tint by turning down all r, g, and b values
            float newTint = Mathf.Lerp(1, cameraHiddenTint, timeElapsed / duration);
            SetTint(obj, newTint);

            yield return null;  //Wait for next frame to pass
        }
    }

    private void SetTint(GameObject obj, float tint) {
        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
        objRenderer.color = new Color(tint, tint, tint, 1);
    }

    private IEnumerator FadeIn(GameObject obj, float duration) {
        float timeElapsed = 0f;
        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0, 1, timeElapsed / duration);
            SetAlpha(obj, newAlpha);

            yield return null;  //Wait for next frame to pass
        }
    }

    private IEnumerator FadeOut(GameObject obj, float duration) {
        float timeElapsed = 0;
        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1, 0, timeElapsed / duration);
            SetAlpha(obj, newAlpha);

            yield return null;  //Wait for next frame to pass
        }
    }

    private void SetAlpha(GameObject obj, float alpha) {
        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
        Color spriteColor = objRenderer.material.color;
        objRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
    }

    private float GetAlpha(GameObject obj) {
        Color spriteColor = obj.GetComponent<SpriteRenderer>().color;
        return spriteColor.a;
    }
}
