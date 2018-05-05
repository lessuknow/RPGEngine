using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour {

    private bool isRotation = false;
    float curAngle = 0;
    float curTime = 0;
    //Default rn is 0.35
    public float totalTime = 0.35f;
    private Quaternion origPos;

	// Use this for initialization
	void Awake () {
        origPos = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
            isRotation = true;
        if(isRotation)
            Rotate();
	}

    private void Rotate()
    {

        curAngle = 360 * curTime/totalTime;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, curAngle));
        if (curTime>=totalTime)
        {
            isRotation = false;
            transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            curAngle = 0;
            curTime = 0;
        }
        curTime += Time.deltaTime;
    }
}
