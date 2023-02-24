using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Exp", menuName = "Exp Skill", order = 1)]
public class ExpSkill : Skill
{
    public override Result UseSkill(Creature user, Creature target)
    {
        if (user is Player)
        {
            (user as Player).Exp += MaxValue;
            return Result.Hit;
        }

        return Result.Miss;
    }
}
