using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    public string Name;
    public List<ConditionalSkill> conditionalSkills;
    PlayerMovement playerMovement;
    GameObject playerGO;
    Player player;
    CombatHandler combatHandler;
    int restProgress = 0;

    Node previousNode;

    public bool Fleeing;
    public bool NotMoving;
    public int BaseSpawnWeight;
    public int MaxSpawnWeight;
    public bool ScaleWeightWithFloor;
    public int ExpValue;

    public List<ItemDrop> ItemDrops;

    public bool IsDead = false;

    public Transform pfItemPickup;

    // Start is called before the first frame update
    protected override void Start()
    {
        playerGO = GameObject.Find("Player");
        playerMovement = playerGO.GetComponent<PlayerMovement>();
        playerMovement.OnAction.AddListener(StartMove);
        player = playerGO.GetComponent<Player>();

        combatHandler = Camera.main.GetComponent<CombatHandler>();

        base.Start();
    }

    public override void Die()
    {
        if (!IsDead)
        {
            base.Die();
            if (Grid.Nodes[(int)transform.position.x, (int)transform.position.y].ItemOnMe == null)
                HandleItemDrops();
            Camera.main.GetComponent<Grid>().enemies.Remove(gameObject);
            //Camera.main.GetComponent<CombatHandler>().EnemyDeath();
            IsDead = true;
        }
    }

    void HandleItemDrops()
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
            return;

        Item item = entries[Random.Range(0, totalWeight)];

        if (item != null)
        {
            var g = Instantiate(pfItemPickup, transform.position, Quaternion.identity);
            g.GetComponent<ItemPickup>().Setup(item, Grid.Nodes[(int)transform.position.x, (int)transform.position.y]);
            g.SetParent(GameObject.Find("Root").transform);
        }
        
    }

    void StartMove()
    {
        if (combatHandler.CurrentEnemy == null && !NotMoving)
        {
            StopCoroutine(startMove());
            StartCoroutine(startMove());
        }
    }

    IEnumerator startMove()
    {
        //Determine if resting
        restProgress += player.Speed - Speed;

        if (restProgress >= 100)
        {
            restProgress -= 100;
            yield break;
        }

        //Otherwise Move
        DoMove();

        yield return new WaitForSeconds(.2f);

        //Determine if taking second move
        if (restProgress <= -100)
        {
            restProgress += 100;
            DoMove();
        }
    }

    void DoMove()
    {
        List<Node> moveOptions = new List<Node>();

        bool stop = false;

        //Determine Move Options
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x == 0 && y == 0) || (x!=0 && y!=0))
                    continue;

                if ((int)transform.position.x + x >= Grid.xSize || (int)transform.position.x + x < 0 || (int)transform.position.y + y >= Grid.ySize || (int)transform.position.y + y < 0)
                    continue;

                if (Grid.Nodes[(int)transform.position.x + x, (int)transform.position.y + y].ThingOnMe == null)
                {
                    if (previousNode == null || (previousNode != null && Grid.Nodes[(int)transform.position.x + x, (int)transform.position.y + y] != previousNode))
                    {
                        moveOptions.Add(Grid.Nodes[(int)transform.position.x + x, (int)transform.position.y + y]);
                    }
                }
                else if (Grid.Nodes[(int)transform.position.x + x, (int)transform.position.y + y].ThingOnMe == player)
                {
                    moveOptions.Clear();

                    if (Fleeing)
                        moveOptions.Add(Grid.Nodes[(int)transform.position.x - x, (int)transform.position.y - y]);

                    else
                        moveOptions.Add(Grid.Nodes[(int)transform.position.x + x, (int)transform.position.y + y]);

                    stop = true;
                    break;
                }
            }

            if (stop)
                break;
        }

        if (moveOptions.Count <= 0)
        {
            if (previousNode.ThingOnMe == null || (previousNode.ThingOnMe == player && !Fleeing))
                moveOptions.Add(previousNode);
            else
                return;
        }

        Node node = moveOptions[Random.Range(0, moveOptions.Count)];

        if (node.ThingOnMe == null)
        {
            previousNode = Grid.Nodes[(int)transform.position.x, (int)transform.position.y];
            node.SetThingOnMe(this);
            return;
        }
        else if (node.ThingOnMe is Player)
        {
            combatHandler.StartCombat(this, false);
            return;
        }

    }
}
