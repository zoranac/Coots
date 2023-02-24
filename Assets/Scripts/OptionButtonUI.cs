using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButtonUI : MonoBehaviour
{
    public Skill Skill;

    HoverTooltip tooltip;
    private void Setup()
    {
        tooltip = GetComponent<HoverTooltip>();
        tooltip.showLocation = new Vector3((GetComponent<RectTransform>().rect.width/2f + 150) + transform.position.x, transform.position.y);
        tooltip.Side = Side.None;
        tooltip.Tip = "There's nothing here...";
        tooltip.Title = "Empty";
        tooltip.hoverManager = gameObject.GetComponentInParent<HoverManager>();
    }

    public void SetSkill(SkillSlot slot)
    {
        Setup();

        Skill = slot.Skill;
        tooltip.Tip = slot.Skill.Description ?? "";
        tooltip.Title = slot.Skill.name;
        tooltip.Extra = slot.CurrentCoolDown > 0 ? "On Cooldown: " + slot.CurrentCoolDown : "";
    }

    public void SetSkill(ItemSlotUI item)
    {
        Setup();

        Skill = item.Item.Skill;
        tooltip.Tip = item.Item.Skill.Description ?? "";
        tooltip.Title = item.Item.Skill.name;
        tooltip.Extra = item.Uses > 0 ? "Uses: " + item.Uses : "";
    }


    public void RemoveItem()
    {
        Skill = null;
        tooltip.Tip = "There's nothing here...";
        tooltip.Title = "Empty";
        tooltip.Remove();   
    }

}
