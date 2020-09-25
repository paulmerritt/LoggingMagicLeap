using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using System;

public class TouchpadGestures : MonoBehaviour {

    #region Public Variables
    public Text typeText, stateText, directionText;
    private string[] text;
    public Camera Camera;
    #endregion

    #region Private Variables
    private MLInput.Controller _controller;
    #endregion

    private LogToConsoleHelper consoler = new LogToConsoleHelper();
    public string api_key = "pmerritt160fd12639ea467f88d9d4dfeee7b321";

    public string ip_address = "192.168.7.163";

	public int session_id = 0;

    #region Unity Methods
    void Start() {
        MLInput.Start();
        _controller = MLInput.GetController(MLInput.Hand.Left);
        text = new string[4];
        text[0] = "default"; 
        text[1] = "default"; 
        text[2] = "default";
        text[3] = "default";
        LogToFileHelper logger = new LogToFileHelper();
        
        StartCoroutine(logger.LogToFileStringArray("log_touchpad.json", text));
        StartCoroutine(consoler.NewSession("http://"+ip_address+":57000/ext/"+ api_key + "/new_session"));
    }

    void OnDestroy() {
        MLInput.Stop();
    }            

    void Update() {
        updateTransform();
        updateGestureText();
        GameObject.Find("DebugLogTouchpad").GetComponent<TextMeshProUGUI>().text = text[0] + text[1] + text[2];
    }
    #endregion

    #region Private Methods
    void updateGestureText() {
        string gestureType = _controller.CurrentTouchpadGesture.Type.ToString();
        string gestureState = _controller.TouchpadGestureState.ToString();
        string gestureDirection = _controller.CurrentTouchpadGesture.Direction.ToString();

        typeText.text = "Type: " + gestureType;
        text[0] = typeText.text;
        stateText.text = "State: " + gestureState;
        text[1] = stateText.text;
        directionText.text = "Direction: " + gestureDirection;
        text[2] = directionText.text;

        try{
                LogToConsoleHelper.jsn_sent j = new LogToConsoleHelper.jsn_sent();
                j.entry_id = 1;
			    j.message_data = text[0] + text[1] + text[2];
			    j.time_created = ""+System.DateTime.Now;
			    j.category = "External";
                 
                string s = "[" + JsonUtility.ToJson(j) + "]";

                StartCoroutine(consoler.PostRequest("http://"+ip_address+":57000/ext/"+ api_key+"/"+consoler.session_id, s));
                text[3] = consoler.receivedTextPost;
            }
            catch (Exception e){
                text[3] = e.ToString();
            }
    }
    
    void updateTransform() {
        float speed = Time.deltaTime * 5.0f;

        Vector3 pos = Camera.transform.position + Camera.transform.forward;
        gameObject.transform.position = Vector3.SlerpUnclamped(gameObject.transform.position, pos, speed);

        Quaternion rot = Quaternion.LookRotation(gameObject.transform.position - Camera.transform.position);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, rot, speed);
    }
    #endregion
}