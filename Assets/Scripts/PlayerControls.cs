using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
        
    
    [SerializeField] public TileManager tm;

    //Custom enums. curDir holds the player's current direction its facing, and curState
    //holds the player's current menu state (e.g., no menu, in the main menu, in the selection menu, ect.)
    //NOTE: selection menu is a default menu that can be populated by various things (item/skill names)
    //and prob. will need more variables to differentiate between those.
    public Direction curDir = Direction.South;
    public enum Direction { North, East, South, West };
    private enum curState { _Null, _Menu, _Selection, _Char, _Shop };
    private curState cS;
    
    //Variables for the movement stuff (Rotations and end pos); for lerping.
    private Vector3 endPos;
    private Quaternion endRotate;
    private float moveLerpTime = 0, rotateLerpTime = 0;
    private bool moving = false, rotating = false;
    [SerializeField] private float rotateSpeed = 5, moveSpeed = 5;

    //Scale of each tile; used with the movement code.
    float tileSize = 1.33f;
    
    //RN these are hardcoded.
    const int menuNum = 5, charNum = 3;

    //RN it is player one's turn.
    private int curCharTurn = 0;
    //For the selection
    private enum curSelect { Skill, Passive};
    private curSelect selectionType;


    public void Awake()
    {
        cS = curState._Null;
    }

    //Code for rotating the player and moving it.
    public void OverworldMovement()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            rotateLerpTime = 0;
            if (curDir == Direction.South)
            {
                endRotate.eulerAngles = new Vector3(0, 90, 0);
                curDir = Direction.East;
                rotating = true;
            }
            else if (curDir == Direction.North)
            {
                endRotate.eulerAngles = new Vector3(0, 270, 0);
                curDir = Direction.West;
                rotating = true;
            }
            else if (curDir == Direction.East)
            {

                endRotate.eulerAngles = new Vector3(0, 180, 0);
                curDir = Direction.North;
                rotating = true;
            }
            else
            {
                endRotate.eulerAngles = new Vector3(0, 0, 0);
                curDir = Direction.South;
                rotating = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            rotateLerpTime = 0;
            if (curDir == Direction.North)
            {
                endRotate.eulerAngles = new Vector3(0, 90, 0);
                curDir = Direction.East;
                rotating = true;
            }
            else if (curDir == Direction.South)
            {
                endRotate.eulerAngles = new Vector3(0, 270, 0);
                curDir = Direction.West;
                rotating = true;
            }
            else if (curDir == Direction.West)
            {

                endRotate.eulerAngles = new Vector3(0, 180, 0);
                curDir = Direction.North;
                rotating = true;
            }
            else
            {
                endRotate.eulerAngles = new Vector3(0, 0, 0);
                curDir = Direction.South;
                rotating = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            moveLerpTime = 0;
            int xMod = 0;
            int yMod = 0;

            if (curDir == Direction.South)
                yMod = -1;
            else if (curDir == Direction.North)
                yMod = 1;
            else if (curDir == Direction.East)
                xMod = -1;
            else
                xMod = 1;

            //Vector3 checkPos = tlmp.CellToWorld(a);
            Vector3 checkPos = tm.GetTilePos(transform.position);
            checkPos.z += yMod * tileSize;
            checkPos.x += xMod * tileSize;

            if (tm.IsWalkable(checkPos))
            {
                endPos[0] = checkPos[0] + tileSize / 2;
                endPos[1] = transform.position.y;
                endPos[2] = checkPos[2] + tileSize / 2;
                moving = true;
            }

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            moveLerpTime = 0;
            int xMod = 0;
            int yMod = 0;

            if (curDir == Direction.South)
                yMod = 1;
            else if (curDir == Direction.North)
                yMod = -1;
            else if (curDir == Direction.East)
                xMod = 1;
            else
                xMod = -1;

               //Vector3 checkPos = tlmp.CellToWorld(a);
            Vector3 checkPos = tm.GetTilePos(transform.position);
            checkPos.z += yMod * tileSize;
            checkPos.x += xMod * tileSize;

            if (tm.IsWalkable(checkPos))
            {
                endPos[0] = checkPos[0] + tileSize / 2;
                endPos[1] = transform.position.y;
                endPos[2] = checkPos[2] + tileSize / 2;
                moving = true;
            }
            
        }
    }
    //Code for interacting with the object in front.
    //Object needs a boxcollider.
    public void InteractForward()
    {
        int xMod = 0;
        int yMod = 0;

        if (curDir == Direction.South)
            yMod = -1;
        else if (curDir == Direction.North)
            yMod = 1;
        else if (curDir == Direction.East)
            xMod = -1;
        else
            xMod = 1;

        //Vector3 checkPos = tlmp.CellToWorld(a);
        Vector3 checkPos = transform.position;
        checkPos.z += yMod * tileSize;
        checkPos.x += xMod * tileSize;
        Collider[] x = Physics.OverlapSphere(checkPos, 0.2f);

        //check if theres a door
        if (x.Length > 0 && x[0].tag == "Interactable")
        {
            print("toggled itneractable");
            Interact_Base bs = x[0].gameObject.GetComponent<Interact_Base>();
            bs.Toggle();
        }
    }

    public Collider GetCollisionInFront()
    {
        int xMod = 0;
        int yMod = 0;

        if (curDir == Direction.South)
            yMod = -1;
        else if (curDir == Direction.North)
            yMod = 1;
        else if (curDir == Direction.East)
            xMod = -1;
        else
            xMod = 1;

        //Vector3 checkPos = tlmp.CellToWorld(a);
        Vector3 checkPos = transform.position;
        checkPos.z += yMod * tileSize;
        checkPos.x += xMod * tileSize;
        Collider[] x = Physics.OverlapSphere(checkPos, 0.2f);

        //check if theres a door
        if (x.Length > 0 && x[0].tag == "Enemy")
        {
            print("is enemy");
            return x[0];
        }
        return null;
    }

    //Sets the charater's current state to "In shop". This is the only thing that sets cS to Shop.
    public void CharInShop()
    {
        cS = curState._Shop;
    }
    
   
    //Handles the shop's UI. Called in update when cS == curState._Shop.
    public void ProcessShopUI()
    {

    }

    void Update () {
        //If we're not in a menu, just run the overworld movement code.
        if(cS == curState._Null)
            {
            OverworldMovement();
        }
        
        //Handle the actual movement/Rotation lerps.
        if(moving)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, moveLerpTime);
            moveLerpTime += Time.deltaTime * moveSpeed ;
            if (moveLerpTime >= 1)
            {
                Vector3.Lerp(transform.position, endPos, 1);
                moveLerpTime = 0;
                moving = false;
                
            }
        }
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
    }
    
}
