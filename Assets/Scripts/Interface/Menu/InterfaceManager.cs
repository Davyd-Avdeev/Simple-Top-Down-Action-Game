using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InterfaceManager : MonoBehaviour
{
    public bool isOpened;
    [SerializeField]
    private Transform menuPanel;
    private InventoryManager im;
    private DialogManager dm;
    private QuestManager qm;
    private StorageManager sm;
    private Transform player;
    [SerializeField]
    private Transform questInfo;
    public Transform HPBar;
    public Transform ammoInfo;
    public Transform reloadInfo;
    public List<InfoQuestSpot> activeQuests;
    public bool smthOpened;

    private void Start()
    {
        isOpened = false;
        smthOpened = false;
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        dm = GameObject.FindGameObjectWithTag("npcCanvas").GetComponent<DialogManager>();
        qm = GameObject.FindGameObjectWithTag("questCanvas").GetComponent<QuestManager>();
        sm = GameObject.FindGameObjectWithTag("storageCanvas").GetComponent<StorageManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        menuPanel.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            activeQuests.Add(questInfo.GetChild(i).GetComponent<InfoQuestSpot>());
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CheckOpenedObjs())
            {
                isOpened = true;
                menuPanel.gameObject.SetActive(true);

                player.GetComponent<PlayerContrl>().PlayerInUIPanel(true);
            }
            else if (isOpened)
            {
                isOpened = false;
                menuPanel.gameObject.SetActive(false);

                player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
            }
        }
    }

    public bool CheckOpenedObjs()
    {
        smthOpened = !isOpened && !im.isOpened && !qm.isOpened && !dm.isDialogOpened && !dm.isStoregOpened && !sm.IsOpened;
        return smthOpened;
    }

    public void OutputQuests()
    {
        for (int i = 0; i < 3; i++)
        {
            //questInfo.GetChild(i).GetComponent<>
            //activeQuests[i].qName;
        }
    }

    public void AddQuestOnPanel(btnQuestSlot quest)
    {
        activeQuests[2].qText.text = activeQuests[1].qText.text;
        activeQuests[2].isEmpty = activeQuests[1].isEmpty;
        if (!activeQuests[1].isEmpty)
        {
            activeQuests[2].quest = activeQuests[1].quest;
        }
        activeQuests[1].qText.text = activeQuests[0].qText.text;
        activeQuests[1].isEmpty = activeQuests[0].isEmpty;
        if (!activeQuests[0].isEmpty)
        {
            activeQuests[1].quest = activeQuests[0].quest;
        }
        activeQuests[0].qText.text = $"{quest.quest.qName} {quest.haveCountItem}/{quest.quest.count}";
        activeQuests[0].quest = quest;
        activeQuests[0].isEmpty = false;
    }

    public void RemoveQuestOnPanel(int index)
    {
        switch (index)
        {
            case 0:
                if (!activeQuests[1].isEmpty)
                {
                    activeQuests[0].isEmpty = false;
                    activeQuests[0].quest = activeQuests[1].quest;
                    activeQuests[0].qText.text = activeQuests[1].qText.text;
                    if (!activeQuests[2].isEmpty)
                    {
                        activeQuests[1].isEmpty = false;
                        activeQuests[1].quest = activeQuests[2].quest;
                        activeQuests[1].qText.text = activeQuests[2].qText.text;
                    }
                    else
                    {
                        ClearActiveQuestSlot(1);
                    }
                }
                else
                {
                    ClearActiveQuestSlot(0);
                }
                break;

            case 1:                
                if (!activeQuests[2].isEmpty)
                {
                    activeQuests[1].isEmpty = false;
                    activeQuests[1].quest = activeQuests[2].quest;
                    activeQuests[1].qText.text = activeQuests[2].qText.text;
                    ClearActiveQuestSlot(2);
                }
                else
                {
                    ClearActiveQuestSlot(1);
                }
                break;

            case 2:
                ClearActiveQuestSlot(2);
                break;
        }
    }

    public void ClearActiveQuestSlot(int index)
    {
        activeQuests[index].isEmpty = true;
        activeQuests[index].quest = null;
        activeQuests[index].qText.text = "";
    }

    public void UpdateQuestOnPanel()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!activeQuests[i].isEmpty)
            {
                activeQuests[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{activeQuests[i].quest.quest.qName} {activeQuests[i].quest.haveCountItem}/{activeQuests[i].quest.quest.count}";
            }
        }
    }
}
