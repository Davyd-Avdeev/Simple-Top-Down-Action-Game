using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    public bool IsOpened;
    private Transform player;
    private InventoryManager im;
    private InterfaceManager intefaceManager;
    public List<InventorySlot> storageSlots;
    public List<InventorySlot> inventorySlots;
    public List<StorageOpen> storages;
    public Transform storagePanel;
    public Transform inventoryPanel;
    public StorageOpen openedStorage = null;

    private void Start()
    {
        IsOpened = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        intefaceManager = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
        for (int i = 0; i < storagePanel.childCount; i++)
        {
            if (storagePanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                storageSlots.Add(storagePanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                inventorySlots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        storagePanel.gameObject.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
        foreach (StorageOpen storage in storages)
        {
            // LoadStoragePanel(storage.storageList);
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsOpened)
        {
            if (IsOpened)
            {
                IsOpened = false;
                SaveInventory();
                SaveStorage(openedStorage);
                storagePanel.gameObject.SetActive(false);
                inventoryPanel.gameObject.SetActive(false);
                openedStorage = null;
                player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
                im.quickslotPanel.GetComponent<QuickslotInventory>().Cheker();
            }
        }
    }

    public void AddStorageItem(ItemScriptableObject _item, int _amount, ItemScriptableObject _magItem, int _magAmount)
    {
        foreach (InventorySlot slot in storageSlots)
        {
            if (slot.isEmpty == true)
            {
                slot.isEmpty = false;
                slot.item = _item;
                slot.SetIcon(_item.icon);
                slot.amount = _amount;
                if (_magItem != null)
                {
                    slot.magItem = _magItem;
                    slot.magAmount = _magAmount;
                }
                if (_item.maximumAmount != 1) // added this if statement for single items
                {
                    slot.itemAmountText.text = _amount.ToString();
                }
                else
                {
                    slot.itemAmountText.text = "";
                }
                break;
            }
        }

    }

    public void LoadInventoryPanel()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (!im.slots[i].isEmpty)
            {
                im.slots[i].GetComponentInChildren<DragAndDropItem>().ExchangeSlotData(inventorySlots[i], im.slots[i]);
            }
        }
    }

    public void SaveInventory()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].isEmpty == false)
            {
                im.slots[i].GetComponentInChildren<DragAndDropItem>().ExchangeSlotData(im.slots[i], inventorySlots[i]);
            }
        }
    }

    public void SaveStorage(StorageOpen storage)
    {
        storage.storageItems.Clear();

        foreach (InventorySlot slot in storageSlots)
        {
            if (slot.isEmpty == false)
            {
                StorageItem item = new StorageItem(slot.item, slot.amount, slot.magItem, slot.magAmount);
                storage.storageItems.Add(item);
            }
        }

    }

    public void ClearPanel(List<InventorySlot> slotsList)
    {
        foreach (InventorySlot slot in slotsList)
        {
            NullifySlotData(slot);
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

    public void LoadStoragePanel(StorageOpen storage)
    {
        ClearPanel(storageSlots);

        for (int i = 0; i < storage.storageItems.Count; i++)
        {
            AddStorageItem(storage.storageItems[i].item, storage.storageItems[i].amount, storage.storageItems[i].magItem, storage.storageItems[i].magAmount);
        }
    }
}
