using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class btnQuestSlot : MonoBehaviour
{
    public QuestScriptable quest;
    public QuestManager qm;
    private TextMeshProUGUI nametext;
    private TextMeshProUGUI typeText;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI processText;
    private Toggle toggle;
    public bool isEmpty;
    public bool isActive;
    public bool isFinished;
    public int haveCountItem;


    void Start()
    {
        qm = GameObject.FindGameObjectWithTag("questCanvas").GetComponent<QuestManager>();
        nametext = qm.descriptionPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
        typeText = qm.descriptionPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        descriptionText = qm.descriptionPanel.GetChild(2).GetComponent<TextMeshProUGUI>();
        processText = qm.descriptionPanel.GetChild(4).GetComponent<TextMeshProUGUI>();
        toggle = qm.descriptionPanel.GetChild(3).GetComponent<Toggle>();
    }

    public void BtnQuestSlot()
    {
        if (qm.selectedQuestSlot != null)
        {
            qm.selectedQuestSlot.GetComponent<Button>().interactable = true;
        }        
        qm.selectedQuestSlot = this.transform;
        GetComponent<Button>().interactable = false;
        nametext.text = quest.qName; // Quest Name
        typeText.text = $"Type: {quest.questType}"; // Quest Type
        descriptionText.text = quest.qDescription; // Quest Descryption
        processText.text = $"{haveCountItem}/{quest.count}"; // Quest count items 0\1o, example
        toggle.isOn = isActive;
        qm.descriptionPanel.GetChild(3).GetComponent<ToggleEvent>().quest = quest;
        qm.descriptionPanel.GetChild(3).GetComponent<ToggleEvent>().questSlot = this;
        qm.descriptionPanel.GetChild(3).gameObject.SetActive(true);        
    }
}
