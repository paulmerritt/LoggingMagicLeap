using System.Collections;
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
            sr.WriteLine(data + " --- " +  System.DateTime.Now);
            sr.Close();
        }
        else {
            var sr = File.CreateText(destination);
            sr.WriteLine(data + " --- " +  System.DateTime.Now);
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
