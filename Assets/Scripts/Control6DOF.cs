using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using TMPro;
using UnityEngine.UI;

//used for the laser pointer

public class Control6DOF : MonoBehaviour {
    #region Private Variables
    
    private const float _triggerThreshold = 0.2f;


    #endregion

    #region Unity Methods
    public MLInput.Controller _controller;
    public bool _triggerPressed = false;

    private string[] text;

    void Start () {
        //Start receiving input by the Control
        MLInput.Start();
        _controller = MLInput.GetController(MLInput.Hand.Left);
        text = new string[3];
        LogToFileHelper logger = new LogToFileHelper();
        StartCoroutine(logger.LogToFileStringArray("log_controller.txt", text));
    }
    void OnDestroy () {
        //Stop receiving input by the Control
        MLInput.Stop();
    }    
    void FixedUpdate () {
        //Attach the Beam GameObject to the Control
        transform.position = _controller.Position;
        transform.rotation = _controller.Orientation;    
        


        //if trigger value goes above the threshold, it was pressed
        if (_controller.TriggerValue > _triggerThreshold) {
            _triggerPressed = true;

        }
        //if trigger is pressed and the value is back to 0, it's not pressed
        else if (_controller.TriggerValue == 0.0f && _triggerPressed) {
            _triggerPressed = false;

        }

        text[0] = "Controller Position: " + transform.position;        
        text[1] = "Controller Rotation: " + transform.rotation;        
        text[2] = "Trigger Pressed: " + _triggerPressed;        

        GameObject.Find("DebugLogController").GetComponent<TextMeshProUGUI>().text = text[0] + "\n" + text[1] + "\n" + text[2];
        

    }
    #endregion
    
}