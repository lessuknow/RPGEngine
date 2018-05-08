using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Hover : MonoBehaviour {

    private float floatDiff = 0.07f;
    private float origY;
    private float speedMod = 0.01f;
    private bool floatUp = false;
    private Vector3 max, min,orig;
	// Use this for initialization
	void Start () {
        origY = transform.position.y;
        orig = transform.position;
        max = new Vector3(transform.position.x,
            transform.position.y + floatDiff,
            transform.position.z);
        min = new Vector3(transform.position.x,
            transform.position.y - floatDiff,
            transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y+.03f >= origY + floatDiff)
            floatUp = false;
        else if (transform.position.y - .03f <= origY - floatDiff)
            floatUp = true;
		if(floatUp)
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y + Time.deltaTime * speedMod,
                transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y - Time.deltaTime * speedMod,
                transform.position.z);
        }
    }
}
