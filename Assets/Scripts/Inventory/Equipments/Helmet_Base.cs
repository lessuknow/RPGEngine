using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet_Base : Item {

    protected int defValue;

	public Helmet_Base(string name, int def)
    {
        name = "buzz";
        defValue = def;
    }
    public int getDefValue()
    {
        return defValue;
    }

}
