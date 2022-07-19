using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputShopping : MonoBehaviour
{
    public Transform sliderCount;
    public Transform inputField;
    public DialogManager dm;
    public InventoryManager im;
    public Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        dm = GameObject.FindGameObjectWithTag("npcCanvas").GetComponent<DialogManager>();
    }

    public void OnChangeInputFieldCount()
    {
        if (transform.GetComponent<TMP_InputField>().text != "")
        {
            if (int.Parse(transform.GetComponent<TMP_InputField>().text) <= sliderCount.GetComponent<Slider>().minValue || int.Parse(transform.GetComponent<TMP_InputField>().text) > sliderCount.GetComponent<Slider>().maxValue)
            {
                transform.GetComponent<TMP_InputField>().text = "1";
            }
            if (int.Parse(transform.GetComponent<TMP_InputField>().text) == sliderCount.GetComponent<Slider>().value)
            {
                return;
            }
            sliderCount.GetComponent<Slider>().value = int.Parse(transform.GetComponent<TMP_InputField>().text);
        }
    }

    public void OnChangeSliderCount()
    {
        inputField.GetComponent<TMP_InputField>().text = transform.GetComponent<Slider>().value.ToString();
    }

    public void BtnBuyClick()
    {
        if (dm.selectedSlot != null)
        {
            if (dm.selectedSlot.item.itemType == ItemType.Magazine)
            {
                for (int i = 0; i < dm.selectedSlot.amount; i++)
                {
                    im.AddItem(dm.selectedSlot.item, dm.selectedSlot.item.maximumAmount, null, 0);
                }
            }
            else
            {
                im.AddItem(dm.selectedSlot.item, (int)sliderCount.GetComponent<Slider>().value, null, 0);
            }
            dm.selectedSlot.amount -= (int)sliderCount.GetComponent<Slider>().value;
            dm.selectedSlot.storeItem.leftCount -= (int)sliderCount.GetComponent<Slider>().value;
            if (dm.selectedSlot.storeItem.leftCount <= 0)
            {
                dm.selectedSlot.storeItem.isSold = true;
            }
            dm.selectedSlot.itemAmountText.text = dm.selectedSlot.amount.ToString();
            if (dm.selectedSlot.amount <= 0)
            {
                dm.DeleteStoreSlot(dm.selectedSlot);
            }            
            dm.selectedSlot.GetComponent<Image>().sprite = dm.notSelectedSlotSprite;
            dm.selectedSlot = null;
            sliderCount.GetComponent<Slider>().maxValue = 1;
            sliderCount.GetComponent<Slider>().value = 1;
        }        
    }

    public void BtnExit()
    {
        dm.storePanel.gameObject.SetActive(false);
        dm.ClearStore();

        dm.isStoregOpened = false;
        player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
    }
}
