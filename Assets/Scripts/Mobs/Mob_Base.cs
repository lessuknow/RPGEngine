using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Base : MonoBehaviour{

    protected int hp;
    protected int maxHp;
    [SerializeField] MobUI mobUI;
    private enum AI { _chase};
    private AI ai;
    public AStar astar;
    public GameObject target;
    public TileManager tm;

    public Mob_Base()
    {
        hp = 100;
        maxHp = hp;
        ai = AI._chase;
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
    public int GetSkill()
    {
        int x = Random.Range(0, 3);
        //We're goingt o change this later... Def. need to refactor.
        List<Vector3> nextMove = astar.FindPath(transform.position, target.transform.position);
        //Note: Add a seperate FindPath function for just the next one.
        if (nextMove.Count > 0)
        {
            tm.MoveEnemy(transform.position, nextMove[0]);
            transform.position = (nextMove[0]);
        }
        
        return x;

    }


}
