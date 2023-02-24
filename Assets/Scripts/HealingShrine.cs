using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingShrine : Interactable
{
    public Sprite UsedSprite;
    bool used;

    public override void DoInteraction()
    {
        if (!used)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.currentHP = player.MaxHP;
            used = true;
            GetComponent<SpriteRenderer>().sprite = UsedSprite;
            PopupText.Create(player.WarningSpawnPoint.transform.position, "You meow at the shrine and your wounds are healed.", new Color32(244, 255, 167, 255), 3, "ExplorationScene");
        }
    }
}
