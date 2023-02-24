using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject UIRootObject;

    public Animator Transition;
    //public Animator CameraTransition;
    public float TransitionTime = 1;

    public static LevelLoader i;

    public bool Loading;

    private AsyncOperation sceneAsync;

    public void LoadLevel(string scene, LoadSceneMode mode)
    {
        Transition.ResetTrigger("End");
        Transition.ResetTrigger("Start");
        Loading = true;
        StartCoroutine(loadLevel(scene, mode));
    }

    public void UnloadLevel(string scene)
    {
        Transition.ResetTrigger("End");
        Transition.ResetTrigger("Start");
        Loading = true;
        StartCoroutine(unloadLevel(scene));
    }

    public void ReloadLevel(string scene, LoadSceneMode mode)
    {
        Transition.ResetTrigger("End");
        Transition.ResetTrigger("Start");
        Loading = true;
        StartCoroutine(reloadLevel(scene, mode));
    }

    IEnumerator reloadLevel(string scene, LoadSceneMode mode)
    {
      
        Transition.SetTrigger("Start");
        //CameraTransition.SetTrigger("Start");
        yield return new WaitForSeconds(TransitionTime);


        AsyncOperation _scene = SceneManager.LoadSceneAsync(scene, mode);
        _scene.allowSceneActivation = false;
        sceneAsync = _scene;

        while (_scene.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + _scene.progress);
            yield return null;
        }

        //Activate the Scene
        sceneAsync.allowSceneActivation = true;

        while (!_scene.isDone)
        {
            // wait until it is really finished
            yield return null;
        }

        enableScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));

        var unload = SceneManager.UnloadSceneAsync(scene);

        while (!unload.isDone)
        {
            // wait until it is really finished
            yield return null;
        }

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            Camera.main.GetComponent<Grid>().Generate();
        }

        yield return new WaitForSeconds(1f);

        Transition.SetTrigger("End");

        Loading = false;
        //CameraTransition.SetTrigger("End");
    }

    IEnumerator loadLevel(string scene, LoadSceneMode mode)
    {
        Transition.SetTrigger("Start");
        //CameraTransition.SetTrigger("Start");
        yield return new WaitForSeconds(TransitionTime);

        AsyncOperation _scene = SceneManager.LoadSceneAsync(scene, mode);
        _scene.allowSceneActivation = false;
        sceneAsync = _scene;

        while (_scene.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + _scene.progress);
            yield return null;
        }

        //Activate the Scene
        sceneAsync.allowSceneActivation = true;

        while (!_scene.isDone)
        {
            // wait until it is really finished
            yield return null;
        }

        enableScene(SceneManager.GetSceneAt(SceneManager.sceneCount-1));

        Transition.SetTrigger("End");
        //CameraTransition.SetTrigger("End");

        Loading = false;
    }

    IEnumerator unloadLevel(string scene)
    {
        Transition.SetTrigger("Start");
        //CameraTransition.SetTrigger("Start");
        yield return new WaitForSeconds(TransitionTime);

        var unload = SceneManager.UnloadSceneAsync(scene);

        SceneManager.MoveGameObjectToScene(UIRootObject, SceneManager.GetSceneByName("SampleScene"));

        while (!unload.isDone)
        {
            // wait until it is really finished
            yield return null;
        }

        Transition.SetTrigger("End");
        //CameraTransition.SetTrigger("End");

        Loading = false;
    }

    void enableScene(Scene scene)
    {
        if (scene.IsValid())
        {
            Debug.Log("Scene is Valid");
            SceneManager.MoveGameObjectToScene(UIRootObject, scene);
            SceneManager.SetActiveScene(scene);
        }
    }

    private void Awake()
    {
        if (i != null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            i = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
