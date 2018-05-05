using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decklist : MonoBehaviour {

    List<Card_Base> contents;

	// Use this for initialization
	void Start () {

        contents = new List<Card_Base>();

        Card_Base card = new Two_Of_Clubs();

        contents.Add(card);
        //our deck has literally one card rn...
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
