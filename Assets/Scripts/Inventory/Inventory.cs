using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory<T> {

    List<T> stored;

    public Inventory()
    {
        stored = new List<T>();
    }

    public void AddItem(T itm)
    {
        stored.Add(itm);
    }

    public T GetItem(int pos)
    {
        return stored[pos];
    }
    
    public int GetCount()
    {
        return stored.Count;
    }
    

}
