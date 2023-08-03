using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpBar : MonoBehaviour
{
    [SerializeField] private GameObject powerUpBar;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;

    private PowerUp powerUp;

    public PowerUpBar(PowerUp powerUp) {
        this.powerUp = powerUp;
    }

    // Start is called before the first frame update
    void Start()
    {
        powerUpBar.GetComponent<Image>().sprite = powerUp.gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
