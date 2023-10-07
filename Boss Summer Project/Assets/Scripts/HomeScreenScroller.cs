using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeScreenScroller : MonoBehaviour
{
    [SerializeField] private float speed;
    private float distanceTravelled = 0; 
    private BackgroundTiler backgroundTiler;
    // Start is called before the first frame update
    void Start()
    {
        backgroundTiler = GetComponent<BackgroundTiler>();
    }

    void Update()
    {
        float f = transform.position.x - speed * Time.deltaTime;
        CheckDistance();

        transform.position = new(f, 0, 0);
    }

    void CheckDistance()
    {
        distanceTravelled += speed * Time.deltaTime;
        if(distanceTravelled >= backgroundTiler.Width + 0.01)
        {
            distanceTravelled = 0;
            backgroundTiler.MoveTileRight();
        }
    }
}
