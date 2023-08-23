using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Overall game controller - flows between levels, exits, etc, etc
 */
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Tooltip("The prefab to spawn in as a portal once per island")]
    public GameObject portalType;

    public int islandIndex = -1;    // start at tutorial, then go to zero, then count up forever
    public int nextIsland;
    public Island[] islandTypes;
    public Island currentIsland;

    [Tooltip("Layermask that should only select terrain")]
    public LayerMask terrainMask;
    public GameObject playerObject;

    [Header("Exit UI")]
    public Image exitX;
    public Image exitBackground;
    public float exitDelay, exitTimer;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Respawn player if they fall off.
        if (playerObject.transform.position.y < ObjectScatter.respawnFloor || Input.GetKeyDown(KeyCode.Backspace)) {
            RespawnPlayer();
        }

        // Exit UI
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Tab)) {
            exitTimer += Time.deltaTime;
            if (exitTimer > 1) Application.Quit();
        } else {
            exitTimer -= Time.deltaTime/exitDelay;
            if (exitTimer < 0) exitTimer = 0;
        }
            exitBackground.fillAmount = exitTimer;
            exitX.fillAmount = exitTimer;
    }

    /*
        Marks the completion of the current level...I thought there would be more here
     */
    public static void CompleteLevel() {
        instance.GoToNextLevel();
    }

    /*
        Transitions the game to the next level in the cycle, resetting everything as needed
     */
    public void GoToNextLevel() {
        ++islandIndex;

        //clear away old island
        Destroy(currentIsland.gameObject);

        // spawn new island
        nextIsland = islandIndex%islandTypes.Length;
        Debug.Log("Island index == " + islandIndex + " :: " + nextIsland);
        currentIsland = Instantiate(islandTypes[nextIsland]).GetComponent<Island>();

        // place player
        RespawnPlayer();
    }

    /*
        Bring the player back up to randomly fall down onto the island, resetting them
     */
    public static void RespawnPlayer() {
        Vector3 playerSpawn = Random.insideUnitSphere*instance.currentIsland.spawnRadius*.75f;
        playerSpawn.y = ObjectScatter.maxSpawnHeight;
        instance.playerObject.transform.position = playerSpawn;
    }
}
