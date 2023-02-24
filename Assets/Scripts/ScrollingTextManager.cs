using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollingTextManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float ScrollSpeed = 2f;

    private TextMeshProUGUI m_CloneTextObj;

    private RectTransform m_textRectTrans;
    private string sourceText;
    private string tempText;
    private bool hasTextChanged;
    private TMP_Text m_TextComponent;
    private void Awake()
    {
        resetClone();
    }

    void resetClone()
    {
        if (m_CloneTextObj != null)
        {
            Destroy(m_CloneTextObj.gameObject);
        }

        m_textRectTrans = textMeshPro.GetComponent<RectTransform>();
        m_TextComponent = textMeshPro.GetComponent<TMP_Text>();
        float width = textMeshPro.preferredWidth + 200;// < 225 ? 225 : textMeshPro.preferredWidth;

        m_CloneTextObj = Instantiate(textMeshPro, m_textRectTrans) as TextMeshProUGUI;
        RectTransform cloneRectTrans = m_CloneTextObj.GetComponent<RectTransform>();
        cloneRectTrans.transform.position = new Vector3(cloneRectTrans.transform.position.x + width / 2, cloneRectTrans.transform.position.y, cloneRectTrans.transform.position.z);
        cloneRectTrans.anchorMin = new Vector2(1, .5f);
        cloneRectTrans.localScale = new Vector3(1, 1, 1);
    }


    private IEnumerator Start()
    {
        float width = textMeshPro.preferredWidth + 100;// < 225 ? 225 : textMeshPro.preferredWidth;

        Vector3 startPos = m_textRectTrans.position;

        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);

        float scrollPos = 0;
        while (true) 
        {
            float y = m_textRectTrans.position.y;

            if (scrollPos > width)
            {
                scrollPos -= width;
            }

            if (hasTextChanged)
            {
                m_textRectTrans.position = startPos;

                m_CloneTextObj.text = textMeshPro.text;
                scrollPos = 0;
                yield return new WaitForFixedUpdate();
                    
                width = textMeshPro.preferredWidth + 100;
                resetClone();
                //RectTransform cloneRectTrans = m_CloneTextObj.GetComponent<RectTransform>();
                //cloneRectTrans.transform.position = m_textRectTrans.position;
                //cloneRectTrans.transform.position = new Vector3(cloneRectTrans.transform.position.x + width / 2, cloneRectTrans.transform.position.y, cloneRectTrans.transform.position.z);
                hasTextChanged = false;
            }
          

            m_textRectTrans.position = new Vector3(startPos.x - (scrollPos % width), y, startPos.z);

            scrollPos += ScrollSpeed * 20 * Time.deltaTime;

            yield return null;
        }
    }

    void ON_TEXT_CHANGED(Object obj)
    {
        if (obj == m_TextComponent)
            hasTextChanged = true;
    }
}
