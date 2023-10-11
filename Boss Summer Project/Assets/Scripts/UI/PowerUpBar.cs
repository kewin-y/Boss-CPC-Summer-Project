using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private PowerUp powerUp;

    //Acts as a constructor method: receives the power up, sets the icon, sets max duration
    public void Setup(PowerUp powerUp) {
        this.powerUp = powerUp;
        SetPowerUpIcon();
        SetMaxDuration(powerUp.Duration);
    }

    //Sets the sprite of the power up bar to the power up icon
    private void SetPowerUpIcon() {
        GameObject background = gameObject.transform.GetChild(0).gameObject;
        GameObject fill = gameObject.transform.GetChild(1).gameObject;
        Sprite powerUpIcon = powerUp.gameObject.GetComponent<SpriteRenderer>().sprite;

        background.GetComponent<Image>().sprite = powerUpIcon;
        fill.GetComponent<Image>().sprite = powerUpIcon;
    }

    //Sets the max value of the power up slider, then initializes the length to 1
    private void SetMaxDuration(float maxDuration) {
        if (powerUp.IsInfinite) {
            slider.maxValue = 1;
            slider.value = 1;
        } else {
            slider.maxValue = maxDuration;
            slider.value = maxDuration;
        }
    }

    //Updates the length of the power up bar based on the new duration left
    public void SetDurationLeft(float durationleft) {
        slider.value = durationleft;
    }
}
