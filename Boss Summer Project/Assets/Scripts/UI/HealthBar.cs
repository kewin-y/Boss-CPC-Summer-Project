using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image fill;

    //Sets the max value of the health slider, then initializes the colour and length
    public void SetMaxHealth(float health) {
        slider.maxValue = 2 * health;
        slider.value = health;

        //Set the health bar to "max health" colour (rightmost colour of the health gradient)
        fill.color = healthGradient.Evaluate(1f);
    }

    //Updates the colour and length of the health bar based on the new health
    public void SetHealth(float health) {
        slider.value = health;
        //Commented this out so that the health bar retains its opacity as its value changes
        //fill.color = healthGradient.Evaluate(slider.normalizedValue);
    }
}
