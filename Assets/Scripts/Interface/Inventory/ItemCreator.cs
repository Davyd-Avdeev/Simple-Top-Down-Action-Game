using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item",menuName ="Inventory/New Item")]
public class ItemCreator : ItemScriptableObject
{
    private void Start()
    {
        itemType = ItemType.Default;
    }
}
