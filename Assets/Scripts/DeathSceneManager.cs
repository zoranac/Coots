using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSceneManager : MonoBehaviour
{
    public void Restart()
    {
        Camera.main.GetComponent<Grid>().ClearEnemies();
        LevelLoader.i.LoadLevel("SampleScene",LoadSceneMode.Single);
    } 

    public void Quit()
    {
        Application.Quit();
    }

}
