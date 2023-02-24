using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpManager : MonoBehaviour
{
    Player player;
    public GameObject StatBump;
    public GameObject LearnSkillLayout;
    public GameObject RemoveSkillLayout;
    public List<Skill> AllPlayerSkillOptions = new List<Skill>();

    public List<Skill> CurrentOptions = new List<Skill>();

    List<GameObject> optionGameObjects = new List<GameObject>();
    List<GameObject> removalGameObjects = new List<GameObject>();

    public GameObject pfSkillOption;

    Skill skillToAdd;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Strength"));
        //increase hp by a set amount
        player.MaxHP += 10;

        StatBump.SetActive(true);

        var options = AllPlayerSkillOptions.Where(x => !player.Skills.Exists(y => y.Skill == x)).ToArray();

        List<int> indexes = new List<int>();

        if (options.Count() > 3)
        {
            while (CurrentOptions.Count < 3)
            {
                int rand = Random.Range(0, options.Count());

                if (indexes.Contains(rand))
                    continue;

                indexes.Add(rand);
                CurrentOptions.Add(options[rand]);
            }
        }
        else
        {
            CurrentOptions.AddRange(options);
        }

        foreach (var item in CurrentOptions)
        {
            var g = Instantiate(pfSkillOption);
            g.transform.SetParent(LearnSkillLayout.transform);
            g.GetComponent<LearnSkillButtonUI>().Setup(item);
            g.GetComponent<LearnSkillButtonUI>().manager = this;
            optionGameObjects.Add(g);
        }

        if (player.Skills.Count >= 6)
        {
            int i = 0;
            foreach (var slot in player.Skills)
            {
                i++;
                //Cant replace the starting skills
                if (i <= 3)
                {
                    continue;
                }

                var g = Instantiate(pfSkillOption);
                g.transform.SetParent(RemoveSkillLayout.transform);
                g.GetComponent<LearnSkillButtonUI>().Setup(slot);
                g.GetComponent<LearnSkillButtonUI>().manager = this;
                removalGameObjects.Add(g);
            }
        }

    }

    public void BumpSTR()
    {
        player.Strength += 2;
        StatBump.SetActive(false);
        LearnSkillLayout.transform.parent.gameObject.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(optionGameObjects[0]);
    }

    public void BumpMAG()
    {
        player.Magic += 2;
        StatBump.SetActive(false);
        LearnSkillLayout.transform.parent.gameObject.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(optionGameObjects[0]);
    }

    public void BumpDEF()
    {
        player.Defense += 2;
        StatBump.SetActive(false);
        LearnSkillLayout.transform.parent.gameObject.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(optionGameObjects[0]);
    }

    public void BumpACC()
    {
        player.AccuracyMod += 2;
        StatBump.SetActive(false);
        LearnSkillLayout.transform.parent.gameObject.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(optionGameObjects[0]);
    }

    public void BumpDOD()
    {
        player.Dodge += 2;
        StatBump.SetActive(false);
        LearnSkillLayout.transform.parent.gameObject.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(optionGameObjects[0]);

    }

    public void SkillToAdd(Skill skill)
    {
        skillToAdd = skill;
        LearnSkillLayout.transform.parent.gameObject.SetActive(false);
        if (player.Skills.Count >= 6)
        {
            RemoveSkillLayout.transform.parent.gameObject.SetActive(true);
            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(removalGameObjects[0]);
        }
        else
        {
            player.Skills.Add(new SkillSlot(skillToAdd));
            LevelLoader.i.UnloadLevel("LevelUp");
        }
    }

    public void SkillToRemove(SkillSlot slot)
    {
        player.Skills.Remove(slot);
        player.Skills.Add(new SkillSlot(skillToAdd));
        
        LevelLoader.i.UnloadLevel("LevelUp");
    }

    public void Skip()
    {
        LevelLoader.i.UnloadLevel("LevelUp");
    }
}
