using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopupText : MonoBehaviour
{
    TextMeshProUGUI tmp;
    float disappearTimer;

    public static PopupText Create(Vector3 pos, string text, Color color, float timer = 1, string canvas = "Canvas")
    {
        var popupTransform = Instantiate(GameAssets.i.pfPopup);
        popupTransform.SetParent(GameObject.Find(canvas).transform);
        popupTransform.transform.position = pos + new Vector3(Random.Range(-50,50), Random.Range(-50, 50),0);
        PopupText popup = popupTransform.GetComponent<PopupText>();
        popup.Setup(text, color, timer);
        return popup;
    }
    
    public void Setup(string text, Color color, float timer = 1)
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.SetText(text);
        tmp.color = color;
        disappearTimer = timer;
    }

    // Update is called once per frame
    void Update()
    {
        float moveYSpeed = 5f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a - (disappearSpeed * Time.deltaTime));

            if (tmp.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
