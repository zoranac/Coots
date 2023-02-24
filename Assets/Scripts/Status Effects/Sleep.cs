using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Sleep", menuName = "Status Effect/Sleep", order = 1)]
public class Sleep : StatusEffect
{
    public bool DoRemovalCheck = true;

    public override bool ApplyEffect(Creature user, Creature target)
    {
        return base.ApplyEffect(user, target);
    }

    public override void DoThing(Creature user, Creature target)
    {
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
