using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicDoT", menuName = "Status Effect/Basic DoT", order = 1)]
public class BasicDoT : StatusEffect
{
    public SkillType Type = SkillType.Physical;

    public override bool ApplyEffect(Creature user, Creature target)
    {
        return base.ApplyEffect(user, target);
    }

    public override void DoThing(Creature user, Creature target)
    {
        int value = GenericValue;

        if (Type == SkillType.Physical)
            value += user.Strength;
        else if (Type == SkillType.Magical)
            value += user.Magic;

        target.AlterCurrentHP(-Mathf.Max(Random.Range(value - 5, value + 6), 0), Type);
    }

    public override void RemoveEffect(Creature target)
    {
        //Any stat changes should be undone here
        base.RemoveEffect(target);
    }
}
