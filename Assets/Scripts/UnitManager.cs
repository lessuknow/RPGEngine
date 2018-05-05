using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    //Players all move first, and then all the enemies move.

    public Character[] chars;
    public List<Mob_Base> enemies;
    public CharacterUI oneUI, twoUI, threeUI;
    public PlayerControls pC;
    
    [SerializeField] UIHandler uh;

    private void Awake()
    {
        chars = new Character[3];
        //Wow, we have 3 characters!
        for (int i = 0; i < chars.Length; i++)
            chars[i] = new Character();
        //This will take in a given skill and do the stuff to the relevant characters

        chars[0].charUI = oneUI;
        chars[1].charUI = twoUI;
        chars[2].charUI = threeUI;

        chars[0].AddSkill(new Skill_Base("Tar.Friend", true));
        chars[0].AddSkill(new Skill_Base("Tar.Enemy", false));

        enemies = new List<Mob_Base>();
        

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            print("Reduced player1 hp");
            chars[2].AddHealth(-1);
        }
    }

    private void RunEnemySkills()
    {
        for (int i = 0; i < enemies.Count; i++)
            enemies[i].UseSkill();
    }

    //Returns if the tested skill targets friends or enemies.
    public bool CharSkillIsFriendly(int curPlayer, int curSkillPos)
    {
        return chars[curPlayer].GetSkill(curSkillPos).IsFriendly();
    }

    public List<string> GetSkillDisplay(int curPlayer)
    {
        return chars[curPlayer].GetSkillNames();
    }

    public void AddItem(Skill_Base itm)
    {
        chars[0].AddSkill(itm);
        uh.CallTextbox("You got a " + itm.GetName());
    }

    public void UseSelected(string selectionType,int curPlayer, int pos, int skillPos)
    {
        print("Selected!");
        if(selectionType == "Skill")
        {
            print("is skill "+curPlayer+" "+pos);
            
            if(CharSkillIsFriendly(curPlayer,skillPos))
            { 
                if(skillPos < (chars[curPlayer].GetSkillNames().Count))
                {
                    print("is a valid position");
                    ProcessFriendly(chars[curPlayer], chars[pos], chars[curPlayer].GetSkill(skillPos));

                    //That means our skill went through
                    RunEnemySkills();
                }
            }
            else
            {
                Mob_Base enm = pC.GetCollisionInFront().GetComponent<Mob_Base>();
                print("is enemy skill " + enm);
                ProcessEnemy(chars[curPlayer], enm, chars[curPlayer].GetSkill(skillPos));
            }
        }
       
    }


    public void ProcessEnemy(Character caster, Mob_Base target, Skill_Base skill)
    {
        float result = 0;
        result += skill.GetWisScaling() * caster.wis;

        result *= -1;

        target.AddHealth(Mathf.RoundToInt(result));

    }

    //Currently heals the target, based on the caster's wis
    public void ProcessFriendly(Character caster, Character target, Skill_Base skill)
    {
        float result = 0;
        result += skill.GetWisScaling() * caster.wis;

        target.AddHealth(Mathf.RoundToInt(result));
        
    }

}
