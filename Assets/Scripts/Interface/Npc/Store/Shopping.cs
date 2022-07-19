using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shopping : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public StoreSlot storeSlot;
    public DialogManager dm;
    private Transform player;
    private Vector3 offset;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dm = GameObject.FindGameObjectWithTag("npcCanvas").GetComponent<DialogManager>();
        storeSlot = transform.GetComponentInParent<StoreSlot>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (storeSlot.isEmpty)
            return;        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (storeSlot.isEmpty)
            return;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("1");
        if (storeSlot.isEmpty)
            return;
        Debug.Log("2");
        if (dm.selectedSlot != null)
        {
            dm.selectedSlot.GetComponent<Image>().sprite = dm.notSelectedSlotSprite;
        }
        dm.selectedSlot = storeSlot;
        storeSlot.GetComponent<Image>().sprite = dm.selectedSlotSprite;
        dm.sliderCount.GetComponent<Slider>().maxValue = storeSlot.amount;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (storeSlot.isEmpty)
            return;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (storeSlot.isEmpty)
            return;
    }
}
