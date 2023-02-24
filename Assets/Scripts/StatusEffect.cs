using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Effect
{
    public StatusEffect StatusEffect;
    public int CurrentDuration;
    public Creature User;

    public Effect(StatusEffect status, Creature user)
    {
        StatusEffect = status;
        CurrentDuration = status.Duration;
        User = user;
    }

    public virtual void RemoveEffect(Creature target)
    {
        target.RemoveStatusEffects(this);

        //Any stat changes should be undone here
        StatusEffect.RemoveEffect(target);
    }
}

public enum Stat
{
    STRENGTH,
    MAGIC,
    DEFENSE,
    ACCURACY,
    DODGE,
    SPEED
}

[Serializable]
public class StatValue
{
    public Stat Stat;
    public int Value;
}

[Serializable]
[CreateAssetMenu(fileName = "Status Effect", menuName = "Status Effect/Basic Effect", order = 1)]
public class StatusEffect : ScriptableObject
{
    public string Name;
    public string Description;
    public int Duration;
    public int GenericValue;
    public List<StatValue> StatValues;
    public Sprite Icon;
    public Color color = Color.white;

    public virtual bool ApplyEffect(Creature user, Creature target)
    {
        if (target.ApplyStatusEffect(this, user))
        {
            foreach (var item in StatValues)
            {
                switch (item.Stat)
                {
                    case Stat.STRENGTH:
                        target.Strength += item.Value;
                        break;
                    case Stat.MAGIC:
                        target.Magic += item.Value;
                        break;
                    case Stat.DEFENSE:
                        target.Defense += item.Value;
                        break;
                    case Stat.ACCURACY:
                        target.AccuracyMod += item.Value;
                        break;
                    case Stat.DODGE:
                        target.Dodge += item.Value;
                        break;
                    case Stat.SPEED:
                        target.Speed += item.Value;
                        break;
                    default:
                        break;
                }
            }
            
            return true;
        }

        return false;
    }

    public virtual void DoThing(Creature user, Creature target)
    {

    }

    public virtual void RemoveEffect(Creature target)
    {
        //Any stat changes should be undone here
        foreach (var item in StatValues)
        {
            switch (item.Stat)
            {
                case Stat.STRENGTH:
                    target.Strength -= item.Value;
                    break;
                case Stat.MAGIC:
                    target.Magic -= item.Value;
                    break;
                case Stat.DEFENSE:
                    target.Defense -= item.Value;
                    break;
                case Stat.ACCURACY:
                    target.AccuracyMod -= item.Value;
                    break;
                case Stat.DODGE:
                    target.Dodge -= item.Value;
                    break;
                case Stat.SPEED:
                    target.Speed -= item.Value;
                    break;
                default:
                    break;
            }
        }
    }

}
