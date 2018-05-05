using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Base : MonoBehaviour{

    protected int hp;
    protected int maxHp;
    [SerializeField] MobUI mobUI;

    public Mob_Base()
    {
        hp = 100;
        maxHp = hp;
    }

    public float GetHPPercent()
    {
        return (float)hp / maxHp;
    }

    public void AddHealth(int num)
    {
        if (hp + num > maxHp)
            hp = maxHp;
        else if (hp + num <= 0)
            hp = 0;
        else
            hp += num;
        print("new health is " + hp);
        mobUI.SetHPAmount(GetHPPercent());
    }

    //Returns a skill's damage value
    public int UseSkill()
    {
        int x = Random.Range(0, 3);

        return x;

    }


}
