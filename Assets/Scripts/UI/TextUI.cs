﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour {

    [SerializeField] private Text txt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(string newText)
    {
        txt.text = newText;
    }

}
