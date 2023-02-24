using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpShrine : Interactable
{
    public Sprite UsedSprite;
    bool used;

    public override void DoInteraction()
    {
        if (!used)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.GainExp(50);
            used = true;
            GetComponent<SpriteRenderer>().sprite = UsedSprite;
            PopupText.Create(player.WarningSpawnPoint.transform.position, "Your paws tingle as the power of the stone flows into you.", Color.white, 3, "ExplorationScene");
        }
    }
}
