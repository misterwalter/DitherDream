using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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
        if (playerObject.transform.position.y < -100 || Input.GetKeyDown(KeyCode.Backspace)) {
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

    public static void CompleteLevel() {
        instance.ResetLevel();
    }

    public void ResetLevel() {
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

    public static void RespawnPlayer() {
        Vector3 playerSpawn = Random.insideUnitSphere*instance.currentIsland.spawnRadius*.75f;
        playerSpawn.y = ObjectScatter.maxSpawnHeight;
        instance.playerObject.transform.position = playerSpawn;
    }
}
