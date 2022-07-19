using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/New Craft")]
public class ItemCraftebleCreator : ItemCrafteble
{
    void Start()
    {
        itemType = ItemType.Default;
    }
}
