using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EquipmentDictionary {

    static readonly EquipmentDictionary _instance = new EquipmentDictionary();
    public static EquipmentDictionary Instance
    {
        get
        {
            return _instance;
        }
    }

    EquipmentDictionary()
    {

    }

}
