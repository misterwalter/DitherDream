using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FPSCounter : MonoBehaviour
{
    public Text text;
    public int fps, expectedFps=30;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fps = Mathf.CeilToInt(1f/Time.deltaTime);
        text.text = fps!=expectedFps?fps + " fps":"";
    }
}
