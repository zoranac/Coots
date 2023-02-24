using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public UnityEvent OnAction = new UnityEvent();

    Grid Grid;
    CombatHandler combatHandler;
    Player player;

    float waitTime = .25f;
    float startWaitTime = 0;
    bool waiting = false;

    public AudioClip Step;
    // Start is called before the first frame update
    void Start()
    {
        Grid = Camera.main.GetComponent<Grid>();
        combatHandler = Camera.main.GetComponent<CombatHandler>();
        player = GetComponent<Player>();

        OnAction.AddListener(StartWaiting);
        SceneManager.activeSceneChanged += checkAsignments;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= checkAsignments;
    }

    void checkAsignments(Scene from, Scene to)
    {
        Grid = Camera.main.GetComponent<Grid>();
        combatHandler = Camera.main.GetComponent<CombatHandler>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            if (Time.time >= startWaitTime + waitTime)
                waiting = false;
        }
        else if (combatHandler.CurrentEnemy == null && !player.LevelingUp && !LevelLoader.i.Loading)
        {
            GetKeyInput();
        }
    }


    void GetKeyInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.position.y + 1 >= Grid.ySize)
                return;

            var thing = Grid.Nodes[(int)transform.position.x, (int)transform.position.y + 1].ThingOnMe;

            if (thing == null)
            {
                DoMove(new Vector2(transform.position.x, transform.position.y + 1));
                return;
            }

            else if (thing is Interactable)
            {
                DoInteraction(thing as Interactable);
                return;
            }

            else if (thing is Enemy)
            {
                DoCombat(thing as Enemy);
                return;
            }
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.position.y - 1 < 0)
                return;

            var thing = Grid.Nodes[(int)transform.position.x, (int)transform.position.y - 1].ThingOnMe;

            if (thing == null)
            {
                DoMove(new Vector2(transform.position.x, transform.position.y - 1));
                return;
            }

            else if (thing is Interactable)
            {
                DoInteraction(thing as Interactable);
                return;
            }

            else if (thing is Enemy)
            {
                DoCombat(thing as Enemy);
                return;
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x - 1 < 0)
                return;

            var thing = Grid.Nodes[(int)transform.position.x - 1 , (int)transform.position.y].ThingOnMe;

            if (thing == null)
            {
                DoMove(new Vector2(transform.position.x - 1, transform.position.y));
                return;
            }

            else if (thing is Interactable)
            {
                DoInteraction(thing as Interactable);
                return;
            }

            else if (thing is Enemy)
            {
                DoCombat(thing as Enemy);
                return;
            }
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x + 1 >= Grid.xSize)
                return;

            var thing = Grid.Nodes[(int)transform.position.x + 1, (int)transform.position.y].ThingOnMe;

            if (thing == null)
            {
                DoMove(new Vector2(transform.position.x + 1, transform.position.y));
                return;
            }

            else if (thing is Interactable)
            {
                DoInteraction(thing as Interactable);
                return;
            }

            else if (thing is Enemy)
            {
                DoCombat(thing as Enemy);
                return;
            }
        }
    }

    void DoMove(Vector2 xy)
    {
        if (Grid.Nodes[(int)xy.x, (int)xy.y].ItemOnMe != null)
        {
            if (player.TryAddItem(Grid.Nodes[(int)xy.x, (int)xy.y].ItemOnMe.Item))
            {
                Destroy(Grid.Nodes[(int)xy.x, (int)xy.y].ItemOnMe.gameObject);
                Grid.Nodes[(int)xy.x, (int)xy.y].ItemOnMe = null;
            }
        }

        Grid.Nodes[(int)xy.x, (int)xy.y].SetThingOnMe(player);
        Camera.main.GetComponent<SoundManager>().PlayAudio(Step);
        OnAction.Invoke();
    }

    void DoInteraction(Interactable interactable)
    {
        interactable.DoInteraction();
        OnAction.Invoke();
    }

    void DoCombat(Enemy enemy)
    {
        combatHandler.StartCombat(enemy);
        OnAction.Invoke();
    }


    void StartWaiting()
    {
        waiting = true;
        startWaitTime = Time.time;
    }
}
