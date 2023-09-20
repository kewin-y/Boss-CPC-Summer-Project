using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private Transform player;
    //When the sword is summoned, reset to max health and restore normal colour
    //Also, receive the sword script that is currently in effect so the sword health bar can be updated.
    
    public void Initialize() {
        VisualEffects.SetColor(gameObject, Color.white);
    }

    //For every frame that the player has the sword, follow the player and rotate towards the mouse
    void Update()
    {

    }

    //Makes the sword follow the player
    //Rotates the sword to face towards the mouse position
    private void PointToMouse() {

        
    }

    //Implement durability for the sword
    
}
