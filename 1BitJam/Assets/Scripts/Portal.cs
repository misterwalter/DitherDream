using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Portal that the player is seeking to reach, that resets the level when touched
 */
public class Portal : MonoBehaviour
{
    public Rigidbody rb;
    public Renderer renderer;

    // Update is called once per frame
    void Update()
    {
        // Respawn portal if/when it falls down past the island
        // It's easier than sampling a valid spot, and in this specific case in this specific game it really doesn't matter if it shows up late.
        if (transform.position.y < ObjectScatter.respawnFloor) {
            Vector3 newPosition = Random.insideUnitSphere.normalized*GameManager.instance.currentIsland.spawnRadius;
            newPosition.y = ObjectScatter.maxSpawnHeight;
            transform.position = newPosition;
            renderer.enabled = false;   // make sure it's not visible for a single frame at the start;
        } else renderer.enabled = rb.velocity.magnitude < 1f;   // conceal portal while it falls
    }


    /*
        The portal activates the next level in the sequence when touched by the player
        And destroys other objects that it lands on so that it won't end up stuck in a tree and inaccessible
        This must be reconsidered if we have more intricate architecture in the future
     */
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<FirstPersonMovement>()) {
            Debug.Log("RESTART LEVEL, GOOD JOB!");
            GameManager.CompleteLevel();
        } else if (other.gameObject.transform.name != "Ground") {
            Destroy(other.gameObject);  // crash to the ground
        }
        
    }
}
