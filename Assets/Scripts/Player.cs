using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Creature
{
    public List<Item> Items = new List<Item>();
    public List<ItemSlotUI> ItemSlotUIs = new List<ItemSlotUI>();
    public int Exp = 0;

    public Transform WarningSpawnPoint;
    public int Floor;

    public bool LevelingUp;

    public bool TryAddItem(Item item)
    {
        if (Items.Count >= 4)
        {
            CreatePopup("Your feline pockets are full.", Color.white, true, WarningSpawnPoint, 3);
            return false;
        }

        Items.Add(item);

        foreach (var slot in ItemSlotUIs)
        {
            if (slot.Item == null)
            {
                slot.SetItem(item);
                break;
            }
        }


        return true;
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
        ItemSlotUIs.Where(x => x.Item == item).ToArray()[0].RemoveItem(item);
    }

    public void GainExp(int exp)
    {
        Exp += exp;
        if (Exp >= 100)
        {
            Exp -= 100;
            LevelingUp = true;
            SceneManager.sceneUnloaded += endLevelUp;
            LevelLoader.i.LoadLevel("LevelUp", LoadSceneMode.Additive);
        }
    }

    public override void Die()
    {
        if (gameObject != null)
        {
            base.Die();

            LevelLoader.i.LoadLevel("Death", LoadSceneMode.Additive);

            Destroy(gameObject);
        }
    }

    protected override void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == "SampleScene" && Floor == 3)
            CreatePopup("How deep does this dungeon go?", Color.white, true, WarningSpawnPoint, 3);
        else if (scene.name == "SampleScene" && Floor == 6)
            CreatePopup("I worry the Lord and Lady are in another dungeon...", Color.white, true, WarningSpawnPoint, 3);
    }

    void endLevelUp(Scene scene)
    {
        if (scene.name == "LevelUp")
        {
            LevelingUp = false;
            SceneManager.sceneUnloaded -= endLevelUp;
        }
    }
}
