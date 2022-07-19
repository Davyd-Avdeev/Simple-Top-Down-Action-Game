using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class btnDialog : MonoBehaviour
{
    public Npc npc;
    public InventoryManager im;
    public DialogManager dm;
    public QuestManager qm;
    public DialogScriptable dialog;
    public Transform player;
    void Start()
    {
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        dm = GameObject.FindGameObjectWithTag("npcCanvas").GetComponent<DialogManager>();
        qm = GameObject.FindGameObjectWithTag("questCanvas").GetComponent<QuestManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void BtnYes()
    {
        if (npc.haveQuest)
        {
            qm.CreateQuest(dialog.questScripteble);
            npc.haveQuest = false;
            npc.getedQuest = true;
            player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
        }
        else if (npc.getedQuest) //true = reward false = defaultPhrase
        {
            if (qm.CheckFinishedQuest(dialog.questScripteble)) // bool Cheking
            {
                int needCount = dialog.questScripteble.count;
                foreach (InventorySlot slot in im.slots)
                {
                    if (slot.isEmpty)
                    {
                        continue;
                    }
                    if (dialog.questScripteble.needItem == slot.item)
                    {
                        if (needCount >= slot.amount)
                        {
                            needCount -= slot.amount;
                            slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData(slot);
                        } else if (needCount < slot.amount)
                        {
                            slot.amount -= needCount;
                            slot.itemAmountText.text = slot.amount.ToString();
                            needCount = 0;
                        }
                        if (needCount == 0)
                        {
                            break;
                        }
                    }
                }
                im.AddItem(dialog.questScripteble.rewardItem, dialog.questScripteble.countReward, null, 0);
                qm.DeleteQuest(dialog.questScripteble);
                npc.getedQuest = false;
                npc.completedQuest = true;
            }
            player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
        }
        else if (npc.completedQuest)
        {
            dm.storePanel.gameObject.SetActive(true);
            dm.AddStoreSlots(dialog);
            dm.isStoregOpened = true;
            player.GetComponent<PlayerContrl>().PlayerInUIPanel(true);
        }

        dm.ClearDialog();
    }

    public void BtnNo() 
    {
        if (npc.haveQuest)
        {
            player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
        }
        else if (npc.getedQuest) //true = reward false = defaultPhrase
        {
            if (qm.CheckFinishedQuest(dialog.questScripteble)) // bool Cheking
            {
                npc.completedQuest = true;
            }
            else
            {

            }
            player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
        }
        else if (npc.completedQuest)
        {
            dm.RefreshStoreItems(dialog);
        }

        dm.ClearDialog();
    }

    

    public void BtnSkip()
    {
        dm.isSkiped = true;
    }

}
