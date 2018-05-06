using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    //Variables for lerping the various UI elements.

    //Are the UI elements currently moving?
    private bool moving_charStat = false,
        moving_command = false, moving_textbox = false, moving_selectedBox = false;
    //The end positions for each UI element; for lerps.
    private Vector3 lerpEndPos_cs, lerpEndPos_cmd, lerpEndPos_sb;
    //Is the text box open?
    private bool openTextBox;
    //Holds the currentl lerp values.
    private float lerpTime_charStat, lerpTime_command, lerpTime_textBox, lerpTime_selectBox;

    //The actual UI elements.
    public Image charStatUI, commandUI,textBoxUI, selectedBoxUI;
    public Text textBoxText;

    //Just for ease of use; self explaining.
    private float charStatHeight, commandWidth,selectedWidth;
    //Are the UI elements on the screen?
    private bool onScreen_charStat = false,
        onScreen_command = false, onScreen_textBox = false, onScreen_selectedBox = false;

    //Border between the textbox and the top of the screen.
    private float textBoxScreenBorder = 15f;

    //Just makes thigns easier.
    private int screenXCenter, screenYCenter;
    //txtBoxRate is how fast the textbox opens/closes, and cmpRate is just the variable that's used
    //to store the time differences.
    private float txtBoxRate = 0.1f, cmpRate = 0.0f;

    //Commands and the selections; Actual images. Selections are explained in PlayerControls.cs.
    public Image[] cmds, sps, chs;
    //What's the current position of the cursor in the main menu/selection menu.
    int curCommandSelection = 0;
    int curSelectedNum = 0;
    int curChar = 0;

    //transitionTime is for how fast the UI elements move; duration is for how long the textBox is
    //displayed on the screen before it goes away.
    [SerializeField]
    private float transitionTime = 5f, textBoxDuration = 4f ;

    //For the menu elements.
    Color baseColor = Color.blue, selectColor = Color.red;

    [SerializeField] SelectionUI sU;

    public void Awake()
    {
        //Set the variables used for default menu UI positioning.
        charStatHeight = charStatUI.rectTransform.rect.height;
        commandWidth = commandUI.rectTransform.rect.width;
        selectedWidth = selectedBoxUI.rectTransform.rect.width;
        screenXCenter = Screen.width/2;
        screenYCenter = Screen.height / 2;

        //Move the objects so they're out of the screen and also work with multiple resoultions, hopefully
        charStatUI.rectTransform.position = new Vector3(screenXCenter, -charStatHeight/2, 0);
        commandUI.rectTransform.position = new Vector3(Screen.width + commandWidth/2, screenYCenter, 0);
        selectedBoxUI.rectTransform.position = new Vector3(selectedWidth/2 - selectedWidth, screenYCenter, 0);
        textBoxUI.rectTransform.position = new Vector3(screenXCenter, Screen.height - textBoxScreenBorder - textBoxUI.rectTransform.rect.height/2, 0);
        textBoxUI.rectTransform.localScale = new Vector3(1, 0, 1);
        lerpEndPos_cs = new Vector3(screenXCenter, charStatUI.rectTransform.position.y, 0);
        lerpEndPos_cmd = new Vector3(commandUI.rectTransform.position.x, screenYCenter, 0);
        lerpEndPos_sb = new Vector3(selectedBoxUI.rectTransform.position.x, screenYCenter, 0);

        //Update the Select/Command boxes, so they already have the default colors.
        UpdateCmdPos(0);
        UpdateSelectPos(0);
        UpdateCharPos(-1);
    }

    private void MoveTextbox()
    {
        int pos = 1;
        if (!openTextBox)
        {
            pos = -1;
        }
        textBoxUI.rectTransform.localScale += new Vector3(0, txtBoxRate * pos, 0);
        cmpRate += txtBoxRate;
        if (cmpRate >= 1)
        {

            if (openTextBox)
                textBoxText.enabled = true;

            moving_textbox = false;
            onScreen_textBox = !onScreen_textBox;
            cmpRate = 0;
        }
    }
    private void MoveSelectedBox()
    {
        if (lerpTime_selectBox < 1)
        {
            selectedBoxUI.rectTransform.position = Vector3.Lerp(selectedBoxUI.rectTransform.position,
            lerpEndPos_sb,
            lerpTime_selectBox);

            lerpTime_selectBox += Time.deltaTime * transitionTime;
        }
        else
        {
            //Lerp is finished here.
            moving_selectedBox = false;
            onScreen_selectedBox = !onScreen_selectedBox;
        }
    }
    private void MoveCharStat()
    {
        if (lerpTime_charStat < 1)
        {
            charStatUI.rectTransform.position = Vector3.Lerp(charStatUI.rectTransform.position,
            lerpEndPos_cs,
            lerpTime_charStat);

            lerpTime_charStat += Time.deltaTime * transitionTime;
        }
        else
        {
            //Lerp is finished here.
            moving_charStat = false;
            onScreen_charStat = !onScreen_charStat;
        }
    }
    private void MoveCommand()
    {
        if (lerpTime_command < 1)
        {
            commandUI.rectTransform.position = Vector3.Lerp(commandUI.rectTransform.position,
            lerpEndPos_cmd,
            lerpTime_command);

            lerpTime_command += Time.deltaTime * transitionTime;
        }
        else
        {
            //Lerp is finished here.
            moving_command = false;
            onScreen_command = !onScreen_command;
        }
    }
    public void Update()
    {
        //Debug test for opening the textbox.
        if(Input.GetKeyDown(KeyCode.U))
        {
            ToggleTextbox();
        }

        //If the relevant UI element is supposed to be moving, move it!
        if(moving_textbox)
        {
            MoveTextbox();
        }
        if (moving_selectedBox)
        {
            MoveSelectedBox();
        }
        if (moving_charStat)
        {
            MoveCharStat();
        }
        if(moving_command)
        {
            MoveCommand();
        }     

    }

    //Updates the colors for the command menu
    public void UpdateCmdPos(int newPos)
    {
        curCommandSelection = newPos;
        for (int i=0;i<cmds.Length;i++)
        {
            if (i != curCommandSelection)
                cmds[i].color = baseColor;
            cmds[curCommandSelection].color = selectColor;
        }
    }

    //Updates the colors for the selection menu
    public void UpdateSelectPos(int newPos)
    {
        curSelectedNum = newPos;
        for (int i = 0; i < sps.Length; i++)
        {
            if (i != curSelectedNum)
                sps[i].color = baseColor;
            sps[curSelectedNum].color = selectColor;
        }
    }


    //Updates the colors for selecting a character. IF given -1, sets all characters to their default color.
    public void UpdateCharPos(int newPos)
    {
        curChar = newPos;
        for (int i = 0; i < chs.Length; i++)
        {
            if (i != curChar)
                chs[i].color = baseColor;
            if(curChar != -1)
                chs[curChar].color = selectColor;
        }
    }

    //Toggles the Player's Main UI (So far, the character UI and the command UI)
    public void TogglePlayerUI()
    {
        ToggleCharacterUI();
        ToggleCommandUI();
    }

    //These just set the relevant variables for activating the relevant UI part.
    //Notably; set the lerpTime to 0 for lerping, changing the end position so it moves on/off the screen,
    //and then letting the program it should be moving that relelvant UI part.
    private void ToggleCharacterUI()
    {
        lerpTime_charStat = 0;
        if (onScreen_charStat == false)
        {
            lerpEndPos_cs.y += charStatHeight;
        }
        else
        {
            lerpEndPos_cs.y -= charStatHeight;
        }
        moving_charStat = true;
    }

    private void ToggleCommandUI()
    {
        lerpTime_command = 0;
        if(onScreen_command == false)
        {
            lerpEndPos_cmd.x -= commandWidth;
        }
        else
        {
            lerpEndPos_cmd.x += commandWidth;
        }
        moving_command = true;
    }


    public void ToggleSelectionUI(List<string> selections)
    {
        lerpTime_selectBox = 0;
        if (onScreen_selectedBox == false)
        {
            lerpEndPos_sb.x += selectedWidth;
        }
        else
        {
            lerpEndPos_sb.x -= selectedWidth;
        }
        moving_selectedBox = true;
        sU.SetSelection(selections);
    }

    public bool IsTextboxOpen()
    {
        return openTextBox;
    }

    private void ToggleTextbox()
    {
        lerpTime_textBox = 0;
        if (onScreen_textBox == false)
        {
            //We are opening the textbox
            openTextBox = true;
        }
        else
        {
            textBoxText.enabled = false;
            openTextBox = false;
        }
        moving_textbox = true;
    }

    //Draw the textbox, wth the relevant text.
    public void CallTextbox(string txt)
    {
        textBoxText.text = txt;
        if (openTextBox == false)
            ToggleTextbox();

        if (!IsInvoking())
            Invoke("ToggleTextbox", textBoxDuration);

    }

    public void CloseTextbox()
    {
        if (openTextBox == true)
            ToggleTextbox();
        if(IsInvoking())
        {
            CancelInvoke("ToggleTextbox");
        }
    }

}
