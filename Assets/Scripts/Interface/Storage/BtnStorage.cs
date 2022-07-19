using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnStorage : MonoBehaviour
{
    public StorageManager sm;
    public InventoryManager im;

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("storageCanvas").GetComponent<StorageManager>();
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
    }
    public void BtnTakeAll()
    {
        for (int i = 0; i < sm.storageSlots.Count; i++)
        {
            foreach (InventorySlot slot in sm.inventorySlots)
            {
                if (!sm.storageSlots[i].isEmpty)
                {
                    if (slot.isEmpty)
                    {
                        im.slots[1].GetComponentInChildren<DragAndDropItem>().ExchangeSlotData(slot, sm.storageSlots[i]);
                    }
                }                
            }
        }
    }

    public void BtnPutAll()
    {
        for (int i = 0; i < sm.storageSlots.Count; i++)
        {
            if (sm.storageSlots[i].isEmpty)
            {
                for (int k = 0; k < sm.inventorySlots.Count; k++)
                {
                    if (!sm.inventorySlots[k].isEmpty)
                    {
                        im.slots[1].GetComponentInChildren<DragAndDropItem>().ExchangeSlotData(sm.storageSlots[i], sm.inventorySlots[k]);
                    }
                }
            }
        }
    }
}
