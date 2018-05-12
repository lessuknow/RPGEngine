using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Opens up the shop UI, and handles all of that good stuff.
public class Shop_NPC : Interact_Base {

    public UnitManager um;
    public ShopHandler sui;
    public PlayerControls pc;

    public override void Toggle()
    {
        pc.CharInShop();
        //um.Textbox("Hey, I heard you were a pretty cool dude");
        sui.ToggleShop();
    }
}
