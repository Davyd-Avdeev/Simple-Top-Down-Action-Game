using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    public ItemCrafteble recipe;
    public InventoryManager im;
    public CraftingManager cm;

    private void Awake()
    {
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        cm = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CraftingManager>();
    }

    

    public void SetIcon()
    {
        this.transform.GetChild(0).GetComponent<Image>().sprite = recipe.craftedItem.icon;
    }

    public void BtnCraft()
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
                    if (slot.amount <= 1)
                    {
                        slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData(slot);
                    }
                    if (slot.amount >= 2)
                    {
                        slot.amount--;
                        slot.itemAmountText.text = slot.amount.ToString();
                    }
                    break;
                }
            }
        }
        im.AddItem(recipe.craftedItem, recipe.craftCount, null, 0);

        cm.CanCraftCheck();

        switch (recipe.itemType)
        {
            case ItemType.Weapon:
                im.recipesPanel.GetChild(0).GetComponent<BtnRecipes>().RecipesSlot(cm.r_Weapon);
                break;
            case ItemType.Magazine:
                im.recipesPanel.GetChild(0).GetComponent<BtnRecipes>().RecipesSlot(cm.r_Magizine);
                break;
            case ItemType.Consumables:
                im.recipesPanel.GetChild(0).GetComponent<BtnRecipes>().RecipesSlot(cm.r_Consumables);
                break;
        }

    }
}
