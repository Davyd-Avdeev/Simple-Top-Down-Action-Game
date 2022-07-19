using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsMenu : MonoBehaviour
{
    public Button btnDescriprtion;
    public Button btnUpgrade;
    public InventoryManager im;
    private Transform descriptionPanel;
    private Transform upgradePanel;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        btnDescriprtion.interactable = false;
        descriptionPanel = im.descriptionPanel;
        upgradePanel = im.craftPanel;
    }

    public void onClickInventMenu()
    {
        im.activeMenu = im.descriptionPanel;
        descriptionPanel.gameObject.SetActive(true);
        upgradePanel.gameObject.SetActive(false);        
        btnUpgrade.interactable = true;
        btnDescriprtion.interactable = false;

    }

    public void onClickUpgradeMenu()
    {
        im.activeMenu = im.craftPanel;
        upgradePanel.gameObject.SetActive(true);
        descriptionPanel.gameObject.SetActive(false);        
        btnDescriprtion.interactable = true;
        btnUpgrade.interactable = false;
    }
}

