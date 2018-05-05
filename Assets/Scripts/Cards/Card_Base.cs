using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Base {

    protected string name;
    protected enum type_enum { _character, _spell};

    protected type_enum type;

    public string getType()
    {
        return type.ToString();
    }

    public Card_Base()
    {

    }

}
