using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    public Player player;
    CombatHandler combatHandler;
    public GameObject OptionButtonPrefab;
    public GameObject OptionSpawnPoint;
    public GameObject Canvas;
    List<GameObject> buttons = new List<GameObject>();

    public HealthBar healthbar;
    public HealthBar enemyhealthbar;

    public GameObject ItemsButton;
    public GameObject SkillsButton;

    float waitTime = 2f;
    float startWaitTime = 0;
    bool waiting = false;

    public Animator VFX;
    public Animator VFXEnemy;

    public List<string> enemyNames = new List<string>();

    int tabOpened = -1;

    public void ShowItemOptions()
    {
        if (tabOpened == 1)
        {
            tabOpened = -1;
            clearOptions();
            return;
        }

        int spacer = 0;
        tabOpened = 1;
        foreach (var button in buttons)
        {
            Destroy(button);
        }

        buttons.Clear();

        foreach (var item in player.ItemSlotUIs)
        {
            if (item.Item == null)
                continue;

            var g = Instantiate(OptionButtonPrefab);
            g.transform.SetParent(Canvas.transform);

            g.transform.position = OptionSpawnPoint.transform.position + new Vector3(0,-spacer);

            g.transform.parent = OptionSpawnPoint.transform;

            g.GetComponentInChildren<TMP_Text>().text = item.Item.Name;

            g.GetComponent<Button>().onClick.AddListener(delegate { UseItem(item); });

            g.GetComponent<OptionButtonUI>().SetSkill(item);

            buttons.Add(g);

            spacer += 60;
        }

        if (buttons.Count > 0)
        {
            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(buttons[0]);
        }
    }

    public void ShowSkillOptions()
    {
        if (tabOpened == 0)
        {
            tabOpened = -1;
            clearOptions();
            return;
        }

        int spacer = 0;
        tabOpened = 0;
        clearOptions();

        int index = 0;

        foreach (var skill in player.Skills)
        {
            var g = Instantiate(OptionButtonPrefab);
            g.transform.SetParent(Canvas.transform);

            g.transform.position = OptionSpawnPoint.transform.position + new Vector3(0, -spacer);

            g.transform.parent = OptionSpawnPoint.transform;

            g.GetComponent<Button>().interactable = skill.CurrentCoolDown > 0 ? false : true;

            if (skill.CurrentCoolDown > 0)
            {
                var color = g.GetComponentInChildren<TMP_Text>().color;

                g.GetComponentInChildren<TMP_Text>().color = new Color(color.r, color.g, color.b, .33f);
            }

            g.GetComponent<Button>().onClick.AddListener(delegate { UseSkill(skill); });

            g.GetComponentInChildren<TMP_Text>().text = "[" + (index + 1) + "] " +  skill.Skill.name + (skill.CurrentCoolDown > 0 ? " CD ("+ skill.CurrentCoolDown + ")":"");

            g.GetComponent<OptionButtonUI>().SetSkill(skill);

           buttons.Add(g);
            index++;
            spacer += 60;
        }

        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(buttons[0]);
    }

    void clearOptions()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<OptionButtonUI>().RemoveItem();
            Destroy(button);
        }

        buttons.Clear();
    }
    public void UseItem(ItemSlotUI item)
    {
        var result = item.UseItem(player);

        if (result != Result.Miss)
        {
            if (item.Item.Skill.ApplyVFXToSelf)
                VFX.Play(item.Item.Skill.VFX);
            else
                VFXEnemy.Play(item.Item.Skill.VFX);
        }

        if (item.Uses <= 0)
        {
            player.RemoveItem(item.Item);
        }

        DidSomething();
    }


    public void UseSkill(SkillSlot slot)
    {
        var result = slot.Skill.UseSkill(player, Camera.main.GetComponent<CombatHandler>().CurrentEnemy);

        if (result != Result.Miss)
        {
            if (slot.Skill.ApplyVFXToSelf)
                VFX.Play(slot.Skill.VFX);
            else
                VFXEnemy.Play(slot.Skill.VFX);
        }

        slot.CurrentCoolDown = slot.Skill.Cooldown + 1;

        DidSomething();
    }

    void DidSomething()
    {
        foreach (var skillSlot in player.Skills)
        {
            skillSlot.CurrentCoolDown--;

            if (skillSlot.CurrentCoolDown < 0)
                skillSlot.CurrentCoolDown = 0;
        }

        foreach (var effect in player.StatusEffects.ToList())
        {
            effect.StatusEffect.DoThing(effect.User, player);
        }

        combatHandler.HandleEnemyTurn();

        clearOptions();
        player.ReduceStatusEffectDurations();

        startWaiting();
    }

    void startWaiting()
    {
        waiting = true;
        startWaitTime = Time.time;
        SkillsButton.SetActive(false);
        ItemsButton.SetActive(false);
    }

    void stopWaiting()
    {
        if (player.StatusEffects.Exists(x => x.StatusEffect is Sleep))
        {
            player.CreatePopup("Sleeping", new Color(.75f, .75f, .75f, 1), true);
            DidSomething();
            return;
        }

        waiting = false;
        startWaitTime = 0;
        SkillsButton.SetActive(true);
        ItemsButton.SetActive(true);
        if (tabOpened == 0)
        {
            tabOpened = -1;
            ShowSkillOptions();
            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(buttons[0]);

        }
        else if (tabOpened == 1)
        {
            tabOpened = -1;
            ShowItemOptions();
            if (buttons.Count > 0)
                GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(buttons[0]);
            else
                GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(SkillsButton);
        }
        else
        {
            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(SkillsButton);
        }

    }

    // Start is called before the first frame update
    public void OnEnable()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        // player = GetComponent<Player>();
        combatHandler = Camera.main.GetComponent<CombatHandler>();

        healthbar.creature = player;
        enemyhealthbar.creature = combatHandler.CurrentEnemy;
        GameObject.Find("EnemyName").GetComponent<TMP_Text>().text = /*enemyNames[Random.Range(0, enemyNames.Count)] + ", The " +*/ combatHandler.CurrentEnemy.Name;
        GameObject.Find("EnemyImage").GetComponent<Image>().sprite = combatHandler.CurrentEnemy.BattleSprite;

        combatHandler.VFXEnemy = VFXEnemy;
        combatHandler.VFX = VFX;

        MusicManager.i.StartBattle();

        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(SkillsButton);
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            if(combatHandler.CurrentEnemy != null && Time.time >= startWaitTime + waitTime)
            {
                stopWaiting();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (tabOpened == 0)
                {
                    clearOptions();
                    tabOpened = -1;
                }
                else
                    ShowSkillOptions();
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                if (tabOpened == 1)
                {
                    clearOptions();
                    tabOpened = -1;
                }
                else
                    ShowItemOptions();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (tabOpened == 0 && player.Skills.Count >= 1)
                {
                    UseSkill(player.Skills[0]);
                }
                else if(tabOpened == 1 && player.ItemSlotUIs.Count >= 1 && player.ItemSlotUIs[0].Item != null)
                {
                    UseItem(player.ItemSlotUIs[0]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (tabOpened == 0 && player.Skills.Count >= 2)
                {
                    UseSkill(player.Skills[1]);
                }
                else if (tabOpened == 1 && player.ItemSlotUIs.Count >= 2 && player.ItemSlotUIs[1].Item != null)
                {
                    UseItem(player.ItemSlotUIs[1]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (tabOpened == 0 && player.Skills.Count >= 3)
                {
                    UseSkill(player.Skills[2]);
                }
                else if (tabOpened == 1 && player.ItemSlotUIs.Count >= 3 && player.ItemSlotUIs[2].Item != null)
                {
                    UseItem(player.ItemSlotUIs[2]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (tabOpened == 0 && player.Skills.Count >= 4)
                {
                    UseSkill(player.Skills[3]);
                }
                else if (tabOpened == 1 && player.ItemSlotUIs.Count >= 4 && player.ItemSlotUIs[3].Item != null)
                {
                    UseItem(player.ItemSlotUIs[3]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (tabOpened == 0 && player.Skills.Count >= 5)
                {
                    UseSkill(player.Skills[4]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                if (tabOpened == 0 && player.Skills.Count >= 6)
                {
                    UseSkill(player.Skills[5]);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                if (tabOpened == 0 && player.Skills.Count >= 7)
                {
                    UseSkill(player.Skills[6]);
                }
            }
        }
    }
}
