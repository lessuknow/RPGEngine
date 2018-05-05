using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character{

    public int hp = 10;
    public int maxHp;
    private Inventory skills;
    public int str = 4;
    public int wis = 6;
    public CharacterUI charUI;

    public Character()
    {
        maxHp = hp;
        skills = new Inventory();
    }

    public float GetHPPercent()
    {
        return (float)hp / maxHp;
    }

    public void AddHealth(int num)
    {
        //print(hp+" healed by " + num);
        if (hp + num > maxHp)
            hp = maxHp;
        else if (hp + num <= 0)
            hp = 0;
        else
            hp += num;
        //print("hp is now " + hp);
        charUI.SetHPAmount(GetHPPercent());
    }

    public List<string> GetSkillNames()
    {
        return skills.GetNames();
    }

    public void AddSkill(Skill_Base s)
    {
        skills.AddItem(s);
    }

    public Skill_Base GetSkill(int pos)
    {
        return skills.GetItem(pos);
    }

}
