using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CI.HttpClient;
//using UnityEngine.XR.WSA.Input;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.Assertions;
using Newtonsoft.Json;
using System.Linq;
using System;
using TMPro;

public class TestHTTPPost : MonoBehaviour
{

    public string api_key = "pmerritt160fd12639ea467f88d9d4dfeee7b321";
	
	bool done = false;

	public string ip_address = "192.168.7.163";

	public int session_id = 0;

    private bool tested_new_session = false;
	private bool post_request_finished_flag = false;
	private bool get_request_finished_flag = false;

	private string receivedTextPost = "";
	private string receivedTextGet = "";
	private IEnumerator i = null;

    // Start is called before the first frame update
    void Start () {
		StartCoroutine(PostRequest("http://"+ip_address+":57000/ext/"+ api_key + "/new_session", "test"));
	}

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Text").GetComponent<TextMeshProUGUI>().text = receivedTextPost + "\n" + session_id;
    }


    public IEnumerator PostRequest(string url, string json)
	 {
		 //post_request_finished_flag = false;
	     var uwr = new UnityWebRequest(url, "POST");
	     byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
	     uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
	     uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
	     uwr.SetRequestHeader("Content-Type", "application/json");

	     //Send the request then wait here until it returns
	     uwr.SendWebRequest();

	     if (uwr.isNetworkError)
	     {
	         Debug.Log("Error While Sending: " + uwr.error);
             receivedTextPost = uwr.error;
	     }
	     else
	     {
			 Debug.Log(uwr.downloadHandler.text);
			 receivedTextPost = uwr.downloadHandler.text;
			 session_id = GetSessionID(uwr.downloadHandler.text);
	     }
		 
		 
		Debug.Log("waiting");
		 yield return new WaitForSeconds(3f);
		 Debug.Log("waited");
		 uwr.Dispose();

		TestIfPosted();
		 
		yield break;
	 }

     void TestIfPosted(){
		Debug.Log("Testing if posted to session");
		if (receivedTextPost != "" && post_request_finished_flag){
			Debug.Log("Test passed, posted to session and received: " + receivedTextPost);
		}
		else {
			Debug.Log("Test failed, unable to post to session");
			Debug.Log(post_request_finished_flag);
		}
	 }

     int GetSessionID(string s){
		 int id = session_id;
		 if (s.Contains("New Session ID")){
				 List<int> indices = new List<int>();
				 indices = FindEachIndex('\'', s);
				 id = int.Parse(s.Substring(indices[0], indices[1]-indices[0]-1));
			}
			else {
				
			}
			return id;
	 }

	 List<int> FindEachIndex(char c, string s){
		 int i = 0;
		 List<int> result = new List<int>();
		  foreach (char cc in s)
		{
			i++;
			if (c == cc){
				result.Add(i);
			}
		}
		return result;
	 }

    public class jsn_sent{
    	public int id;
    	public string timestamp;
    	public string category;
    	public string message_data;
    }
	public class jsn_received{
		public string value;
		public string time_created;
	}
}
