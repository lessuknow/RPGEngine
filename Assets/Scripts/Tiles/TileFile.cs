using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TileFile : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        string readPath = Application.dataPath + "/testRead.txt";
        string writePath = Application.dataPath + "/testWrite.txt";
        print(writePath);

        StreamWriter sw;
        if(!File.Exists(writePath))
        {
            sw = File.CreateText(writePath);
        }
        else
        {
            sw = new StreamWriter(writePath);
        }

        sw.WriteLine("Buzz");
        sw.Close();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
