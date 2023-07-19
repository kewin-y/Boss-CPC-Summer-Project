using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCamera : MonoBehaviour
{
    [SerializeField] private GameObject laserEye;
    [SerializeField] private LineRenderer laserRenderer;
    [SerializeField] private float aimRadius;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float laserEyeFadeInDuration;
    [SerializeField] private float cameraHiddenTint;
    [SerializeField] private float laserDelay;
    [SerializeField] private float laserFadeOutTime;

    private bool playerDetected;
    private bool openFire = true;

    // Start is called before the first frame update
    void Start()
    {
        SetAlpha(laserEye, 0);
        SetAlpha(laserRenderer, 0);
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

        //Wait, then shoot the laser
        yield return new WaitForSeconds(laserDelay);

        //If the player is out of range now, don't shoot
        if (!playerDetected) {
            openFire = true;
            yield break;
        }

        //Draw the laser from the camera to the player
        laserRenderer.SetPosition(0, transform.position);
        laserRenderer.SetPosition(1, playerTransform.position);

        //Pulse effect: laser appears, then immediately starts fading out
        SetAlpha(laserRenderer, 1);
        yield return StartCoroutine(FadeOut(laserRenderer, laserFadeOutTime));

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

    //Fade out a GameObject over a specified duration
    private IEnumerator FadeOut(GameObject obj, float duration) {
        float timeElapsed = 0;

        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1, 0, timeElapsed / duration);
            SetAlpha(obj, newAlpha);

            yield return null;  //Wait for next frame to pass
        }
    }

    //Fade out a LineRenderer over a specified duration (used exclusively for laser)
    private IEnumerator FadeOut(LineRenderer laser, float duration) {
        float timeElapsed = 0;

        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1, 0, timeElapsed / duration);
            SetAlpha(laser, newAlpha);

            yield return null;  //Wait for next frame to pass
        }
    }

    //Change the alpha value of the object's sprite renderer
    private void SetAlpha(GameObject obj, float alpha) {
        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
        Color spriteColor = objRenderer.material.color;
        objRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
    }

    //Change the alpha value of a line renderer (used exclusively for laser)
    private void SetAlpha(LineRenderer laser, float alpha) {
        Color laserColor = laser.material.color;
        laser.material.color = new Color(laserColor.r, laserColor.g, laserColor.b, alpha);
    }

    private float GetAlpha(GameObject obj) {
        Color spriteColor = obj.GetComponent<SpriteRenderer>().color;
        return spriteColor.a;
    }
}
