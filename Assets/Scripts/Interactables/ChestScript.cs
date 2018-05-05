using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : Interact_Base {

    //TODO: Add targetting.

    public Skill_Base itm;

    public UnitManager um;

    private bool opened = false;

	public override void Toggle()
    {
        if(!opened)
        { 
            um.AddItem(itm);
            opened = true;
        }
    }
}
