using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Thing
{
    public Item Item;

    public void Setup(Item item, Node node)
    {
        GetComponent<SpriteRenderer>().sprite = item.Icon;
        Item = item;
        node.ItemOnMe = this;
    }
}
