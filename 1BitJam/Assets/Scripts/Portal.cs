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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -100f) {
            Vector3 newPosition = Random.insideUnitSphere.normalized*GameManager.instance.currentIsland.spawnRadius;
            newPosition.y = ObjectScatter.maxSpawnHeight;
            transform.position = newPosition;
        }

        renderer.enabled = rb.velocity.magnitude < 1f;
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
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
