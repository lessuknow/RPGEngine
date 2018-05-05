using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Character : Card_Base {

    protected int health;
    protected int mana;

    protected int str;
    protected int def;
    protected int agi;
    //should be all we need for now...


	public Card_Character()
    {
        name = "card_character";
        type = type_enum._character;

        health = 20;
        mana = 15;
        str = 4;
        def = 3;
        agi = 2;
    }

}
