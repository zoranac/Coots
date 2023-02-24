using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject TitleObj;

   public void Begin()
    {
        SceneManager.LoadScene("Introduction");
    }

    public void Credits()
    {
        toggleTitle(false);
        SceneManager.LoadScene("Credits", LoadSceneMode.Additive);
        SceneManager.sceneUnloaded += activeSceneChange;
    }

    void activeSceneChange (Scene from)
    {
        if (from.name == "Credits")
        {
            toggleTitle(true);
            SceneManager.sceneUnloaded -= activeSceneChange;
        }
    }

    void toggleTitle(bool value)
    {
        TitleObj.SetActive(value);
    }

}
