﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LogToFileHelper : MonoBehaviour
{
    void SaveFile(string fileName, string data)
     {
        string destination = Application.persistentDataPath + fileName;
        if (File.Exists(destination))
        {
            Debug.Log(destination+" already exists.");
            var sr = File.AppendText(destination);
            var s = "";
            if (fileName.Contains(".json")){
                s = "{\"data\": " + data + ", \"timestamp\": {\"" + System.DateTime.Now + "\"}}";
            }
            else {
                s = data + " --- " + System.DateTime.Now;
            }
            sr.WriteLine(s);
            sr.Close();
        }
        else {
            var sr = File.CreateText(destination);
            var s = "";
            if (fileName.Contains(".json")){
                s = "{\"data\": " + data + ", \"timestamp\": {\"" + System.DateTime.Now + "\"}}";
            }
            else {
                s = data + " --- " + System.DateTime.Now;
            }
            sr.WriteLine(s);
            sr.Close();
        }
        
        
     }

    public IEnumerator LogToFileVector3Array(string file_name, Vector3[] arr)
    {
        while (true) {
            string s = "";
            foreach (Vector3 v in arr){
                s+= v + ", ";
            }
            if (file_name.Contains(".json")){
                s = "{\"" + s + "\"}";
            }
             yield return new WaitForSeconds(.1f);
             SaveFile(file_name, s);
         }
    }

    public IEnumerator LogToFileStringArray(string file_name, string[] arr)
    {
        while (true) {
            string s = "";
            foreach (string v in arr){
                s+= v + ", ";
            }
            if (file_name.Contains(".json")){
                s = "{\"" + s + "\"}";
            }
             yield return new WaitForSeconds(.1f);
             SaveFile(file_name, s);
         }
    }
    

    public IEnumerator LogToFile(string file_name, string _data)
    {
        while (true) {
             yield return new WaitForSeconds(.1f);
             SaveFile(file_name, _data);
         }
    }
}
