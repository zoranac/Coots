using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public int BaseUses;
    public Skill Skill;
    public Sprite Icon;

    private void OnEnable()
    {
        if(Skill != null && Skill.Description == "")
        {
            Skill.Description = Description;
        }
    }

    public Result UseItem(Player player)
    {
        var currentEnemy = Camera.main.GetComponent<CombatHandler>().CurrentEnemy;

        var result = Skill.UseSkill(player, currentEnemy);

        return result;
    }

}
