using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LogCameraPosition : MonoBehaviour
{
    // Start is called before the first frame update
    private string[] text;
    private LogToConsoleHelper consoler = new LogToConsoleHelper();
    public string api_key = "pmerritt160fd12639ea467f88d9d4dfeee7b321";

	public string ip_address = "192.168.7.163";

	public int session_id = 0;
    void Start()
    {
        text = new string[3];
        LogToFileHelper logger = new LogToFileHelper();
        StartCoroutine(logger.LogToFileStringArray("log_camera.json", text));
        StartCoroutine(consoler.NewSession("http://"+ip_address+":57000/ext/"+ api_key + "/new_session"));
    }

    // Update is called once per frame
    void Update()
    {
        text[0] = "" + GameObject.Find("Main Camera").transform.position;
        text[1] = "" + GameObject.Find("Main Camera").transform.rotation;
        try{
                LogToConsoleHelper.jsn_sent j = new LogToConsoleHelper.jsn_sent();
                j.entry_id = 1;
			    j.message_data = text[0];
			    j.time_created = ""+System.DateTime.Now;
			    j.category = "External";
                string s = "[" + JsonUtility.ToJson(j) + "]";

               if (consoler.session_id != 0){
                        StartCoroutine(consoler.PostRequest("http://"+ip_address+":57000/ext/"+ api_key+"/"+consoler.session_id, s));
                }
                text[3] = consoler.receivedTextPost;
            }
            catch (Exception e){
                text[3] = e.ToString();
            }
        GameObject.Find("DebugLogCamera").GetComponent<TextMeshProUGUI>().text = "Camera position: " + GameObject.Find("Main Camera").transform.position;
    }
}
