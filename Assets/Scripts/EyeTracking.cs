using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using TMPro;

public class EyeTracking : MonoBehaviour {
    #region Public Variables
    public GameObject Camera;
    public Material FocusedMaterial, NonFocusedMaterial;
    public string ip_address = "192.168.7.163";
    public string api_key = "pmerritt160fd12639ea467f88d9d4dfeee7b321";
    #endregion

    #region Private Variables
    private Vector3 _heading;
    private MeshRenderer _meshRenderer;
    #endregion

    private Vector3[] eyePos;
    private LogToConsoleHelper consoler = new LogToConsoleHelper();
    #region Unity Methods
    void Start() {
        MLEyes.Start();
        transform.position = Camera.transform.position + Camera.transform.forward * 5.0f;
		// Get the meshRenderer component
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        eyePos = new Vector3[1];
        LogToFileHelper logger = new LogToFileHelper();
        StartCoroutine(logger.LogToFileVector3Array("log_eye.json", eyePos));
        StartCoroutine(consoler.NewSession("http://"+ip_address+":57000/ext/"+ api_key + "/new_session"));
    }    
    private void OnDisable() {
        MLEyes.Stop();
    }
    void Update() {
        if (MLEyes.IsStarted) {
            RaycastHit rayHit;
            eyePos[0] = MLEyes.FixationPoint;
            _heading = MLEyes.FixationPoint - Camera.transform.position;
            LogToConsoleHelper.jsn_sent j = new LogToConsoleHelper.jsn_sent();
            j.entry_id = 1;
            j.message_data = "eye position: " + eyePos[0];
            j.time_created = ""+System.DateTime.Now;
            j.category = "External";
            

            string s = "[" + JsonUtility.ToJson(j) + "]";
            if (consoler.session_id != 0){
                StartCoroutine(consoler.PostRequest("http://"+ip_address+":57000/ext/"+ api_key+"/"+consoler.session_id, s));
            }
            GameObject.Find("DebugLogEyes").GetComponent<TextMeshProUGUI>().text = "Eyes are looking at: " + eyePos[0];
            
            // Use the proper material
            if (Physics.Raycast(Camera.transform.position, _heading, out rayHit, 10.0f)) {
                _meshRenderer.material = FocusedMaterial;
            }
            else {
                _meshRenderer.material = NonFocusedMaterial; 
            }

        }
    }
    #endregion
}