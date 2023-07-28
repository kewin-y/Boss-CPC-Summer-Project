using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCamera : MonoBehaviour
{
    [SerializeField] private GameObject laserEye;
    [SerializeField] private LineRenderer laserRenderer;
    [SerializeField] private float aimRadius;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float laserEyeFadeInDuration;
    [SerializeField] private float cameraHiddenTint;
    [SerializeField] private float laserDelay;
    [SerializeField] private float laserFadeOutTime;
    [SerializeField] private Transform laserStartPosition;

    private PlayerController player;
    private bool playerDetected;
    private bool openFire = true;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObj.GetComponent<PlayerController>();
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
                StartCoroutine(FadeToColor(gameObject, laserEyeFadeInDuration, Color.white));
            }

            //Make the laser point to the player
            laserEye.transform.right = playerObj.transform.position - laserEye.transform.position;
            laserEye.transform.Rotate(new Vector3(0, 0, 90));    //offset 90 degrees in z axis (fix)

            if (openFire)
                StartCoroutine(ShootSequence());
        } 
        else {

            //If the laser eye is revealed, hide the camera and laser eye
            if (GetAlpha(laserEye) == 1) {
                StartCoroutine(FadeOut(laserEye, laserEyeFadeInDuration));
                StartCoroutine(FadeToColor(gameObject, laserEyeFadeInDuration, Color.black));
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
        // laserRenderer.SetPosition(0, transform.position);
        laserRenderer.SetPosition(0, laserStartPosition.position);
        laserRenderer.SetPosition(1, playerObj.transform.position);

        ///Laser appears while tinting the player red, inflicting damage
        SetAlpha(laserRenderer, 1);
        SetColor(playerObj, Color.red);
        player.TakeDamage(30);

        //Laser immediately starts fading out along with red tint
        StartCoroutine(FadeOut(laserRenderer, laserFadeOutTime));
        yield return StartCoroutine(FadeToColor(playerObj, laserFadeOutTime, Color.white));  //White tint = restore original color

        openFire = true;
    }

    private IEnumerator FadeToColor(GameObject obj, float duration, Color reqColor) {
        Color originalColor = obj.GetComponent<SpriteRenderer>().color;

        float timeElapsed = 0f;
        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;

            //Fade from the original color to the required color
            Color newColor = Color.Lerp(originalColor, reqColor, timeElapsed / duration);
            SetColor(obj, newColor);

            yield return null;  //Wait for next frame to pass
        }
    }

    private void SetColor(GameObject obj, Color newColor) {
        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
        objRenderer.color = newColor;
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

    //Get the alpha value of an object
    private float GetAlpha(GameObject obj) {
        Color spriteColor = obj.GetComponent<SpriteRenderer>().color;
        return spriteColor.a;
    }
}
