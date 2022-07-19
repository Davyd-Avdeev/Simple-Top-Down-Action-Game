using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnRecipes : MonoBehaviour
{
    private InventoryManager im;
    private CraftingManager cm;
    public string activeCategory;
    public GameObject recipeSlot;

    private void Start()
    {

        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        cm = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CraftingManager>();
    }
    public void BtnWeapon()
    {
        cm.CanCraftCheck();
        RecipesSlot(cm.r_Weapon);
        im.recipesPanel.gameObject.SetActive(true);
        im.craftPanel.gameObject.SetActive(false);
        im.recipesPanel.GetChild(0).GetComponent<BtnRecipes>().activeCategory = "Weapon";
        im.recipesPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = "Оружие";
    }

    public void BtnMagazine()
    {
        cm.CanCraftCheck();
        RecipesSlot(cm.r_Magizine);
        im.recipesPanel.gameObject.SetActive(true);
        im.craftPanel.gameObject.SetActive(false);
        im.recipesPanel.GetChild(0).GetComponent<BtnRecipes>().activeCategory = "Magazine";
        im.recipesPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = "Патроны";
    }

    public void BtnConsumeable()
    {
        cm.CanCraftCheck();
        RecipesSlot(cm.r_Consumables);
        im.recipesPanel.gameObject.SetActive(true);
        im.craftPanel.gameObject.SetActive(false);
        im.recipesPanel.GetChild(0).GetComponent<BtnRecipes>().activeCategory = "Consumeable";
        im.recipesPanel.GetChild(0).GetChild(0).GetComponent<Text>().text = "Прочее";
    }

    public void BtnBack()
    {
        im.craftPanel.gameObject.SetActive(true);
        im.recipesPanel.gameObject.SetActive(false);        
    }

    public void RecipesSlot(List<ItemCrafteble> recipes)
    {
        for (int i = 2; i < im.recipesPanel.childCount; i++)
        {
            Destroy(im.recipesPanel.GetChild(i).gameObject);
        }
        for (int i = 0; i < recipes.Count; i++)
        {
            GameObject rec = Instantiate(recipeSlot, im.recipesPanel.GetChild(1).GetChild(i).transform);
            rec.transform.GetComponent<RecipeSlot>().recipe = recipes[i];
            rec.transform.GetComponent<RecipeSlot>().SetIcon();
            rec.transform.SetParent(im.recipesPanel);
        }
    }

}
