using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Den : Interactable
{
    public List<GameObject> DenEnemies = new List<GameObject>();

    public override void DoInteraction()
    {
        var g = Instantiate(DenEnemies[Random.Range(0, DenEnemies.Count)], transform.position, Quaternion.identity).GetComponent<Enemy>();
        Camera.main.GetComponent<Grid>().Nodes[(int)transform.position.x, (int)transform.position.y].SetThingOnMe(g);
        Camera.main.GetComponent<Grid>().enemies.Add(g.gameObject);
        Camera.main.GetComponent<CombatHandler>().StartCombat(g);
        Destroy(gameObject);
    }
}
