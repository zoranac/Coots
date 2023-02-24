using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Physical,
    Magical,
    Effect
}


public enum Result
{
    Miss,
    Hit
}

[Serializable]
public class SkillSlot
{
    public Skill Skill;
    public int CurrentCoolDown;

    public SkillSlot(Skill skill)
    {
        Skill = skill;
        CurrentCoolDown = 0;
    }
}


[CreateAssetMenu(fileName = "Skill", menuName = "Basic Skill", order = 1)]
public class Skill : ScriptableObject
{
    public SkillType Type;
    public int Accuracy;
    public int MinValue;
    public int MaxValue;
    public bool ApplyValueToSelf;
    public bool ApplyEffectsToSelf;
    public bool isHealing;
    public int Cooldown;
    public List<StatusEffect> EffectsAppliedOnHit = new List<StatusEffect>();
    public AudioClip SFX;
    public string VFX;
    public bool ApplyVFXToSelf;
    public string Description;


    protected Result handleAccuracy(Creature user, Creature target)
    {
        int accuracyRoll = UnityEngine.Random.Range(0, 100);

        if (accuracyRoll >= Accuracy + user.AccuracyMod - target.Dodge)
        {
            //Miss
            //Debug.Log("Miss!");

            target.CreatePopup("Miss", new Color(.75f, .75f, .75f, 1), true);
            Camera.main.GetComponent<SoundManager>().PlayMissAudio();
            return Result.Miss;
        }

        return Result.Hit;
    }

    public virtual Result UseSkill(Creature user, Creature target)
    {
        bool accCheckMade = false;

        if (!ApplyValueToSelf && MaxValue > 0)
        {
            if (handleAccuracy(user, target) == Result.Miss)
                return Result.Miss;

            accCheckMade = true;
        }

        int mod = 0;

        switch (Type)
        {
            case SkillType.Physical:
                mod = user.Strength;
                break;
            case SkillType.Magical:
                mod = user.Magic;
                break;
            default:
                break;
        }

        //Has Damage
        if (MaxValue > 0)
        {
            if (isHealing)
            {
                if (ApplyValueToSelf)
                {
                    user.AlterCurrentHP(Mathf.Max(UnityEngine.Random.Range(MinValue + mod, MaxValue + mod + 1), 0), Type);
                }
                else
                {
                    target.AlterCurrentHP(Mathf.Max(UnityEngine.Random.Range(MinValue + mod, MaxValue + mod + 1),0), Type);
                }
            }
            else
            {
                if (ApplyValueToSelf)
                {
                    user.AlterCurrentHP(Mathf.Min(UnityEngine.Random.Range(-(MinValue + mod), -(MaxValue + mod + 1)),0), Type);
                }
                else
                {
                    target.AlterCurrentHP(Mathf.Min(UnityEngine.Random.Range(-(MinValue + mod), -(MaxValue + mod + 1)), 0), Type);
                }
            }
        }

        //If not applying effects to self and I didnt already make an accuracy check;
        if(!ApplyEffectsToSelf && !accCheckMade && EffectsAppliedOnHit.Count > 0)
        {
            if (handleAccuracy(user, target) == Result.Miss)
                return Result.Miss;

            accCheckMade = true;
        }

        //Apply Effects
        foreach (var effect in EffectsAppliedOnHit)
        {
            if (ApplyEffectsToSelf)
                effect.ApplyEffect(user, user);
            else
                effect.ApplyEffect(user, target);
        }

        if (SFX == null)
            Debug.LogWarning(name + " does not have an SFX");
        else
            Camera.main.GetComponent<SoundManager>().PlayAudio(SFX);

        return Result.Hit;
    }

}
