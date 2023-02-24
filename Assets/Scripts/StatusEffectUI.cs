using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectUI : MonoBehaviour
{
    public StatusEffect StatusEffect;

    HoverTooltip tooltip;
    private void Setup(bool toLeft)
    {
        tooltip = GetComponent<HoverTooltip>();
        if (toLeft)
            tooltip.showLocation = new Vector3(GetComponent<RectTransform>().parent.transform.position.x-200, GetComponent<RectTransform>().parent.transform.position.y);
        else
            tooltip.showLocation = new Vector3(GetComponent<RectTransform>().parent.transform.position.x+200, GetComponent<RectTransform>().parent.transform.position.y-75);

        tooltip.Side = Side.None;
        tooltip.Tip = "There's nothing here...";
        tooltip.Title = "Empty";
        tooltip.Extra = "";
        tooltip.hoverManager = gameObject.transform.parent.GetComponentInParent<HoverManager>();
    }

    public void SetStatusEffect(StatusEffect statusEffect, bool toLeft)
    {
        Setup(toLeft);

        StatusEffect = statusEffect;
        tooltip.Tip = statusEffect.Description;
        tooltip.Title = statusEffect.name;
    }



}
