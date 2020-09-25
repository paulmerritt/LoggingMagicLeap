using UnityEngine;
using UnityEngine.XR.MagicLeap;
using MagicLeap.Core.StarterKit;
using TMPro;
using System;
    
    public class TrackAllPoses : MonoBehaviour
    {
        private const float CONFIDENCE_THRESHOLD = 0.95f;

        #pragma warning disable 414
        [SerializeField, Tooltip("KeyPose to track.")]
        private MLHandTracking.HandKeyPose _keyPoseToTrack = MLHandTracking.HandKeyPose.NoPose;
        #pragma warning restore 414

        [Space, SerializeField, Tooltip("Flag to specify if left hand should be tracked.")]
        private bool _trackLeftHand = true;

        [SerializeField, Tooltip("Flag to specify id right hand should be tracked.")]
        private bool _trackRightHand = true;

        private string[] cur_pose;
        private LogToConsoleHelper consoler = new LogToConsoleHelper();
        public string api_key = "pmerritt160fd12639ea467f88d9d4dfeee7b321";

	    public string ip_address = "192.168.7.163";

	    public int session_id = 0;

        /// <summary>
        /// Calls Start on MLHandTrackingStarterKit.
        /// </summary>
        void Start()
        {
            MLResult result = MLHandTrackingStarterKit.Start();

            #if PLATFORM_LUMIN
            if (!result.IsOk)
            {
                Debug.LogErrorFormat("Error: KeyPoseVisualizer failed on MLHandTrackingStarterKit.Start, disabling script. Reason: {0}", result);
                enabled = false;
                return;
            }
            #endif
            cur_pose = new string[2];
            LogToFileHelper logger = new LogToFileHelper();
            StartCoroutine(logger.LogToFileStringArray("log_poses.json", cur_pose));
            StartCoroutine(consoler.NewSession("http://"+ip_address+":57000/ext/"+ api_key + "/new_session"));
        }

        /// <summary>
        /// Clean up.
        /// </summary>
        void OnDestroy()
        {
            MLHandTrackingStarterKit.Stop();
        }

        /// <summary>
        /// Updates color of sprite renderer material based on confidence of the KeyPose.
        /// </summary>
        void Update()
        {
            float confidenceLeft =  0.0f;
            float confidenceRight = 0.0f;

            if (_trackLeftHand)
            {
                #if PLATFORM_LUMIN
                confidenceLeft = GetKeyPoseConfidence(MLHandTrackingStarterKit.Left);
                #endif
            }

            if (_trackRightHand)
            {
                #if PLATFORM_LUMIN
                confidenceRight = GetKeyPoseConfidence(MLHandTrackingStarterKit.Right);
                #endif
            }

            float confidenceValue = Mathf.Max(confidenceLeft, confidenceRight);
        }


        void PostPoseToConsole(string pose){
            try{
                LogToConsoleHelper.jsn_sent j = new LogToConsoleHelper.jsn_sent();
                 j.entry_id = 1;
			    j.message_data = "Pose is " + pose;
			    j.time_created = ""+System.DateTime.Now;
			    j.category = "External";
                
                 
                string s = "[" + JsonUtility.ToJson(j) + "]";

                StartCoroutine(consoler.PostRequest("http://"+ip_address+":57000/ext/"+ api_key+"/"+consoler.session_id, s));
                cur_pose[1] = consoler.receivedTextPost;
            }
            catch (Exception e){
                cur_pose[1] = e.ToString();
            }
        }

        #if PLATFORM_LUMIN
        /// <summary>
        /// Gets the confidence value for the hand being tracked.
        /// </summary>
        /// <param name="hand">Hand to check the confidence value on.</param>
        private float GetKeyPoseConfidence(MLHandTracking.Hand hand)
        {
            
            if (hand != null)
            {
                switch (hand.KeyPose){
                    case MLHandTracking.HandKeyPose.Finger:{
                        cur_pose[0] = "Finger";
                        GameObject.Find("DebugLogHandPose").GetComponent<TextMeshProUGUI>().text = cur_pose[0];
                        PostPoseToConsole(cur_pose[0]);
                        return hand.HandKeyPoseConfidence;
                        //break;
                    }
                    case MLHandTracking.HandKeyPose.Pinch:{
                        cur_pose[0] = "Pinch";
                        GameObject.Find("DebugLogHandPose").GetComponent<TextMeshProUGUI>().text = cur_pose[0];
                        PostPoseToConsole(cur_pose[0]);
                        return hand.HandKeyPoseConfidence;
                        //break;
                    }
                    case MLHandTracking.HandKeyPose.Thumb:{
                        cur_pose[0] = "Thumb";
                        GameObject.Find("DebugLogHandPose").GetComponent<TextMeshProUGUI>().text = cur_pose[0];
                        PostPoseToConsole(cur_pose[0]);
                        return hand.HandKeyPoseConfidence;
                        //break;
                    }
                    case MLHandTracking.HandKeyPose.L:{
                        cur_pose[0] = "L";
                        GameObject.Find("DebugLogHandPose").GetComponent<TextMeshProUGUI>().text = cur_pose[0];
                        PostPoseToConsole(cur_pose[0]);
                        return hand.HandKeyPoseConfidence;
                        //break;
                    }
                    case MLHandTracking.HandKeyPose.OpenHand:{
                        cur_pose[0] = "Open Hand";
                        GameObject.Find("DebugLogHandPose").GetComponent<TextMeshProUGUI>().text = cur_pose[0];
                        PostPoseToConsole(cur_pose[0]);
                        return hand.HandKeyPoseConfidence;
                        //break;
                    }
                    case MLHandTracking.HandKeyPose.Ok:{
                        cur_pose[0] = "Ok";
                        GameObject.Find("DebugLogHandPose").GetComponent<TextMeshProUGUI>().text = cur_pose[0];
                        PostPoseToConsole(cur_pose[0]);
                        return hand.HandKeyPoseConfidence;
                        //break;
                    }
                    case MLHandTracking.HandKeyPose.C:{
                        cur_pose[0] = "C";
                        GameObject.Find("DebugLogHandPose").GetComponent<TextMeshProUGUI>().text = cur_pose[0];
                        PostPoseToConsole(cur_pose[0]);
                        return hand.HandKeyPoseConfidence;
                        //break;
                    }
                    case MLHandTracking.HandKeyPose.NoPose:{
                        cur_pose[0] = "No Pose";
                        GameObject.Find("DebugLogHandPose").GetComponent<TextMeshProUGUI>().text = cur_pose[0];
                        PostPoseToConsole(cur_pose[0]);
                        return hand.HandKeyPoseConfidence;
                        //break;
                    }
                }
            }
            return 0.0f;
        }
        #endif
    }
    

