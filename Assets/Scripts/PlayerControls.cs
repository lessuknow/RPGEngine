using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
        

    //UIHandler, for handling the UI elements actually being rendered and stuff.
    [SerializeField] public UIHandler uih;
    [SerializeField] public UnitManager um;
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

    //Variables for storing the menu's position and how many elements are in the Main menu.
    private int curMenuPos = 0, curSelectPos = 0, curCharPos = 0;
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
            if (moving)
            {
                ForceEnemyTurn();
            }
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
            if(moving)
            {
                ForceEnemyTurn();
            }
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
            Interact_Base bs = x[0].gameObject.GetComponent<Interact_Base>();
            bs.Toggle();
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
        else if(cS == curState._Shop)
        {
            ProcessShopUI();
        }
        //If we're in a menu, handle the menus accordingly.
        else if(cS == curState._Menu)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                if (curMenuPos != 0)
                    curMenuPos--;
                uih.UpdateCmdPos(curMenuPos);
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                if (curMenuPos < menuNum-1)
                    curMenuPos++;
                uih.UpdateCmdPos(curMenuPos);
            }
        }
        else if(cS == curState._Selection)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (curSelectPos != 0)
                    curSelectPos--;
                uih.UpdateSelectPos(curSelectPos);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (curSelectPos < menuNum - 1)
                    curSelectPos++;
                uih.UpdateSelectPos(curSelectPos);
            }
        }
        else if (cS == curState._Char)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (curCharPos != 0)
                    curCharPos--;
                uih.UpdateCharPos(curCharPos);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (curCharPos < charNum - 1)
                    curCharPos++;
                uih.UpdateCharPos(curCharPos);
            }
        }

        //Interact button; In menus acts as the 'Back' Button. Probably need to fix this later.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //If there's no menu open, just interact with whatever object is right in front of the player.
            if(cS == curState._Null)
                InteractForward();

            //Wait, we're in the main menu! So, we close it!
            else if (cS == curState._Menu)
            {
                cS = curState._Null;
                uih.TogglePlayerUI();
            }
            //We're in the selection menu! So we need to leave it.
            else if (cS == curState._Selection)
            {
                cS = curState._Menu;

                uih.ToggleSelectionUI(um.GetSkillDisplay(curCharTurn));
            }
            //Targetting players; need to back up.
            else if (cS == curState._Char)
            {
                cS = curState._Selection;
                uih.UpdateCharPos(-1);
            }
        }
        //Menu button is currently e; 
        if (Input.GetKeyDown(KeyCode.E) && !um.IsUITextboxOpen())
        {
            //We're not in any menu, so we opened the menu.
            if (cS == curState._Null)
            {
                cS = curState._Menu;
                uih.TogglePlayerUI();
            }            
            //If there's a menu, select the currently highlighted option.
            else if (cS == curState._Menu)
            {
                MenuSelect();
            }
            //We're in the selection menu. Run the currently highlighted option.
            else if (cS == curState._Selection)
            {
                //Do stuff after accessing the app. skill/item.
                //TODO: This stuff!

                cS = curState._Char;

                //If its a friendly skill, we need ot chose a friend
                if (um.CharSkillIsFriendly(curCharTurn, curSelectPos) == true)
                    uih.UpdateCharPos(curCharPos);
                //If its aggressive, we automatically go to performing the move.
                else
                {
                    um.UseSelected(selectionType.ToString(), curCharTurn, curCharPos, curSelectPos);
                    uih.ToggleSelectionUI(um.GetSkillDisplay(curCharTurn));
                    uih.TogglePlayerUI();
                    NextTurn();
                }

            }
            //After getting targeting, do the move!
            else if (cS == curState._Char)
            {
                um.UseSelected(selectionType.ToString(), curCharTurn, curCharPos,curSelectPos);
                uih.ToggleSelectionUI(um.GetSkillDisplay(curCharTurn));
                uih.TogglePlayerUI();
                NextTurn();
            }

        }
        else if(Input.GetKeyDown(KeyCode.E) && um.IsUITextboxOpen())
        {
            um.CloseTextbox();
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

                //If the player moves, after they move the enemies get to move. Easy.
                //We're setting curCharturn to charNum-1 in order to actually have the enemies move,
                //and reset the varaibles to their previous values.
                ForceEnemyTurn();
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
    
    //Handles switching tur.s
    private void NextTurn()
    {
        cS = curState._Null;
        curCharTurn++;
        curCharPos = 0;
        curSelectPos = 0;
        uih.UpdateCharPos(-1);
        if(curCharTurn == charNum)
        {
            um.RunEnemySkills();
            curCharTurn = 0;
        }
    }

    //Has the enmey move.
    private void ForceEnemyTurn()
    {
        cS = curState._Null;
        curCharPos = 0;
        curSelectPos = 0;
        uih.UpdateCharPos(-1);
        um.RunEnemySkills();
        curCharTurn = 0;
    }

    //Handles the act of actually selecting a menu item.
    private void MenuSelect()
    {
        //We're going to assume that the position for items is 3
        //TODO: Finish code for items/the rest of it!
        if(curMenuPos == 0)
        {
            um.UseSelected("Attack" , curCharTurn, curCharPos, curSelectPos);

            cS = curState._Null;
            uih.TogglePlayerUI();
            NextTurn();
        }
        else if(curMenuPos == 2)
        {
            print(um.GetSkillDisplay(curCharTurn).Count);
            if(um.GetSkillDisplay(curCharTurn).Count > 0)
            { 
                uih.ToggleSelectionUI(um.GetSkillDisplay(curCharTurn));
                selectionType = curSelect.Skill;
                cS = curState._Selection;
            }
            else
            {
                um.Textbox(um.chars[curCharTurn].name +" has no skills!"); 
            }
        }
    }

   

}
