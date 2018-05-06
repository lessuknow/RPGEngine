using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    //Players all move first, and then all the enemies move.

    public Character[] chars;
    private List<Mob_Base> enemies;
    public CharacterUI oneUI, twoUI, threeUI;
    public PlayerControls pC;

    //TODO: Make a dictionary to store all of the skills.
    private Skill_Base attack;

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

        chars[0].name = "One";
        chars[1].name = "Two";
        chars[2].name = "Three";

        chars[0].AddSkill(new Skill_Base("Tar.Friend", true));
        chars[0].AddSkill(new Skill_Base("Tar.Enemy", false));

        
        enemies = new List<Mob_Base>();

        attack = new Skill_Base("Attack", false);

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            chars[2].AddHealth(-1);
        }
    }

    public void RunEnemySkills()
    {
        for (int i = 0; i < enemies.Count; i++)
        { 
            chars[0].AddHealth(-enemies[i].GetSkill());
        }
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

    public bool IsUITextboxOpen()
    {
        return uh.IsTextboxOpen();
    }

    public void AddItem(Skill_Base itm)
    {
        chars[0].AddSkill(itm);
        Textbox("You got a " + itm.GetName());
    }
    //refactor these;
    public void Textbox(string x)
    {
        uh.CallTextbox(x);
    }

    public void CloseTextbox()
    {
        uh.CloseTextbox();
    }

    public void UseSelected(string selectionType,int curPlayer, int pos, int skillPos)
    {
        if(selectionType == "Attack")
        {
            Mob_Base enm = pC.GetCollisionInFront().GetComponent<Mob_Base>();
            ProcessEnemy(chars[curPlayer], enm, attack);
        }
        else if(selectionType == "Skill")
        {
            if(CharSkillIsFriendly(curPlayer,skillPos))
            { 
                if(skillPos < (chars[curPlayer].GetSkillNames().Count))
                {
                    ProcessFriendly(chars[curPlayer], chars[pos], chars[curPlayer].GetSkill(skillPos));

                    //That means our skill went through
                    RunEnemySkills();
                }
            }
            else
            {
                Mob_Base enm = pC.GetCollisionInFront().GetComponent<Mob_Base>();
                ProcessEnemy(chars[curPlayer], enm, chars[curPlayer].GetSkill(skillPos));
            }
        }
       
    }

    public void AddEnemy(Mob_Base en)
    {
        enemies.Add(en);
    }

    public void ProcessEnemy(Character caster, Mob_Base target, Skill_Base skill)
    {
        float result = 0;
        result += skill.GetWisScaling() * caster.wis;

        result *= -1;

        target.AddHealth(Mathf.RoundToInt(result));

        Textbox(caster.name + " used " + skill.GetName() + "!\n"
            + target.name + " took " + result + " damage!");
    }

    //Currently heals the target, based on the caster's wis
    public void ProcessFriendly(Character caster, Character target, Skill_Base skill)
    {
        float result = 0;
        result += skill.GetWisScaling() * caster.wis;

        target.AddHealth(Mathf.RoundToInt(result));
        Textbox(caster.name + " used " + skill.GetName() + "!\n"
            +target.name+" recovered "+result+" health!");
    }

}
