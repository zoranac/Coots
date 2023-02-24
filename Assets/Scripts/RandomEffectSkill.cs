using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Random", menuName = "Random Skill", order = 1)]
public class RandomEffectSkill : Skill
{
    public override Result UseSkill(Creature user, Creature target)
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
                    target.AlterCurrentHP(Mathf.Max(UnityEngine.Random.Range(MinValue + mod, MaxValue + mod + 1), 0), Type);
                }
            }
            else
            {
                if (ApplyValueToSelf)
                {
                    user.AlterCurrentHP(Mathf.Min(UnityEngine.Random.Range(-(MinValue + mod), -(MaxValue + mod + 1)), 0), Type);
                }
                else
                {
                    target.AlterCurrentHP(Mathf.Min(UnityEngine.Random.Range(-(MinValue + mod), -(MaxValue + mod + 1)), 0), Type);
                }
            }
        }

        //If not applying effects to self and I didnt already make an accuracy check;
        if (!ApplyEffectsToSelf && !accCheckMade && EffectsAppliedOnHit.Count > 0)
        {
            if (handleAccuracy(user, target) == Result.Miss)
                return Result.Miss;

            accCheckMade = true;
        }

        //Apply Effects
        int rand = Random.Range(0, EffectsAppliedOnHit.Count);

        if (ApplyEffectsToSelf)
            EffectsAppliedOnHit[rand].ApplyEffect(user, user);
        else
            EffectsAppliedOnHit[rand].ApplyEffect(user, target);

        if (SFX == null)
            Debug.LogWarning(name + " does not have an SFX");
        else
            Camera.main.GetComponent<SoundManager>().PlayAudio(SFX);

        return Result.Hit;
    }
}
