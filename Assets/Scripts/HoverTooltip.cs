using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public string Tip;
    public string Title;
    public string Extra;
    public Vector2 showLocation;
    public Side Side = Side.None;
    public HoverManager hoverManager;

    private float timeToWait = .25f;



    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Remove();
    }

    public void OnSelect(BaseEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Remove();
    }

    public void Remove()
    {
        StopAllCoroutines();
        hoverManager.OnMouseLoseFocus();
    }

    private void ShowMessage()
    {
        hoverManager.OnMouseHover(Title, Tip, Extra, showLocation, Side);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);

        ShowMessage();
    }
}
