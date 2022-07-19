using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropStorageItems : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public InventorySlot oldSlot;
    private Transform player;
    public InventoryManager im;
    public QuickslotInventory qi;
    public StorageManager sm;
    private Vector3 offset;

    private void Awake()
    {
        qi = GameObject.FindGameObjectWithTag("QuickslotInventory").GetComponent<QuickslotInventory>();
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        sm = GameObject.FindGameObjectWithTag("storageCanvas").GetComponent<StorageManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        oldSlot = transform.GetComponentInParent<InventorySlot>(); // Находим скрипт InventorySlot в слоте в иерархии
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Если слот пустой, то мы не выполняем то что ниже return;
        if (oldSlot.isEmpty)
            return;

        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        offset = gameObject.transform.position -
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.75f); //Делаем картинку прозрачнее        
        GetComponentInChildren<Image>().raycastTarget = false; // Делаем так чтобы нажатия мышкой не игнорировали эту картинку        
        transform.SetParent(transform.parent.parent.parent); // Делаем наш DraggableObject ребенком InventoryPanel чтобы DraggableObject был над другими слотами инвенторя
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f); // Делаем картинку опять не прозрачной        
        GetComponentInChildren<Image>().raycastTarget = true; // И чтобы мышка опять могла ее засечь

        transform.SetParent(oldSlot.transform); //Поставить DraggableObject обратно в свой старый слот
        transform.position = oldSlot.transform.position;
        //Если мышка отпущена над объектом по имени UIPanel, то...
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "Inventory_Panel" || eventData.pointerCurrentRaycast.gameObject.name == "Storage_Panel")
            {
                return;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
            {                
                if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>().item != null)
                {
                    if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>().item.itemType == ItemType.Consumables)
                    {
                        if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>().item.itemName == oldSlot.item.itemName)
                        {
                            if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>().amount != oldSlot.item.maximumAmount)
                            {
                                IdontKnow(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>());
                                qi.Cheker();
                                return;
                            }
                        }
                    }
                }

                ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>(), oldSlot);
            }
        }
    }
    public void NullifySlotData(InventorySlot slot)
    {
        // убираем значения InventorySlot
        slot.item = null;
        slot.amount = 0;
        slot.isEmpty = true;
        slot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        slot.iconGO.GetComponent<Image>().sprite = null;
        slot.itemAmountText.text = "";
    }

    void IdontKnow(InventorySlot newSlot)
    {
        int outAmount = oldSlot.amount;
        if (newSlot.amount + outAmount <= newSlot.item.maximumAmount)
        {
            newSlot.amount += outAmount;
            newSlot.itemAmountText.text = newSlot.amount.ToString();
            NullifySlotData(oldSlot);
        }
        else
        {
            outAmount -= newSlot.item.maximumAmount - newSlot.amount;
            oldSlot.amount = outAmount;
            oldSlot.itemAmountText.text = oldSlot.amount.ToString();
            newSlot.amount += newSlot.item.maximumAmount - newSlot.amount;
            newSlot.itemAmountText.text = newSlot.amount.ToString();
        }
    }

    void ExchangeSlotData(InventorySlot newSlot, InventorySlot oldSlot)
    {
        // Временно храним данные newSlot в отдельных переменных
        ItemScriptableObject item = newSlot.item;
        int amount = newSlot.amount;
        bool isEmpty = newSlot.isEmpty;
        ItemScriptableObject magItem = newSlot.magItem;
        int magAmount = newSlot.magAmount;
        GameObject iconGO = newSlot.iconGO;
        TMP_Text itemAmountText = newSlot.itemAmountText;

        // Заменяем значения newSlot на значения oldSlot
        newSlot.item = oldSlot.item;
        newSlot.amount = oldSlot.amount;
        if (oldSlot.isEmpty == false)
        {
            newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
            if (oldSlot.item.maximumAmount != 1) // added this if statement for single items
            {
                newSlot.itemAmountText.text = oldSlot.amount.ToString();
            }
            else
            {
                newSlot.itemAmountText.text = "";
            }
        }
        else
        {
            newSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            newSlot.iconGO.GetComponent<Image>().sprite = null;
            newSlot.itemAmountText.text = "";
        }
        newSlot.magItem = oldSlot.magItem;
        newSlot.magAmount = oldSlot.magAmount;
        newSlot.isEmpty = oldSlot.isEmpty;

        // Заменяем значения oldSlot на значения newSlot сохраненные в переменных
        oldSlot.item = item;
        oldSlot.amount = amount;
        if (isEmpty == false)
        {
            oldSlot.SetIcon(item.icon);
            if (item.maximumAmount != 1) // added this if statement for single items
            {
                oldSlot.itemAmountText.text = amount.ToString();
            }
            else
            {
                oldSlot.itemAmountText.text = "";
            }
        }
        else
        {
            oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            oldSlot.iconGO.GetComponent<Image>().sprite = null;
            oldSlot.itemAmountText.text = "";
        }
        oldSlot.magItem = magItem;
        oldSlot.magAmount = magAmount;
        oldSlot.isEmpty = isEmpty;

        if (oldSlot.transform.parent.name == "Inventory_Panel" && newSlot.transform.parent.name == "Storage_Panel")
        {
            if (HaveWeapon(newSlot.item) <= 0)
            { 
                im.DeleteItemInPlayer(newSlot.item);
            }
            
            if (!oldSlot.isEmpty)
            {
                im.CheckHaveItem(oldSlot.item);
            }            
        } 
        else if (oldSlot.transform.parent.name == "Invetory_Panel" && newSlot.transform.parent.name == "Inventory_Panel")
        {
        }
        else if (oldSlot.transform.parent.name == "Storage_Panel" && newSlot.transform.parent.name == "Storage_Panel")
        {
        } 
        else if (oldSlot.transform.parent.name == "Storage_Panel" && newSlot.transform.parent.name == "Inventory_Panel")
        {            
            if (!oldSlot.isEmpty)
            {
                if (HaveWeapon(oldSlot.item) <= 0)
                {
                    im.DeleteItemInPlayer(oldSlot.item);
                }
            }
            im.CheckHaveItem(newSlot.item);
        }
        qi.Cheker();
    }

    public int HaveWeapon(ItemScriptableObject _item)
    {
        int count = 0;
        for (int i = 0; i < sm.inventorySlots.Count; i++)
        {
            Debug.Log($"i = {i}, {im.slots[i].item}");
            if (sm.inventorySlots[i].item != null)
            {
                if (sm.inventorySlots[i].item.model.name == _item.model.name)
                {
                    count++;
                }

            }

        }
        return count;
    }


}
