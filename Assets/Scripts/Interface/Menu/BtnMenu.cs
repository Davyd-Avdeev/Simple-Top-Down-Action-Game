using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnMenu : MonoBehaviour
{
    public DialogManager dm;
    public StorageManager sm;

    private void Start()
    {
        dm = GameObject.FindGameObjectWithTag("npcCanvas").GetComponent<DialogManager>();
        sm = GameObject.FindGameObjectWithTag("storageCanvas").GetComponent<StorageManager>();
    }

    public void btnRefreshStore()
    {
        foreach (Npc npc in dm.npcs)
        {
            dm.RefreshStoreItems(npc.dialog);
        }
    }

    public void btnRefreshStorage()
    {
        foreach (StorageOpen storage in sm.storages)
        {
            if (storage.isUpdating == false)
            {
                continue;
            }
            storage.RefreshStorage();
        }
    }

    public void btnExit()
    {
        Application.Quit();
    }
}
