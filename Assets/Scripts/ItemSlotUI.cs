using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item Item;
    public int Uses = 0;
    public Sprite empty;

    public Button destroy;
    HoverTooltip tooltip;
    private void Start()
    {
        tooltip = GetComponent<HoverTooltip>();
        tooltip.Tip = "There's nothing here...";
        tooltip.Title = "Empty";

        destroy.onClick.AddListener(delegate { GameObject.Find("Player").GetComponent<Player>().RemoveItem(Item); });
        destroy.gameObject.SetActive(false);
    }

    public void SetItem(Item item)
    {
        Item = item;
        GetComponent<Image>().sprite = item.Icon;
        tooltip.Tip = item.Description;
        tooltip.Title = item.Name;
        Uses = item.BaseUses;
        tooltip.Extra = Uses > 0 ? "Uses: " + Uses : "";
    }

    public void RemoveItem(Item item)
    {
        if (item == Item)
        {
            Item = null;
            GetComponent<Image>().sprite = empty;
            tooltip.Tip = "There's nothing here...";
            tooltip.Title = "Empty";
            tooltip.Extra = "";
            destroy.gameObject.SetActive(false);
        }
    }

    public Result UseItem(Player player)
    {
        var returnResult = Item.UseItem(player);

        Uses--;

        tooltip.Extra = Uses > 0 ? "Uses: " + Uses : "";

        return returnResult;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null)
        {
            destroy.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        destroy.gameObject.SetActive(false);
    }

}
