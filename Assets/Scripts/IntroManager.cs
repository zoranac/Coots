using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject ScrollText;
    public Transform MoveToPos;
    public float scrollSpeed = .2f;
    public float fastScroll = .5f;

    float _scrollSpeed;

    bool doneScrolling = false;

    public void Proceed()
    {
        Destroy(GameObject.Find("MenuMusic"));

        SceneManager.LoadScene("SampleScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        _scrollSpeed = scrollSpeed;
        StartCoroutine(moveToPos(MoveToPos.position));
    }
    IEnumerator moveToPos(Vector3 pos)
    {
        while (Vector3.Distance(ScrollText.transform.position, pos) > .35f)
        {
            ScrollText.transform.position = Vector3.MoveTowards(ScrollText.transform.position, pos, _scrollSpeed);
            yield return null;
        }

        doneScrolling = true;
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Proceed"));
    }

    private void Update()
    {
        if (!doneScrolling)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _scrollSpeed = fastScroll;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _scrollSpeed = scrollSpeed;
            }
        }
        else
        {
            /*if (Input.GetKeyDown(KeyCode.Space))
            {
                Proceed();
            }*/
        }
    }


}
