using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Island colorSampleIsland;
    public int pixelFactor;
    // Start is called before the first frame update
    void Start()
    {
        colorSampleIsland.SetRenderSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        SceneManager.LoadScene("Main Scene");
    }
}
