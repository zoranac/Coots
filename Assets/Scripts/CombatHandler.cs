using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatHandler : MonoBehaviour
{
    public Player player;
    public Enemy CurrentEnemy;

    int restProgress = 0;

    public Animator VFX;
    public Animator VFXEnemy;
    public Transform CombatPopupText;

    private Skill previousSkill;

    bool didDeath = false;

    public void StartCombat(Enemy enemy, bool playerInit = true)
    {
        if (CurrentEnemy == null)
        {
            didDeath = false;
            CurrentEnemy = enemy;
            restProgress = 0;
            LevelLoader.i.LoadLevel("Combat", LoadSceneMode.Additive);

            if (!playerInit)
            {
                HandleEnemyTurn(1.5f);
            }

            //SceneManager.sceneLoaded

            //GameObject.Find("PlayerCombat").GetComponent<PlayerCombat>().Setup();
        }
    }

    public void Update()
    {
        if (CurrentEnemy != null)
        {
            CurrentEnemy.CheckIfDead();

            if (CurrentEnemy.IsDead)
            {
                EnemyDeath();
            }
        }
    }

    public void EnemyDeath()
    {
        if (!didDeath)
        {
            LevelLoader.i.UnloadLevel("Combat");

            player.GainExp(CurrentEnemy.ExpValue);
            player.RemoveAllEffects();

            Destroy(CurrentEnemy.gameObject);
            CurrentEnemy = null;

            MusicManager.i.StartMusic();

            didDeath = true;
        }
    }

    public void HandleEnemyTurn(float delay = 1f)
    {
        if (CurrentEnemy != null)
        {
            StartCoroutine(handleEnemyTurn(delay));
        }
    }

    IEnumerator handleEnemyTurn(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (CurrentEnemy == null)
        {
            yield break;
        }

        if (CurrentEnemy.IsDead)
        {
            EnemyDeath();
            yield break;
        }

        //Skip turn if sleeping
        if (CurrentEnemy.StatusEffects.Exists(x=>x.StatusEffect is Sleep))
        {
            CurrentEnemy.CreatePopup("Sleeping", new Color(.75f, .75f, .75f, 1), true);
            //Still reduce effect durations
            ReduceEffectDurations();
            yield break;
        }

        //Determine if resting
        restProgress += player.Speed - CurrentEnemy.Speed;

        if (restProgress >= 100)
        {
            restProgress -= 100;

            CurrentEnemy.CreatePopup("Skipped", new Color(.75f, .75f, .75f, 1), true);

            //Still reduce effect durations
            ReduceEffectDurations();

            yield break;
        }

        //Otherwise Move
        DoAction();

        //Determine if taking second move
        if (restProgress <= -100)
        {

            yield return new WaitForSeconds(delay);
        
            restProgress += 100;

            if (CurrentEnemy == null)
            {
                yield break;
            }

            if (CurrentEnemy.IsDead)
            {
                EnemyDeath();
                yield break;
            }

            CurrentEnemy.CreatePopup("Extra Action", Color.green, true);

            DoAction();
        }
    }

    void ReduceEffectDurations()
    {
        foreach (var effect in CurrentEnemy.StatusEffects)
        {
            effect.StatusEffect.DoThing(effect.User, CurrentEnemy);
        }

        CurrentEnemy.ReduceStatusEffectDurations();
    }

    void DoAction()
    {
        var slotOptions = CurrentEnemy.Skills.Where(x => x.CurrentCoolDown <= 0).ToList();

        if (previousSkill != null && CurrentEnemy.conditionalSkills.Count > 0)
        {
            foreach (var item in CurrentEnemy.conditionalSkills.Where(x => x.SkillToUse.CurrentCoolDown <= 0))
            {
                if (previousSkill == item.PreviousSkill) 
                    slotOptions.Add(item.SkillToUse);
            }            
        }

        if (slotOptions.Count() <= 0 || CurrentEnemy.currentHP <= 0)
        {
            CurrentEnemy.CreatePopup("Skipped", Color.gray, true);
        }
        else
        {
            var slot = slotOptions[Random.Range(0, slotOptions.Count())];

            var result = slot.Skill.UseSkill(Camera.main.GetComponent<CombatHandler>().CurrentEnemy, player);

            PopupText.Create(CombatPopupText.position, CurrentEnemy.Name + " used " + slot.Skill.name + "!",Color.white, .5f);

            if (result != Result.Miss)
            {
                previousSkill = slot.Skill;

                if (slot.Skill.ApplyVFXToSelf)
                    VFXEnemy.Play(slot.Skill.VFX);
                else
                    VFX.Play(slot.Skill.VFX);
            }
            else
            {
                previousSkill = null;
            }

            slot.CurrentCoolDown = slot.Skill.Cooldown + 1;
        }

        foreach (var skillSlot in CurrentEnemy.Skills)
        {
            skillSlot.CurrentCoolDown--;

            if (skillSlot.CurrentCoolDown < 0)
                skillSlot.CurrentCoolDown = 0;
        }

        foreach (var skillSlot in CurrentEnemy.conditionalSkills)
        {
            skillSlot.SkillToUse.CurrentCoolDown--;

            if (skillSlot.SkillToUse.CurrentCoolDown < 0)
                skillSlot.SkillToUse.CurrentCoolDown = 0;
        }


        ReduceEffectDurations();

        if (CurrentEnemy.IsDead)
        {
            EnemyDeath();
            StopAllCoroutines();
        }
    }


}
