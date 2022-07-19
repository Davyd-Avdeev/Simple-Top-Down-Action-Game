using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject UIBG;
    private InterfaceManager intefaceManager;
    public Transform inventoryPanel;
    public Transform quickslotPanel;
    public Transform craftPanel;
    public Transform recipesPanel;
    public Transform btnPanel;
    public Transform descriptionPanel;
    public Transform activeMenu;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isOpened;
    public float reachDistance = 30f;
    private Camera mainCamera;
    private Transform player;

    private void Awake()
    {
        UIBG.SetActive(true);
    }

    void Start()
    {
        intefaceManager = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
        isOpened = false;
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        for (int i = 0; i < quickslotPanel.childCount; i++)
        {
            if (quickslotPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(quickslotPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        activeMenu = descriptionPanel;
        descriptionPanel.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
        UIBG.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
        descriptionPanel.gameObject.SetActive(false);
        craftPanel.gameObject.SetActive(false);
        btnPanel.gameObject.SetActive(false);
    }


    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            //Debug.Log("LOOl");
            if (intefaceManager.CheckOpenedObjs())
            {
                isOpened = !isOpened;
                UIBG.SetActive(true);
                inventoryPanel.gameObject.SetActive(true);
                descriptionPanel.gameObject.SetActive(true);
                btnPanel.gameObject.SetActive(true);

                player.GetComponent<PlayerContrl>().PlayerInUIPanel(true);
                quickslotPanel.gameObject.SetActive(true);
            }
            else if (isOpened)
            {
                CloseInventory();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isOpened)
        {
            CloseInventory();
        }
    }

    public void CloseInventory()
    {
        isOpened = false;
        UIBG.SetActive(false);
        activeMenu.gameObject.SetActive(false);
        activeMenu = descriptionPanel;
        btnPanel.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
        btnPanel.GetChild(1).gameObject.GetComponent<Button>().interactable = true;
        btnPanel.gameObject.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
        recipesPanel.gameObject.SetActive(false);
        descriptionPanel.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";

        player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
    }

    public void AddItem(ItemScriptableObject _item, int _amount, ItemScriptableObject _magItem, int _magAmount)
    {
        bool isAdded = false;
        foreach (InventorySlot slot in slots)
        {
            if (_item.maximumAmount == 1)
            {
                break;
            }
            if (_item.itemType == ItemType.Magazine)
            {
                break;
            }
            // В слоте уже имеется этот предмет
            if (slot.item == _item)
            {                
                if (slot.amount + _amount <= _item.maximumAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    isAdded = true;
                    return;
                }
                else
                {
                    _amount -= _item.maximumAmount - slot.amount;
                    slot.amount += _item.maximumAmount - slot.amount;
                    if (slot.item.maximumAmount != 1)
                    {
                        slot.itemAmountText.text = slot.amount.ToString();
                    }
                    else
                    {
                        slot.itemAmountText.text = "";
                    }

                }
                if (!isAdded)
                {
                    continue;
                }
                break;
            }
        }
        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                if (slot.item.maximumAmount < _amount)
                {
                    slot.amount = slot.item.maximumAmount;

                }
                else
                {
                    slot.amount = _amount;
                }
                _amount -= slot.item.maximumAmount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                if (_magItem != null)
                {
                    slot.magItem = _magItem;
                    slot.magAmount = _magAmount;
                }                
                if (_item.maximumAmount != 1) // added this if statement for single items
                {
                    slot.itemAmountText.text = slot.amount.ToString();
                }
                else
                {
                    slot.itemAmountText.text = "";
                }
                if (_amount <= 0)
                {
                    break;
                }
            }
        }

    }

    public void CheckHaveItem(ItemScriptableObject item)
    {
        if (HaveWeapon(item))
        {

        }
        else
        {
            GameObject go;
            go = Instantiate(item.model);
            go.transform.parent = player.GetChild(0);
            go.transform.rotation = player.GetChild(0).rotation;
            Vector3 LocalScale = go.transform.localScale;
            if (Input.mousePosition.x > FindObjectOfType<Camera>().WorldToScreenPoint(player.position).x)
            {
                LocalScale.y *= 1f;
            }
            else
            {
                LocalScale.y *= -1f;
            }
            go.transform.localScale = LocalScale;
            go.transform.position = player.position;
            go.SetActive(false);
        }
    }

    private bool HaveWeapon(ItemScriptableObject item)
    {
        for (int i = 0; i < player.GetChild(0).childCount; i++)
        {
            if (player.GetChild(0).GetChild(i).GetComponent<Item>() != null)
            {
                if (player.GetChild(0).GetChild(i).GetComponent<Item>().item.model.name == item.model.name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void DeleteItemInPlayer(ItemScriptableObject item)
    {
        for (int i = 0; i < player.GetChild(0).childCount; i++)
        {
            if (player.GetChild(0).GetChild(i).GetComponent<Item>() != null)
            {
                if (player.GetChild(0).GetChild(i).GetComponent<Item>().item.model.name == item.model.name)
                {
                    Destroy(player.GetChild(0).GetChild(i).gameObject);
                }
            }
        }
    }

    public void UpdateSlotMagInfo(InventorySlot slot)
    {

    }
}
