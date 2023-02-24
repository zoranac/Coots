using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Creature : Thing
{
    public Sprite BattleSprite;

    public int Speed = 100;
    public int Strength = 0;
    public int Magic = 0;
    public int Defense = 0;
    public int MaxHP = 100;
    public int AccuracyMod = 0;
    public int Dodge = 0;
    public int currentHP;
    protected Grid Grid;



    public List<Effect> StatusEffects = new List<Effect>();

    List<StatusEffectUI> statusEffectUIs = new List<StatusEffectUI>();

    public List<SkillSlot> Skills = new List<SkillSlot>();
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Grid = Camera.main.GetComponent<Grid>();
        currentHP = MaxHP;
    }


    public void CreatePopup(string text, Color color, bool OnMe = true, Transform transform = null, float duration = 1)
    {
        if (transform != null)
        {
            PopupText.Create(transform.position, text, color, duration, "ExplorationScene");
            return;
        }


        if ((this is Player && OnMe) || (this is Enemy && !OnMe))
        {
            PopupText.Create(GameObject.Find("PopupSpawnLocation").transform.position, text, color, duration);
        }
        else if ((this is Enemy && OnMe) || (this is Player && !OnMe))
        {
            PopupText.Create(GameObject.Find("PopupSpawnLocation Enemy").transform.position, text, color, duration);
        }
    }

    public void AlterCurrentHP(int value, SkillType type)
    {
        if (value < 0)
        {
            if (type == SkillType.Magical)
            {
                value = Mathf.Min(0, value + Magic);
            }
            else if (type == SkillType.Physical)
            {
                value = Mathf.Min(0, value + Defense);
            }
        }

        currentHP += value;

        CreatePopup(value.ToString(), value <= 0 ? Color.red : Color.green);

        if (currentHP > MaxHP)
        {
            currentHP = MaxHP;
        }


    }

    public virtual void Die()
    {

    }

    public bool ApplyStatusEffect(StatusEffect status, Creature user)
    {
        var match = StatusEffects.Where(x => x.StatusEffect.Name == status.Name).ToArray();

        CreatePopup(status.Name, status.color);

        if (match.Count() > 0)
        {
            match[0].CurrentDuration = status.Duration;
            return false;
        }
        else
        {
            StatusEffects.Add(new Effect(status, user));
            var g = Instantiate(GameAssets.i.pfStatusEffectUI);
            if (this is Player)
            {
                g.SetParent(GameObject.Find("StatusEffectBar").transform);
                g.GetComponent<StatusEffectUI>().SetStatusEffect(status, false);
            }
            else if (this is Enemy)
            {
                g.SetParent(GameObject.Find("StatusEffectBarEnemy").transform);
                g.GetComponent<StatusEffectUI>().SetStatusEffect(status, true);
            }

            g.GetComponent<Image>().sprite = status.Icon;
            g.GetComponent<Image>().color = status.color;

           statusEffectUIs.Add(g.GetComponent<StatusEffectUI>());
            return true;
        }
    }

    public void ReduceStatusEffectDurations()
    {
        List<Effect> statusesToRemove = new List<Effect>();

        foreach (var status in StatusEffects)
        {
            status.CurrentDuration--;

            if (status.CurrentDuration <= 0)
            {
                statusesToRemove.Add(status);
            }
        }

        foreach (var status in statusesToRemove)
        {
            status.RemoveEffect(this);
        }

    }
    internal void RemoveStatusEffects(Effect effect)
    {
        StatusEffects.Remove(effect);
        var ui = statusEffectUIs.Find(x => x.StatusEffect == effect.StatusEffect);
        statusEffectUIs.Remove(ui);
        Destroy(ui.gameObject);
    }

    internal void RemoveStatusEffects(StatusEffect status)
    {
        var effect = StatusEffects.Find(x => x.StatusEffect == status);
        effect.RemoveEffect(this);
    }

    public void RemoveAllEffects()
    {
        while (StatusEffects.Count > 0) {
            StatusEffects[0].RemoveEffect(this);
        }
    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    protected virtual void Update()
    {
        CheckIfDead();
    }

    public void CheckIfDead()
    {
        if (currentHP <= 0)
        {
            Die();
        }
    }
}
