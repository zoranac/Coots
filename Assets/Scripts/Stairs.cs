using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stairs : Interactable
{
    public override void DoInteraction()
    {
        //SceneManager.UnloadSceneAsync("SampleScene");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Floor++;
        LevelLoader.i.ReloadLevel("SampleScene", LoadSceneMode.Additive);

        //Camera.main.GetComponent<Grid>().Generate();
    }
}
