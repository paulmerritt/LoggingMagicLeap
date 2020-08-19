using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using TMPro;

public class EyeTracking : MonoBehaviour {
    #region Public Variables
    public GameObject Camera;
    public Material FocusedMaterial, NonFocusedMaterial;
    #endregion

    #region Private Variables
    private Vector3 _heading;
    private MeshRenderer _meshRenderer;
    #endregion

    private Vector3[] eyePos;

    #region Unity Methods
    void Start() {
        MLEyes.Start();
        transform.position = Camera.transform.position + Camera.transform.forward * 5.0f;
		// Get the meshRenderer component
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        eyePos = new Vector3[1];
        LogToFileHelper logger = new LogToFileHelper();
        StartCoroutine(logger.LogToFileVector3Array("log_eyes.txt", eyePos));
    }    
    private void OnDisable() {
        MLEyes.Stop();
    }
    void Update() {
        if (MLEyes.IsStarted) {
            RaycastHit rayHit;
            eyePos[0] = MLEyes.FixationPoint;
            _heading = MLEyes.FixationPoint - Camera.transform.position;
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