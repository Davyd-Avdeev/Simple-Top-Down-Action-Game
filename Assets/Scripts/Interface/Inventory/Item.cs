using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    public ItemScriptableObject magItem;
    public int magAmount;

    public Item(ItemScriptableObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}
