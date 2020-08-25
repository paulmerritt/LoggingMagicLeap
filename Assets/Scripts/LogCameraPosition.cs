using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogCameraPosition : MonoBehaviour
{
    // Start is called before the first frame update
    private string[] text;
    void Start()
    {
        text = new string[2];
        LogToFileHelper logger = new LogToFileHelper();
        StartCoroutine(logger.LogToFileStringArray("log_camera.json", text));
    }

    // Update is called once per frame
    void Update()
    {
        text[0] = "" + GameObject.Find("Main Camera").transform.position;
        text[1] = "" + GameObject.Find("Main Camera").transform.rotation;
        GameObject.Find("DebugLogCamera").GetComponent<TextMeshProUGUI>().text = "Camera position: " + GameObject.Find("Main Camera").transform.position;
    }
}
