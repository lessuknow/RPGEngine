using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This code handles actually moving the enemies on the Tilemap.
public class Mob_Movement : MonoBehaviour {

    private enum AI { _chase };
    private AI ai;
    public Direction curDir = Direction.South;
    public enum Direction { North, East, South, West };
    public AStar astar;
    public GameObject target;
    public TileManager tm;
    private bool moving, rotating;
    private Vector3 endPos;
    private Quaternion endRotate;
    private float moveLerpTime = 0, moveSpeed = 5, 
        rotateLerpTime = 0, rotateSpeed = 5;
    float tileSize = 1.33f;

    // Use this for initialization
    void Start () {
        
        ai = AI._chase;

        //RN hardcoded to instantly turn souht.
        endRotate.eulerAngles = new Vector3(0, 0, 0);
        curDir = Direction.South;
        rotating = true;
    }

    //This should change the rotation of the Mob based on the next position it's moving to.
    private void SetRotation(Vector3 MoveTo)
    {
        print(transform.position - MoveTo);
        
        //If it is moving down
        if(transform.position[2] - MoveTo[2] > 0.1)
        {
            if(curDir != Direction.South)
            {
                //Turn south.
                endRotate.eulerAngles = new Vector3(0, 0, 0);
                curDir = Direction.South;
                rotating = true;
            }
        }
        //Else if it is moving up
        else if(transform.position[2] - MoveTo[2] < -0.1)
        {
            if(curDir != Direction.North)
            {
                //Turn North.
                endRotate.eulerAngles = new Vector3(0, 180, 0);
                curDir = Direction.North;
                rotating = true;

            }
        }
        //Goes right
        else if (transform.position[0] - MoveTo[0] > 0.1)
        {
            if (curDir != Direction.West)
            {
                //Turn west.
                endRotate.eulerAngles = new Vector3(0, 90, 0);
                curDir = Direction.West;
                rotating = true;

            }

        }
        else if (transform.position[0] - MoveTo[0] < -0.1)
        {
            if (curDir != Direction.East)
            {
                //Turn East.
                endRotate.eulerAngles = new Vector3(0, 270, 0);
                curDir = Direction.East;
                rotating = true;

            }

        }

    }

    public void Move()
    {
        //We're goingt o change this later... Def. need to refactor.
        List<Vector3> nextMove = astar.FindPath(transform.position, target.transform.position);
        //Note: Add a seperate FindPath function for just the next one.
        if (nextMove.Count > 0)
        {
            tm.MoveEnemy(transform.position, nextMove[0]);

            SetRotation(nextMove[0]);

            moveLerpTime = 0;

            endPos[0] = nextMove[0][0];
            endPos[1] = transform.position.y;
            endPos[2] = nextMove[0][2];
            moving = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Handle the actual movement/Rotation lerps. We want to rotate first, then move.
        if (rotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, endRotate, rotateLerpTime);
            rotateLerpTime += Time.deltaTime * rotateSpeed;
            if (rotateLerpTime >= 1)
            {
                transform.rotation = endRotate;
                rotateLerpTime = 0;
                rotating = false;
            }
        }
        else if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, moveLerpTime);
            moveLerpTime += Time.deltaTime * moveSpeed;
            if (moveLerpTime >= 1)
            {
                Vector3.Lerp(transform.position, endPos, 1);
                moveLerpTime = 0;
                moving = false;

            }
        }

    }
}
