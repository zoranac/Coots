using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Doomed", menuName = "Status Effect/Doomed", order = 1)]
public class Doomed : StatusEffect
{
    public override bool ApplyEffect(Creature user, Creature target)
    {
        return base.ApplyEffect(user, target);
    }

    public override void DoThing(Creature user, Creature target)
    {
        //target.AlterCurrentHP(-(Mathf.RoundToInt(target.MaxHP / 8f)));
    }

    public override void RemoveEffect(Creature target)
    {
        if (GenericValue <= 0)
            Debug.Log("Doomed Needs a Value!");
        //Any stat changes should be undone here
        target.AlterCurrentHP(-(Mathf.RoundToInt(target.MaxHP / GenericValue)), SkillType.Magical);

        base.RemoveEffect(target);
    }
}
