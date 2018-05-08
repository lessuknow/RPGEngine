using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int gridX, gridY;
    public bool walkable = true;
    public int gCost;
    public int hCost;
    public Node parent;
    private int origGCost, origHCost;

    public Node()
    {

    }

    public void resetCosts()
    {
        gCost = 0;
        hCost = 0;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
        set
        {

        }
    }

}
