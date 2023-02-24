using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 XY;
    public Vector2 WorldPos;
    public Thing ThingOnMe;
    public ItemPickup ItemOnMe;

    public void SetThingOnMe(Thing thing, bool newObj = false)
    {
        ThingOnMe = thing;
        if (!newObj && Camera.main.GetComponent<Grid>().Nodes[(int)thing.transform.position.x, (int)thing.transform.position.y] != null)
        {
            Camera.main.GetComponent<Grid>().Nodes[(int)thing.transform.position.x, (int)thing.transform.position.y].ThingOnMe = null;
        }
        thing.transform.position = XY;
    }
}
