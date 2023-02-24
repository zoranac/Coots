
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WeightedInteractables
{
    public GameObject Interactable;
    public int Weight;
    public bool ScaleWithFloor;
    public int MaxWeight;
}

public class Grid : MonoBehaviour
{
    public GameObject WallPrefab;
    public Player Player;
    public GameObject Stairs;

    public List<GameObject> EnemyPrefabs;
    public List<GameObject> enemies = new List<GameObject>();
    public int EnemyCount = 5;

    public List<WeightedInteractables> interactables;

    private GameObject staticThings;// = new List<GameObject>();

    public Node[,] Nodes;

    public int BasexSize;
    public int BaseySize;

    public int xSize;
    public int ySize;
    // Start is called before the first frame update
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MainCamera");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);

            Generate();
            //SceneManager.sceneLoaded += OnSceneLoaded;
          //  SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
    }

   /* void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            Unload();
            Generate();
        }
    }*/

    void Unload()
    {
        if (Nodes == null)
            return;


            //for (int x = 0; x < staticThings.Count; x++)
            //{
            //Destroy(staticThings);
            //}

        for (int x = 0; x < Nodes.GetLength(0); x++)
        {
            for (int y = 0; y < Nodes.GetLength(1); y++)
            {
                if (Nodes[x, y].ItemOnMe != null)
                    Destroy(Nodes[x, y].ItemOnMe.gameObject);

                Nodes[x, y].ItemOnMe = null;
                Nodes[x, y].ThingOnMe = null;
            }
        }

            //Nodes = new Node[xSize, ySize];
        
    }

    public void ClearEnemies()
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        enemies.Clear();
    }

    public void Generate()
    {
        ClearEnemies();
        Unload();
        staticThings = GameObject.Find("StaticThings");

        int mod = Random.Range(-10, 11);

        xSize = BasexSize + mod;
        ySize = BaseySize + Mathf.RoundToInt(mod/2);

        var map = GetComponent<MapGenerator>().GenerateMap(xSize, ySize);

        bool playerPlaced = false;
        bool stairsPlaced = false;
  
        Nodes = new Node[xSize, ySize];

        var color = WallPrefab.GetComponent<SpriteRenderer>().color;

        color = new Color(Mathf.Min(color.r + .01f * Player.Floor, 1), color.g, color.b, color.a);

        for (int x = 0; x < Nodes.GetLength(0); x++)
        {
            for (int y = 0; y < Nodes.GetLength(1); y++)
            {
                //Setup Node
                Nodes[x, y] = new Node();

                Nodes[x, y].XY = new Vector2(x, y);

                Nodes[x, y].WorldPos = Nodes[x, y].XY;

                //Create map wall
                if (map[x,y] == 1)  // ((x == 0 || y == 0 || x == xSize-1 || y == ySize-1) && Nodes[x, y].ThingOnMe == null)
                {
                    var g = Instantiate(WallPrefab, staticThings.transform).GetComponent<Wall>();
                    g.GetComponent<SpriteRenderer>().color = color;
                    Nodes[x, y].SetThingOnMe(g, true);
                }
                //Create Player
                else if (!playerPlaced)
                {
                    Nodes[x, y].SetThingOnMe(Player, true);
                    playerPlaced = true;
                }
                //Test to Create Stairs
                else if (!stairsPlaced && x > Mathf.RoundToInt(xSize / 2))
                {
                    int r = Random.Range(0, xSize*10);
                    if (r < x)
                    {
                        Nodes[x, y].SetThingOnMe(Instantiate(Stairs, staticThings.transform).GetComponent<Stairs>(), true);
                        stairsPlaced = true;

                        Nodes[x, y].ThingOnMe.transform.SetParent(staticThings.transform);
                    }
                }
            }
        }

        //Create Stairs if not already made
        while (!stairsPlaced)
        {
            int xRand = Random.Range(Mathf.Max(0,Mathf.RoundToInt(xSize / 2)-Mathf.RoundToInt(xSize/10)), xSize);
            int yRand = Random.Range(0, ySize);

            if (Nodes[xRand, yRand].ThingOnMe == null)
            {
                Nodes[xRand, yRand].SetThingOnMe(Instantiate(Stairs, staticThings.transform).GetComponent<Stairs>(), true);
                stairsPlaced = true;
            }
        }

        //Create Border
        for (int x = -1; x <= Nodes.GetLength(0); x++)
        {
            for (int y = -1; y <= Nodes.GetLength(1); y++)
            {
                if (y != -1 && x != -1 && x != Nodes.GetLength(0))
                    y = Nodes.GetLength(1);

                var g = Instantiate(WallPrefab, staticThings.transform);
                g.GetComponent<SpriteRenderer>().color = color;
                g.transform.position = new Vector3(x, y);
            }
        }

        Camera.main.transform.position = new Vector3((float)xSize / 2, (float)ySize / 2, -10);

        Camera.main.orthographicSize = Mathf.Min(xSize, ySize) - (Mathf.Max(xSize, ySize)/10);

        //Create Interactables
        int numberOfInteractables = Random.Range(3, 6);
        int interactablesPlaced = 0;
        int floor = Player.Floor;
        List<GameObject> interactablesToCreate = new List<GameObject>();

        int totalWeight = 0;
        List<GameObject> entries = new List<GameObject>();

        foreach (var interactable in interactables)
        {
            int weight = interactable.Weight + Mathf.Min((interactable.ScaleWithFloor ? floor : 0), interactable.MaxWeight);

            totalWeight += weight;

            for (int j = 0; j < weight; j++)
            {
                entries.Add(interactable.Interactable);
            }
        }

        if (entries.Count <= 0)
            return;

        for (int i = 0; i < numberOfInteractables; i++)
        {
            var interact = entries[Random.Range(0, totalWeight)];

            //only 1 exp shrine can generate per level
            if (interact.name == "Exp Shrine")
            {
                entries.RemoveAll(x => x.name == "Exp Shrine");
                totalWeight -= interactables.Find(x => x.Interactable.name == "Exp Shrine").Weight;
            }

            interactablesToCreate.Add(interact);
        }

        while (interactablesPlaced < interactablesToCreate.Count)
        {
            int xRand = Random.Range(0, xSize);
            int yRand = Random.Range(0, ySize);

            if (Nodes[xRand, yRand].ThingOnMe == null)
            {
                Nodes[xRand, yRand].SetThingOnMe(Instantiate(interactablesToCreate[interactablesPlaced], staticThings.transform).GetComponent<Interactable>(), true);
                interactablesPlaced++;
            }
        }

        //Create Enemies
        List<GameObject> enemiesToCreate = new List<GameObject>();

        int totalEnemies = EnemyCount;
        if (mod <= -7)
        {
            totalEnemies--;
        }
        else if (mod >= 7)
        {
            totalEnemies++;
        }
        totalWeight = 0;
        entries = new List<GameObject>();

        foreach (var enemyG in EnemyPrefabs)
        {
            var enemy = enemyG.GetComponent<Enemy>();

            int weight = enemy.BaseSpawnWeight + Mathf.Min((enemy.ScaleWeightWithFloor ? floor : 0), enemy.MaxSpawnWeight);

            if (weight <= 0)
                continue;

            totalWeight += weight;

            for (int j = 0; j < weight; j++)
            {
                entries.Add(enemyG);
            }
        }

        if (entries.Count <= 0)
            return;


        for (int i = 0; i < EnemyCount; i++)
        {
            enemiesToCreate.Add(entries[Random.Range(0, totalWeight)]);
        }


        while (enemies.Count < EnemyCount)
        {
            int xRand = Random.Range(0, xSize);
            int yRand = Random.Range(0, ySize);

            if (Nodes[xRand,yRand].ThingOnMe == null)
            {
                var g = Instantiate(enemiesToCreate[enemies.Count], Nodes[xRand, yRand].WorldPos, Quaternion.identity);
                Nodes[xRand, yRand].SetThingOnMe(g.GetComponent<Enemy>(),true);
                enemies.Add(g);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
