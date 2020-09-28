using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.Assertions;
using Newtonsoft.Json;
using System.Linq;

//used for the laser pointer

public class Control6DOF : MonoBehaviour {
    #region Private Variables
    
    private const float _triggerThreshold = 0.2f;
    private MLInput.Controller _controller;
    private string[] text;

    #endregion

    public string api_key = "pmerritt160fd12639ea467f88d9d4dfeee7b321";
	public string ip_address = "192.168.7.163";

    
    public bool _triggerPressed = false;
    public bool _bumperPressed = false;
    public bool _homePressed = false;
    private LogToConsoleHelper consoler = new LogToConsoleHelper();

    #region Unity Methods
    void Start()
    {
        MLInput.Start();
        MLInput.OnControllerButtonDown += OnButtonDown;
        MLInput.OnControllerButtonUp += OnButtonUp;
        _controller = MLInput.GetController(MLInput.Hand.Left);
        text = new string[6];
        LogToFileHelper logger = new LogToFileHelper();
        //LogToConsoleHelper consoler = new LogToConsoleHelper();
        StartCoroutine(logger.LogToFileStringArray("log_controller.json", text));
        StartCoroutine(consoler.NewSession("http://"+ip_address+":57000/ext/"+ api_key + "/new_session"));
    }   


    void OnButtonDown(byte controller_id, MLInput.Controller.Button button)
    {
        if (button == MLInput.Controller.Button.HomeTap)
        {
            _homePressed = true;
        }
        if (button == MLInput.Controller.Button.Bumper)
        {
            _bumperPressed = true;
        }
        
    }

    void OnButtonUp(byte controllerId, MLInput.Controller.Button button) {
        if (button == MLInput.Controller.Button.HomeTap)
        {
           _homePressed = false;
        }
        if (button == MLInput.Controller.Button.Bumper)
        {
            _bumperPressed = false;
        }
    }
    

    void Update () {
        //Attach the Beam GameObject to the Control
        transform.position = _controller.Position;
        transform.rotation = _controller.Orientation;    
        
        

        //if trigger value goes above the threshold, it was pressed
        if (_controller.TriggerValue > _triggerThreshold) {
            
            try{
                if(_triggerPressed == false ){
                    LogToConsoleHelper.jsn_sent j = new LogToConsoleHelper.jsn_sent();
                    j.entry_id = 1;
			        j.message_data = "BUTTON TRIGGER";
			        j.time_created = ""+System.DateTime.Now;
			        j.category = "External";
                    

                    string s = "[" + JsonUtility.ToJson(j) + "]";
                    if (consoler.session_id != 0){
                        StartCoroutine(consoler.PostRequest("http://"+ip_address+":57000/ext/"+ api_key+"/"+consoler.session_id, s));
                    }
                    
                    //text[5] = consoler.receivedTextPost;
                }
                
            }catch(Exception e){
                Debug.Log("ohp something went wrong");
                Debug.Log(e.ToString());
                //text[5] = "error: " + e.ToString();
            }
            _triggerPressed = true;

        }
        //if trigger is pressed and the value is back to 0, it's not pressed
        else if (_controller.TriggerValue == 0.0f && _triggerPressed) {
            _triggerPressed = false;
            //text[5] = "let go of trigger";

        }

        text[0] = "Controller Position: " + transform.position;        
        text[1] = "Controller Rotation: " + transform.rotation;        
        text[2] = "Trigger Pressed: " + _triggerPressed;       
        text[3] = "Bumper Pressed: " + _bumperPressed; 
        text[4] = "Home Pressed: " + _homePressed;
        text[5] = "session: " + consoler.session_id;
        

        GameObject.Find("DebugLogController").GetComponent<TextMeshProUGUI>().text = text[0] + "\n" + text[1] + "\n" + text[2] + "\n" + text[3] + "\n" + text[4]+ "\n" + text[5];
        

    }

    void OnDestroy()
    {
        MLInput.Stop();
        MLInput.OnControllerButtonDown -= OnButtonDown;
        MLInput.OnControllerButtonUp -= OnButtonUp;
    }

    
    #endregion
    
}



