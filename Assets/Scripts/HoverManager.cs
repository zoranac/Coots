using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Side
{
    Top,
    Bottom,
    Left,
    Right,
    None
}

public class HoverManager : MonoBehaviour
{
    public TextMeshProUGUI tipTitle;
    public TextMeshProUGUI tipText;
    public TextMeshProUGUI tipExtra;
    public RectTransform tipWindow;

    public Action<string, string, string, Vector2, Side> OnMouseHover;
    public Action OnMouseLoseFocus;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    // Start is called before the first frame update
    void Start()
    {
        HideTip();
    }

    private void ShowTip(string title, string tip, string extra, Vector2 mousPos, Side side)
    {
        tipText.text = tip;
        tipTitle.text = title;
        tipExtra.text = extra;

        //tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);
        tipWindow.gameObject.SetActive(true);
        if (mousPos != Vector2.zero)
        {
            switch (side)
            {
                case Side.Top:
                    tipWindow.transform.position = new Vector2(mousPos.x, mousPos.y + tipWindow.sizeDelta.y * 2);
                    break;
                case Side.Bottom:
                    tipWindow.transform.position = new Vector2(mousPos.x, mousPos.y - tipWindow.sizeDelta.y * 2);
                    break;
                case Side.Left:
                    tipWindow.transform.position = new Vector2(mousPos.x - tipWindow.sizeDelta.x * 2, mousPos.y);
                    break;
                case Side.Right:
                    tipWindow.transform.position = new Vector2(mousPos.x + tipWindow.sizeDelta.x * 2, mousPos.y);
                    break;
                case Side.None:
                    tipWindow.transform.position = new Vector2(mousPos.x, mousPos.y);
                    break;
                default:
                    break;
            }
        }


    }

    private void HideTip()
    {
        tipText.text = default;
        tipTitle.text = default;
        tipExtra.text = default;
        tipWindow.gameObject.SetActive(false);
    }


}
