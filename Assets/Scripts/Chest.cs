using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public List<ItemDrop> ItemDrops = new List<ItemDrop>();
    public Sprite UsedSprite;
    bool used;

    public override void DoInteraction()
    {
        if (!used)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            var item = HandleItemDrops();

            if (!player.TryAddItem(item))
            {
                //PopupText.Create(GameObject.Find("WarningSpawnPoint").transform.position, "No room for item...", Color.red, 1, "ExplorationScene");
            }
            else
            {
                used = true;
                GetComponent<SpriteRenderer>().sprite = UsedSprite;
                PopupText.Create(player.WarningSpawnPoint.transform.position, "The chest creaks as it reveals to you its treasure.", new Color32(169, 110, 23, 255), 3, "ExplorationScene");
            }
        }
    }


    Item HandleItemDrops()
    {
        int totalWeight = 0;

        List<Item> entries = new List<Item>();

        foreach (var _item in ItemDrops)
        {
            totalWeight += _item.Weight;

            for (int i = 0; i < _item.Weight; i++)
            {
                entries.Add(_item.item);
            }
        }

        if (entries.Count <= 0)
            return null;

        return entries[Random.Range(0, totalWeight)];

    }

}
