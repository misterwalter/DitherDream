using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
    Scatters objects more or less randomly, with a few filtering restrictions
 */
public class ObjectScatter : MonoBehaviour
{
    public ScatterPlacement[] scatterPlacements;
    public int scatterCount;
    public Transform scatterRoot;
    public Portal portal;
    public float spawnRadius;
    public static float maxSpawnHeight = 100f;
    public static float respawnFloor = -100f;

    [System.Serializable]
    public class ScatterPlacement {
        [Tooltip("Object type to spawn")]
        public GameObject objectType;
        [Tooltip("Maximum elevation to spawn at")]
        public float minElevation = 0;
        [Tooltip("Maximum elevation to spawn at")]
        public float maxElevation = 100;
        [Tooltip("Requires that the object be spawned on the ground instead of on another object - NOT FUNCTIONAL")]
        public bool requireOpenGround;
        public enum RotationOnSpawn {
            None,
            UprightSpin,
            RandomRotation
        };
        [Tooltip("Options for rotation setting on spawn.\nNone: Upright standard rotation.\nUpright: Spin around Y axis randomly.\nRandom: Fully random rotation on all 3 axis.")]
        public RotationOnSpawn rotationOnSpawn;
    }
    // Start is called before the first frame update
    void Start()
    {
        PlaceScatterables();
    }

    public void PlaceScatterables() {
        StartCoroutine(PrivatePlaceScatterables());
    }

    public IEnumerator PrivatePlaceScatterables() {
        // clear previous
        for (int i = scatterRoot.transform.childCount-1; i >= 0; --i) {
            Destroy(scatterRoot.transform.GetChild(i).gameObject);
        }
        portal = null;

        // Place portal - let physics and respawning sort it out
        Vector3 spawnPosition = Random.insideUnitSphere.normalized*spawnRadius/2;
        spawnPosition.y = maxSpawnHeight;
        portal = Instantiate(GameManager.instance.portalType, spawnPosition, Quaternion.identity, scatterRoot.transform).GetComponent<Portal>();


        // Scatter stuff!
        for (int i = 0; i < scatterPlacements.Length; ++i) {
            //Debug.Log("Attempting to place: " + scatterPlacements[i].objectType.transform.name);
            for (int c = 0; c < scatterCount; ++c) {
                spawnPosition = Random.insideUnitSphere.normalized*spawnRadius;
                spawnPosition.y = maxSpawnHeight;
                RaycastHit hit;
                //Debug.Log("Attempting to place: " + scatterPlacements[i].objectType.transform.name + " @ " + spawnPosition);
                if (Physics.Raycast(spawnPosition, Vector3.down, out hit)) {
                    if (hit.point.y < scatterPlacements[i].minElevation || hit.point.y > scatterPlacements[i].maxElevation) {
                        //Debug.Log("Wrong Elevation :: " + hit.point.y);
                        continue;
                    }
                    if (hit.transform.name != "Ground") {
                        //Debug.Log("Hit something other than the ground :: " + hit.transform.name);
                        continue;
                    }
                    GameObject newScat = Instantiate(scatterPlacements[i].objectType, hit.point, Quaternion.identity, scatterRoot.transform);
                    if (scatterPlacements[i].rotationOnSpawn == ScatterPlacement.RotationOnSpawn.UprightSpin) {
                        newScat.transform.localEulerAngles = Vector3.up*Random.Range(0,360);
                    } else if (scatterPlacements[i].rotationOnSpawn == ScatterPlacement.RotationOnSpawn.RandomRotation) {
                        newScat.transform.rotation = Random.rotation;
                    }
                }
                
            }
            yield return null;
        }
        yield return null;
    }
}

/*
    
 */
#if UNITY_EDITOR
[CustomEditor(typeof(ObjectScatter))]
public class ObjectScatterEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Place")) {
            ((ObjectScatter)target).PlaceScatterables();
        }
    }
}
#endif