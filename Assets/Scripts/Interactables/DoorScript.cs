using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorScript : Interact_Base {

    private bool moving = false, open = false;
    private float lerpNum;
    public TileManager tm;
    private Vector3 endPos;

    public bool getOpen()
    {
        return open;
    }

    public override void Toggle()
    {
        lerpNum = 0;
        moving = true;

        if (open == false)
        { 
            endPos = transform.position;
            endPos.y -= transform.transform.localScale.y;
            //This is so it doesn't glitch out the floor tile.
            endPos.y -= 0.01f;
        }
        tm.DisableDoor(transform.position); 
    }
	
	// Update is called once per frame
	void Update () {
		if(moving == true && open == false)
        {
            if(lerpNum <1)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, lerpNum);
                lerpNum += Time.deltaTime;
            }
            else
            {
                open = true;
                lerpNum = 0;
                moving = false;
            }
        }
	}
}
