using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCrafteble : ScriptableObject
{
    public ItemType itemType;
    [Header("Items for craft")]
    public ItemScriptableObject[] item;
    public ItemScriptableObject craftedItem;
    public int craftCount;
}
