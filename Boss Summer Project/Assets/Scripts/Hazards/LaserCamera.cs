using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCamera : MonoBehaviour
{
    [SerializeField] private GameObject laserEye;
    [SerializeField] private GameObject laser;
    [SerializeField] private float aimRadius;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float laserEyeFadeInDuration;
    [SerializeField] private float cameraHiddenTint;
    [SerializeField] private float laserDelay;
    [SerializeField] private float laserFadeOutTime;
    [SerializeField] private Transform laserStartPosition;
    [SerializeField] private GameObject shield;

    private PlayerController player;
    private LineRenderer laserRenderer;
    private Laser laserScript;
    private ShieldController shieldScript;
    private bool playerDetected;
    private bool openFire = true;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObj.GetComponent<PlayerController>();
        laserRenderer = laser.GetComponent<LineRenderer>();
        laserScript = laser.GetComponent<Laser>();
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
        laserRenderer.SetPosition(1, playerObj.transform.position);
        laserScript.SetEdgeCollider();

        yield return null;

        //If the laser is blocked by a shield, hit the shield instead
        if (laserScript.IsBlockedByShield) {
            laserRenderer.SetPosition(1, shield.transform.position);
            yield return StartCoroutine(shootShield());
        } 
        else {
            yield return StartCoroutine(shootPlayer());
        }
        
        laser.SetActive(false);

    }

    private IEnumerator shootShield() {
        //Laser appears while tinting the shield grey, decreasing its durability
        VisualEffects.SetAlpha(laserRenderer, 1);
        VisualEffects.SetColor(shield, Color.grey);
        shieldScript.TakeDamage(30);

        //Laser immediately starts fading out along with red tint (white tint = restore original colour)
        StartCoroutine(VisualEffects.FadeOut(laserRenderer, laserFadeOutTime));
        yield return StartCoroutine(VisualEffects.FadeToColor(shield, laserFadeOutTime, Color.white));
    }

    private IEnumerator shootPlayer() {
        //Laser appears while tinting the player red, inflicting damage
        VisualEffects.SetAlpha(laserRenderer, 1);
        VisualEffects.SetColor(playerObj, Color.red);
        player.TakeDamage(30);

        //Laser immediately starts fading out along with red tint (white tint = restore original colour)
        StartCoroutine(VisualEffects.FadeOut(laserRenderer, laserFadeOutTime));
        yield return StartCoroutine(VisualEffects.FadeToColor(playerObj, laserFadeOutTime, Color.white));
    }
}