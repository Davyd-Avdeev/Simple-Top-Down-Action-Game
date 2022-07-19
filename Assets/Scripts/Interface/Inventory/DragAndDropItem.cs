using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
/// IPointerDownHandler - Следит за нажатиями мышки по объекту на котором висит этот скрипт
/// IPointerUpHandler - Следит за отпусканием мышки по объекту на котором висит этот скрипт
/// IDragHandler - Следит за тем не водим ли мы нажатую мышку по объекту
public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public InventorySlot oldSlot;
    private Transform player;
    private GameObject textDescryption;
    public InventoryManager im;
    public QuickslotInventory qi;
    private Vector3 offset;

    private void Awake()
    {
        textDescryption = GameObject.FindGameObjectWithTag("Description");
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        qi = GameObject.FindGameObjectWithTag("QuickslotInventory").GetComponent<QuickslotInventory>();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        textDescryption.GetComponent<TextMeshProUGUI>().text = oldSlot.item.itemDescription;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        textDescryption.GetComponent<TextMeshProUGUI>().text = "";
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
            if (eventData.pointerCurrentRaycast.gameObject.name == "UIBG")
            {
                // Выброс объектов из инвентаря - Спавним префаб обекта перед персонажем
                GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + player.transform.up * 2f, Quaternion.identity);
                // Устанавливаем количество объектов такое какое было в слоте
                itemObject.GetComponent<Item>().amount = oldSlot.amount;
                if (oldSlot.item.itemType == ItemType.Weapon)
                {
                    if (oldSlot.magItem != null)
                    {                        
                        itemObject.GetComponent<Item>().magItem = oldSlot.magItem;
                        itemObject.GetComponent<Item>().magAmount = oldSlot.magAmount;
                    }
                }
                im.DeleteItemInPlayer(oldSlot.item);
                NullifySlotData(oldSlot);
                qi.Cheker();
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "Inventory_Panel" || eventData.pointerCurrentRaycast.gameObject.name == "QuickInventory_Panel" || eventData.pointerCurrentRaycast.gameObject.name == "Description_Panel" || eventData.pointerCurrentRaycast.gameObject.name == "Upgrade_Panel")
            {
                return;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
            {
                //Перемещаем данные из одного слота в другой
                if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.parent.name == "Upgrade_Panel")
                {

                }
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
                qi.Cheker();
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

    public void ExchangeSlotData(InventorySlot newSlot, InventorySlot oldSlot)
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
        im.CheckHaveItem(newSlot.item);

    }

    public int HaveWeapon(GameObject item)
    {
        int count = 0;
        for (int i = 0; i < im.slots.Count; i++)
        {
            if (im.slots[i].item != null)
            {
                if (im.slots[i].item.model.name == item.GetComponent<Item>().item.model.name)
                {
                    count++;
                }

            }

        }
        return count;
    }
}
