using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Regenerate", menuName = "Status Effect/Regenerate", order = 1)]
public class Regenerate : StatusEffect
{
    public override bool ApplyEffect(Creature user, Creature target)
    {
        return base.ApplyEffect(user, target);
    }

    public override void DoThing(Creature user, Creature target)
    {
        int value = GenericValue;
        value += user.Magic;

        target.AlterCurrentHP(Mathf.Max(Random.Range(value - 5, value + 6),0), SkillType.Magical);
    }

    public override void RemoveEffect(Creature target)
    {
        base.RemoveEffect(target);
    }
}
