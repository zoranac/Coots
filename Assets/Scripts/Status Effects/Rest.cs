using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Rest", menuName = "Status Effect/Rest", order = 1)]
public class Rest : Sleep
{
    public override bool ApplyEffect(Creature user, Creature target)
    {
        return base.ApplyEffect(user, target);
    }

    public override void DoThing(Creature user, Creature target)
    {
        int value = GenericValue;
        value += user.Magic;

        target.AlterCurrentHP(Mathf.Max(Random.Range(value - 5, value + 6), 0), SkillType.Magical);

        if (DoRemovalCheck)
        {
            if (Random.Range(0, 4) == 0)
            {
                target.RemoveStatusEffects(this);
            }
        }
    }

    public override void RemoveEffect(Creature target)
    {
        //Any stat changes should be undone here
        target.CreatePopup("Woke Up!", new Color(.9f, .9f, .9f, 1), true);
        base.RemoveEffect(target);
    }
}
