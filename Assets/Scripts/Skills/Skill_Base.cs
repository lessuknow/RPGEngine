using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base {

    protected string name = "NaN";
    protected enum type {chars, enemies};
    protected type target;
    protected float wisScaling;
    protected int baseValue = 3;

    public Skill_Base(string _name, bool isFriendly)
    {
        name = _name;
        if (isFriendly)
            target = type.chars;
        else
            target = type.enemies;

        wisScaling = 2f;
    }

    public bool IsFriendly()
    {
        return target == type.chars;
    }

    public string GetTarget()
    {
        return target.ToString();
    }

    public string GetName()
    {
        return name;
    }

    public float GetWisScaling()
    {
        return wisScaling;
    }


}
