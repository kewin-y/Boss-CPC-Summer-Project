using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private int maxDurability;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform player;

    private int durability;
    public int Durability {
        get { return durability; }
        set { durability = value; }
    }

    void Start() {
        gameObject.SetActive(false);
    }

    //
    void OnEnable()
    {
        durability = maxDurability;
    }

    // 
    private void Update()
    {
        FollowPlayer();
        PointToMouse();

        if (durability <= 0) {
            gameObject.SetActive(false);
        }
    }

    //Makes the shield follow the player
    private void FollowPlayer() {
        transform.position = player.position;
    }

    //Rotates the shield to face towards the mouse position
    private void PointToMouse() {
        //Get the mouse position, disregarding the -10 z-axis offset of the main camera
        Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        //Point the shield to the mouse
        transform.right = mousePosition - transform.position;
    }

    public void TakeDamage(int damage) {
        durability -= damage;
    }
}
