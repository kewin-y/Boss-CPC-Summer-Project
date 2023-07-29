using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCamera : MonoBehaviour
{
    [SerializeField] private GameObject laserEye;
    [SerializeField] private GameObject laser;
    [SerializeField] private float aimRadius;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private LayerMask whatIsPlayer;    //Layer mask that contains the player and shield
    [SerializeField] private float laserEyeFadeInDuration;
    [SerializeField] private float cameraHiddenTint;
    [SerializeField] private float laserDelay;
    [SerializeField] private float laserFadeOutTime;
    [SerializeField] private Transform laserStartPosition;
    [SerializeField] private GameObject shield;

    private PlayerController player;
    private LineRenderer laserRenderer;
    private ShieldController shieldScript;
    private bool playerDetected;
    private bool openFire = true;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObj.GetComponent<PlayerController>();
        laserRenderer = laser.GetComponent<LineRenderer>();
        shieldScript = shield.GetComponent<ShieldController>();

        VisualEffects.SetAlpha(laserEye, 0);
        VisualEffects.SetAlpha(laserRenderer, 0);
    }

    // Update is called once per frame
    void Update()
    {
        playerDetected = Physics2D.OverlapCircle(transform.position, aimRadius, whatIsPlayer);

        if (playerDetected) {

            //If the laser eye is hidden, reveal the camera and laser eye
            if (VisualEffects.GetAlpha(laserEye) == 0) {
                StartCoroutine(VisualEffects.FadeIn(laserEye, laserEyeFadeInDuration));
                StartCoroutine(VisualEffects.FadeToColor(gameObject, laserEyeFadeInDuration, Color.white));
            }

            //Make the laser point to the player
            laserEye.transform.right = playerObj.transform.position - laserEye.transform.position;
            laserEye.transform.Rotate(new Vector3(0, 0, 90));    //offset 90 degrees in z axis (fix)

            if (openFire)
                StartCoroutine(ShootSequence());
        } 
        else {

            //If the laser eye is revealed, hide the camera and laser eye
            if (VisualEffects.GetAlpha(laserEye) == 1) {
                StartCoroutine(VisualEffects.FadeOut(laserEye, laserEyeFadeInDuration));
                StartCoroutine(VisualEffects.FadeToColor(gameObject, laserEyeFadeInDuration, Color.black));
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

        //Fire a laser at the player or shield, whichever comes first
        StartCoroutine(FireLaser());

        openFire = true;
    }

    private IEnumerator FireLaser() {

        laser.SetActive(true);

        //Draw the laser from the laser eye to the player
        laserRenderer.SetPosition(0, laserStartPosition.position);

        Vector2 direction = (Vector2) playerObj.transform.position - (Vector2) laserStartPosition.position;
        RaycastHit2D hit = Physics2D.Raycast(laserStartPosition.position, direction.normalized, direction.magnitude, whatIsPlayer);

        if (hit) {

            laserRenderer.SetPosition(1, hit.point);

            GameObject target = hit.collider.gameObject;
            Damageable targetScript = target.GetComponent<Damageable>();

            //Laser appears while tinting the target red, inflicting damage
            VisualEffects.SetAlpha(laserRenderer, 1);
            VisualEffects.SetColor(target, Color.red);
            targetScript.TakeDamage(30);

            //Laser immediately starts fading out along with red tint (white tint = restore original colour)
            StartCoroutine(VisualEffects.FadeOut(laserRenderer, laserFadeOutTime));
            yield return StartCoroutine(VisualEffects.FadeToColor(target, laserFadeOutTime, Color.white));

        }
        
        laser.SetActive(false);

    }
}