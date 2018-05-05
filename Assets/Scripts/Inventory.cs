using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory{

    List<Skill_Base> stored;
    List<string> names;

    public Inventory()
    {
        stored = new List<Skill_Base>();
        names = new List<string>();
    }

    public void AddItem(Skill_Base itm)
    {
        stored.Add(itm);
        names.Add(itm.GetName());
    }

    public Skill_Base GetItem(int pos)
    {
        return stored[pos];
    }

    public List<string> GetNames()
    {
        return names;
    }
    
    public int GetCount()
    {
        return stored.Count;
    }
    

}
