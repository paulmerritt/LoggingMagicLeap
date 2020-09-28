using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using TMPro;


public class HandTrackScript : MonoBehaviour
{
    public enum HandPoses { Ok, Finger, Thumb, OpenHand, Fist, NoPose };
    public HandPoses pose = HandPoses.NoPose;
    public Vector3[] pos;
    public GameObject sphereThumb, sphereIndex, sphereWrist;
    public string ip_address = "192.168.7.163";
    public string api_key = "pmerritt160fd12639ea467f88d9d4dfeee7b321";
    private LogToConsoleHelper consoler = new LogToConsoleHelper();
    
    private MLHandTracking.HandKeyPose[] _gestures;

    private void Start()
    {
        MLHandTracking.Start();
        _gestures = new MLHandTracking.HandKeyPose[5];
        _gestures[0] = MLHandTracking.HandKeyPose.Ok;
        _gestures[1] = MLHandTracking.HandKeyPose.Finger;
        _gestures[2] = MLHandTracking.HandKeyPose.OpenHand;
        _gestures[3] = MLHandTracking.HandKeyPose.Fist;
        _gestures[4] = MLHandTracking.HandKeyPose.Thumb;
        MLHandTracking.KeyPoseManager.EnableKeyPoses(_gestures, true, false);
        pos = new Vector3[3];
        LogToFileHelper logger = new LogToFileHelper();
        StartCoroutine(logger.LogToFileVector3Array("log_hands.json", pos));
        StartCoroutine(consoler.NewSession("http://"+ip_address+":57000/ext/"+ api_key + "/new_session"));
    }
    private void OnDestroy()
    {
        MLHandTracking.Stop();
    }


    private void Update()
    {
        if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Ok))
        {
            pose = HandPoses.Ok;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Finger))
        {
            pose = HandPoses.Finger;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.OpenHand))
        {
            pose = HandPoses.OpenHand;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Fist))
        {
            pose = HandPoses.Fist;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Thumb))
        {
            pose = HandPoses.Thumb;
        }
        else
        {
            pose = HandPoses.NoPose;
        }

        if (pose != HandPoses.NoPose) ShowPoints();

        //GameObject.Find("DebugLogHands").GetComponent<TextMeshProUGUI>().text = "Hands' pose are: " + pose;   
        GameObject.Find("DebugLogHands").GetComponent<TextMeshProUGUI>().text = "Hands' position are:\n" + pos[0] + "\n" + pos[1] + "\n" + pos[2];
        LogToConsoleHelper.jsn_sent j = new LogToConsoleHelper.jsn_sent();
        j.entry_id = 1;
        j.message_data = "hand position: " + pos[0];
        j.time_created = ""+System.DateTime.Now;
        j.category = "External";
        

        string s = "[" + JsonUtility.ToJson(j) + "]";
        if (consoler.session_id != 0){
            StartCoroutine(consoler.PostRequest("http://"+ip_address+":57000/ext/"+ api_key+"/"+consoler.session_id, s));
        }
    }


    private void ShowPoints()
    {
        // Left Hand Thumb tip
        pos[0] = MLHandTracking.Left.Thumb.KeyPoints[2].Position;
        // Left Hand Index finger tip 
        pos[1] = MLHandTracking.Left.Index.KeyPoints[2].Position;
        // Left Hand Wrist 
        pos[2] = MLHandTracking.Left.Wrist.KeyPoints[0].Position;
        sphereThumb.transform.position = pos[0];
        sphereIndex.transform.position = pos[1];
        sphereWrist.transform.position = pos[2];
    }

    private bool GetGesture(MLHandTracking.Hand hand, MLHandTracking.HandKeyPose type)
    {
        if (hand != null)
        {
            if (hand.KeyPose == type)
            {
                if (hand.HandKeyPoseConfidence > 0.9f)
                {
                    return true;
                }
            }
        }
        return false;
    }

}
