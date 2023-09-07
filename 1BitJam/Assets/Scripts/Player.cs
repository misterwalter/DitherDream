using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Supplements easy asset store character controller with DD specific logic.
 */
public class Player : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        // Respawn player if they fall off - or if debug key is pressed
        if (transform.position.y < Island.respawnFloor || Input.GetKeyDown(KeyCode.Backspace)) {
            GameManager.RespawnPlayer();
        }
    }
}
