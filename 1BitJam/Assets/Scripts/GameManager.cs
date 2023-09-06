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
    public Island tutorialIslandType;
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

        // Spawn tutorial island
        currentIsland = Instantiate(tutorialIslandType).GetComponent<Island>();
        RespawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (currentIsland) {
            Debug.Log("Destroying old island: " + currentIsland.transform.name);
            Destroy(currentIsland.gameObject);
        }
        

        // spawn new island
        nextIsland = islandIndex%islandTypes.Length;
        Debug.Log("Island index == " + islandIndex + " :: " + nextIsland);
        currentIsland = Instantiate(islandTypes[nextIsland]).GetComponent<Island>();
        Debug.Log("Welcome to the new island: " + currentIsland.transform.name);

        // place player
        RespawnPlayer();
    }

    /*
        Bring the player back up to randomly fall down onto the island, resetting them
     */
    public static void RespawnPlayer() {
        Debug.Log("Respawning player");
        if (instance.currentIsland.playerStart) {
            instance.playerObject.transform.position = instance.currentIsland.playerStart.position;
            instance.playerObject.transform.rotation = instance.currentIsland.playerStart.rotation;
        } else {
            Vector3 playerSpawn = Random.insideUnitSphere*instance.currentIsland.spawnRadius*.75f;
            playerSpawn.y = Island.maxSpawnHeight;
            instance.playerObject.transform.position = playerSpawn;
        }
    }
}
