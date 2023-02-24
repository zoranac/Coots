using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearnSkillButtonUI : MonoBehaviour
{
    public LevelUpManager manager;

    HoverTooltip tooltip;

    internal void Setup(Skill item)
    {
        GetComponentInChildren<TMP_Text>().text = item.name;
        GetComponent<Button>().onClick.AddListener(delegate { manager.SkillToAdd(item); });

        SetupToolTip();

        tooltip.Tip = item.Description ?? "";
        tooltip.Title = item.name;
        tooltip.Extra = item.Cooldown > 0 ? "Cooldown: " + item.Cooldown : "";
    }

    internal void Setup(SkillSlot slot)
    {
        GetComponentInChildren<TMP_Text>().text = slot.Skill.name;
        GetComponent<Button>().onClick.AddListener(delegate { manager.SkillToRemove(slot); });

        SetupToolTip();

        tooltip.Tip = slot.Skill.Description ?? "";
        tooltip.Title = slot.Skill.name;
        tooltip.Extra = slot.Skill.Cooldown > 0 ? "On Cooldown: " + slot.Skill.Cooldown : "";
    }

    private void SetupToolTip()
    {
        tooltip = GetComponent<HoverTooltip>();
        tooltip.showLocation = new Vector3(transform.parent.position.x, transform.parent.position.y - 150);
        tooltip.Side = Side.None;
        tooltip.Tip = "There's nothing here...";
        tooltip.Title = "Empty";
        tooltip.hoverManager = gameObject.GetComponentInParent<HoverManager>();
    }
}
