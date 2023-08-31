using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashMeter : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        print(slider);
        slider.value = slider.maxValue;
    }

    public void SetDefaultValue()
    {
        StopAllCoroutines();
        slider.value = slider.maxValue;
    }

    public IEnumerator StartSequence(float cooldown)
    {
        slider.value = slider.minValue;

        float timeElapsed = 0;

        while (timeElapsed < cooldown) {
            timeElapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, timeElapsed/cooldown);
            yield return null;
        }

        slider.value = slider.maxValue;
    }

    
}
