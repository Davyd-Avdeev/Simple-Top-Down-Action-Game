using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Npc : MonoBehaviour
{
    public DialogScriptable dialog;
    public GameObject text;
    public bool haveQuest;
    public bool getedQuest;
    public bool completedQuest;
    private Transform player;
    private DialogManager dm;
    private QuestManager qm;
    private InterfaceManager intefaceManager;
    public float takeDistance;
    private bool isActive = false;

    private void Start()
    {
        dm = GameObject.FindGameObjectWithTag("npcCanvas").GetComponent<DialogManager>();
        dm.npcs.Add(this);
        qm = GameObject.FindGameObjectWithTag("questCanvas").GetComponent<QuestManager>();
        intefaceManager = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        text.SetActive(false);
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
                    if (haveQuest)
                    {                      
                        dm.StartQuestDialog(dialog, "AddQuest");
                    }
                    else if (getedQuest)
                    {
                        if (qm.CheckFinishedQuest(dialog.questScripteble)) // bool Cheking
                        {
                            dm.StartQuestDialog(dialog, "RewardQuest");
                        }
                        else
                        {
                            dm.StartQuestDialog(dialog, "NoCompletedQuest");
                        }
                    } 
                    else if (completedQuest)
                    {
                        dm.StartQuestDialog(dialog, "DefaultPhrase");
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        dm.dialogPanel.GetChild(0).GetChild(i).GetComponent<btnDialog>().npc = this;
                    }
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
