using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public ItemCrafteble[] recipes;
    public List<ItemCrafteble> r_Magizine;
    public List<ItemCrafteble> r_Weapon;
    public List<ItemCrafteble> r_Consumables;
    public InventoryManager im;
    public int canCraftCount;

    private void Start()
    {
        im = this.GetComponent<InventoryManager>();
    }
    public void CanCraftCheck()
    {
        r_Magizine.Clear();
        r_Weapon.Clear();
        r_Consumables.Clear();

        int readyCount = 0;
        foreach (ItemCrafteble recipe in recipes)
        {
            for (int i = 0; i < recipe.item.Length; i++)
            {
                foreach (InventorySlot slot in im.slots)
                {
                    if (slot.isEmpty)
                    {
                        continue;
                    }
                    if (recipe.item[i] == slot.item)
                    {
                        readyCount++;
                        break;
                    }
                }
            }

            if (readyCount == recipe.item.Length)
            {
                Debug.Log("ready to craft: " + recipe.name + " Type: " + recipe.itemType);
                switch (recipe.itemType)
                {
                    case ItemType.Magazine:
                        r_Magizine.Add(recipe);
                        break;
                    case ItemType.Consumables:
                        r_Consumables.Add(recipe);
                        break;
                    case ItemType.Weapon:
                        r_Weapon.Add(recipe);
                        break;
                }
            }
            readyCount = 0;
        }
    }
}
