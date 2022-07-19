using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageListScriptable : ScriptableObject
{
    public List<StorageItem> storageItems;    
}

[System.Serializable]
public class StorageItem
{
    public StorageItem(ItemScriptableObject _item, int _amount, ItemScriptableObject _magItem, int _magAmount )
    {
        item = _item;
        amount = _amount;
        magItem = _magItem;
        magAmount = _magAmount;
    }
    public ItemScriptableObject item;
    public int amount;
    public ItemScriptableObject magItem;
    public int magAmount;
}
