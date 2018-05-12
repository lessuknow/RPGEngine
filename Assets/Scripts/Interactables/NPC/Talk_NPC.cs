using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Prints out a single line to a textbox. TOOD: Make it that the text can be set via inspector;
//Handle mutliple textboxes?
public class Talk_NPC : Interact_Base {

    public UnitManager um;

    public override void Toggle()
    {
        um.Textbox("Hey, I heard you were a pretty cool dude");
    }
}
