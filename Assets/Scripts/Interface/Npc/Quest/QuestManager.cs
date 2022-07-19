using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public GameObject questSlotPrefab;
    public QuestScriptable questTest;
    public List<Transform> questSlots;
    public InventoryManager im;
    private InterfaceManager intefaceManager;
    public Transform questPanel;
    public Transform questSpotsPanel;
    public Transform selectedQuestSlot;
    public Transform descriptionPanel;
    public Transform player;
    public bool isOpened;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        intefaceManager = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
        isOpened = false;
        for (int i = 0; i < questSpotsPanel.childCount; i++)
        {
            if (questSpotsPanel.GetChild(i).GetComponent<btnQuestSlot>() != null)
            {
                questSlots.Add(questSpotsPanel.GetChild(i));
                questSlots[i].gameObject.SetActive(false);
            }
        }
        questPanel.gameObject.SetActive(false);
        descriptionPanel.GetChild(3).gameObject.SetActive(false);
        descriptionPanel.GetChild(4).GetComponent<TextMeshProUGUI>().text = "";
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (intefaceManager.CheckOpenedObjs())
            {
                isOpened = !isOpened;
                questPanel.gameObject.SetActive(true);

                player.GetComponent<PlayerContrl>().PlayerInUIPanel(true);
            }
            else if (isOpened)
            {
                CloseQuestPanel();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isOpened)
        {
            CloseQuestPanel();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            FindQuestItemsInInventory();
        }
    }

    public void CloseQuestPanel()
    {
        isOpened = false;
        questPanel.gameObject.SetActive(false);
        if (selectedQuestSlot != null)
        {
            selectedQuestSlot.GetComponent<Button>().interactable = true;
        }
        selectedQuestSlot = null;
        descriptionPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        descriptionPanel.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        descriptionPanel.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        descriptionPanel.GetChild(3).GetComponent<Toggle>().isOn = false;
        descriptionPanel.GetChild(3).gameObject.SetActive(false);
        descriptionPanel.GetChild(4).GetComponent<TextMeshProUGUI>().text = "";

        player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
    }

    public bool CheckFinishedQuest(QuestScriptable quest)
    {
        for (int i = 0; i < questSlots.Count; i++) //9 count quest's slots
        {
            if (quest.qName == questSlots[i].GetComponent<btnQuestSlot>().quest.qName)
            {
                if (quest.questType == QuestType.FindItem)
                {
                    int haveCount = 0;
                    foreach (InventorySlot slot in im.slots)
                    {
                        if (slot.isEmpty)
                        {
                            continue;
                        }
                        if (quest.needItem == slot.item)
                        {
                            haveCount += slot.amount;
                            if (haveCount >= quest.count)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }
        }
        return false;
    }

    public void CreateQuest(QuestScriptable quest)
    {
        for (int i = 0; i < questSlots.Count; i++) //9 count quest's slots
        {
            if (questSlots[i].GetComponent<btnQuestSlot>().isEmpty)
            {
                questSlots[i].GetComponent<btnQuestSlot>().isEmpty = false;
                questSlots[i].GetComponent<btnQuestSlot>().quest = quest;
                questSlots[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.qName;
                questSlots[i].gameObject.SetActive(true);
                break;
            }
        }
        foreach (InventorySlot slot in im.slots)
        {
            if (slot.isEmpty != true) 
            {
                QuestItemCheker(slot.item, slot.amount);
            }
        }
    }

    public void CreateQuestV2()
    {
        GameObject questSlot = Instantiate(questSlotPrefab);
        questSlot.transform.SetParent(questSpotsPanel);
        questSlot.GetComponent<RectTransform>().localScale = Vector3.one;

    }

    public void DeleteQuest(QuestScriptable quest)
    {
        for (int i = 0; i < questSlots.Count; i++) //9 count quest's slots
        {
            if (questSlots[i].GetComponent<btnQuestSlot>().isEmpty)
            {

            }
            else
            {
                if (questSlots[i].GetComponent<btnQuestSlot>().quest.qName == quest.qName)
                {
                    questSlots[i].GetComponent<btnQuestSlot>().quest = null;
                    questSlots[i].GetComponent<btnQuestSlot>().isEmpty = true;
                    questSlots[i].gameObject.SetActive(false);
                    break;
                }
            }
        }
    }

    public void QuestItemCheker(ItemScriptableObject item, int amount)
    {
        for (int i = 0; i < questSlots.Count; i++) //9 max count quest's slots
        {
            if (questSlots[i].GetComponent<btnQuestSlot>().isEmpty)
            {

            }
            else
            {
                if (questSlots[i].GetComponent<btnQuestSlot>().quest.needItem == item)
                {
                    questSlots[i].GetComponent<btnQuestSlot>().haveCountItem += amount;
                    if (questSlots[i].GetComponent<btnQuestSlot>().haveCountItem >= questSlots[i].GetComponent<btnQuestSlot>().quest.count)
                    {
                        questSlots[i].GetComponent<btnQuestSlot>().isFinished = true;
                    }

                }
            }
        }

        intefaceManager.UpdateQuestOnPanel();
    }

    public void FindQuestItemsInInventory()
    {
        for (int i = 0; i < questSlots.Count; i++) //9 count quest's slots
        {

            foreach (InventorySlot slot in im.slots)
            {
                if (slot.isEmpty)
                {
                    continue;
                }
                if (questSlots[i].GetComponent<btnQuestSlot>().quest.needItem == slot.item)
                {
                    questSlots[i].GetComponent<btnQuestSlot>().haveCountItem += slot.amount;
                    if (questSlots[i].GetComponent<btnQuestSlot>().haveCountItem >= questSlots[i].GetComponent<btnQuestSlot>().quest.count)
                    {
                        questSlots[i].GetComponent<btnQuestSlot>().isFinished = true;
                    }
                }
            }
        }
    }
}
