using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
   public void Return()
    {
        SceneManager.UnloadSceneAsync("Credits");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Return();
        }
    }
}
