using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Poisoned", menuName = "Status Effect/Poisoned", order = 1)]
public class Poisoned : StatusEffect
{

    public override bool ApplyEffect(Creature user, Creature target)
    {
        return base.ApplyEffect(user, target);
    }

    public override void DoThing(Creature user,Creature target)
    {
        if (GenericValue <= 0)
            GenericValue = 8;

        int extraAmount = user.Magic;

        target.AlterCurrentHP(-Mathf.Max(Mathf.RoundToInt(target.MaxHP / GenericValue)+extraAmount, 0), SkillType.Magical);
    }

    public override void RemoveEffect(Creature target)
    {
        //Any stat changes should be undone here
        base.RemoveEffect(target);
    }
}
