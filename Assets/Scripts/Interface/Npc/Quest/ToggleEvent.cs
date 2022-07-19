using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleEvent : MonoBehaviour, IPointerClickHandler
{
    public QuestScriptable quest;
    public btnQuestSlot questSlot;
    public Toggle toggle;
    public InterfaceManager infM;
    private void Start()
    {
        toggle = GetComponent<Toggle>();
        infM = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
    }
    public void OnChanged()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (toggle.isOn)
        {
            questSlot.isActive = true;
            infM.AddQuestOnPanel(questSlot);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (!infM.activeQuests[i].isEmpty)
                {
                    if (infM.activeQuests[i].quest.quest.qName == questSlot.quest.qName)
                    {
                        infM.RemoveQuestOnPanel(i);
                        questSlot.isActive = false;
                    }
                }
            }
        }
    }


}
