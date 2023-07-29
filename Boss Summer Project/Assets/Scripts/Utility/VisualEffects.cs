using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility class containing static methods which change the appearance of an object
public class VisualEffects : MonoBehaviour
{
    public static IEnumerator FadeToColor(GameObject obj, float duration, Color reqColor) {
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

    public static void SetColor(GameObject obj, Color newColor) {
        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
        objRenderer.color = newColor;
    }

    public static IEnumerator FadeIn(GameObject obj, float duration) {
        float timeElapsed = 0f;

        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0, 1, timeElapsed / duration);
            SetAlpha(obj, newAlpha);

            yield return null;  //Wait for next frame to pass
        }
    }

    //Fade out a GameObject over a specified duration
    public static IEnumerator FadeOut(GameObject obj, float duration) {
        float timeElapsed = 0;

        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1, 0, timeElapsed / duration);
            SetAlpha(obj, newAlpha);

            yield return null;  //Wait for next frame to pass
        }
    }

    //Fade out a line renderer over a specified duration (used exclusively for laser)
    public static IEnumerator FadeOut(LineRenderer laser, float duration) {
        float timeElapsed = 0;

        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1, 0, timeElapsed / duration);
            SetAlpha(laser, newAlpha);

            yield return null;  //Wait for next frame to pass
        }
    }

    //Change the alpha value of the object's sprite renderer
    public static void SetAlpha(GameObject obj, float alpha) {
        SpriteRenderer objRenderer = obj.GetComponent<SpriteRenderer>();
        Color spriteColor = objRenderer.material.color;
        objRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
    }

    //Change the alpha value of a line renderer (used exclusively for laser)
    public static void SetAlpha(LineRenderer laser, float alpha) {
        Color laserColor = laser.material.color;
        laser.material.color = new Color(laserColor.r, laserColor.g, laserColor.b, alpha);
    }

    //Get the alpha value of an object
    public static float GetAlpha(GameObject obj) {
        Color spriteColor = obj.GetComponent<SpriteRenderer>().color;
        return spriteColor.a;
    }
}
