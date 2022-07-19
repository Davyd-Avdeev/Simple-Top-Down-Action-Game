using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageOpen : MonoBehaviour
{
    public GameObject text;
    private Transform player;
    private StorageManager sm;
    private InventoryManager im;
    private InterfaceManager intefaceManager;
    public StorageListScriptable storageList;
    public List<StorageItem> storageItems;
    public float takeDistance;
    private bool isActive = false;
    public bool isUpdating = false;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        sm = GameObject.FindGameObjectWithTag("storageCanvas").GetComponent<StorageManager>();
        intefaceManager = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        text.SetActive(false);
        sm.storages.Add(this);
    }

    private void Update()
    {
        if (intefaceManager.CheckOpenedObjs())
        {
            if (isActive && Vector2.Distance(player.position, transform.position) <= takeDistance)
            {
                text.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    sm.IsOpened = true;
                    sm.LoadInventoryPanel();
                    sm.LoadStoragePanel(this);
                    sm.openedStorage = this;
                    sm.storagePanel.gameObject.SetActive(true);
                    sm.inventoryPanel.gameObject.SetActive(true);
                    player.GetComponent<PlayerContrl>().PlayerInUIPanel(true);
                }
            }
            else
            {
                text.SetActive(false);
            }
        }
        else
        {
            text.SetActive(false);
        }
    }

    public void RefreshStorage()
    {
        storageItems.Clear();
        for (int i = 0; i < storageList.storageItems.Count; i++)
        {
            storageItems.Add(storageList.storageItems[i]);
        }
    }

    private void OnMouseEnter()
    {
        isActive = true;
    }

    private void OnMouseExit()
    {
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
