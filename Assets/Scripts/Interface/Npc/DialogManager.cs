using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private Transform player;
    public List<Transform> dialogSpots;
    public List<StoreSlot> storeSlots;
    public Transform dialogPanel;
    public Transform storePanel;
    public Transform sliderCount;
    public Transform inputField;
    public Transform activePanel;
    public ItemScriptableObject testItem;
    public StoreSlot selectedSlot;
    public Sprite selectedSlotSprite;
    public Sprite notSelectedSlotSprite;
    public bool isWrite = false;
    public bool isSkiped = false;
    public bool isMiniSkiped = false;
    public bool isDialogOpened;
    public bool isStoregOpened;
    public List<Npc> npcs;
    private bool isStoped = false;


    IEnumerator ShowText(string text, TextMeshProUGUI textUI)
    {
        isWrite = true;
        int i = 0;
        while (i <= text.Length)
        {
            if (isSkiped)
            {
                break;
            }

            if (isMiniSkiped)
            {
                isMiniSkiped = false;
                textUI.text = text;
                break;
            }
            textUI.text = text.Substring(0, i);
            i++;

            yield return new WaitForSeconds(0.07f);
        }
        isWrite = false;
    }

    IEnumerator DefaultPhrase(DialogScriptable dialog)
    {
        dialogSpots[0].gameObject.SetActive(true);
        dialogSpots[0].GetChild(0).GetComponent<SpriteRenderer>().sprite = dialog.npcIcon;
        StartCoroutine(ShowText(dialog.defaultPhrase, dialogSpots[0].GetChild(1).GetComponent<TextMeshProUGUI>()));
        while (true)
        {
            if (isWrite == false)
            {
                dialogPanel.GetChild(0).GetChild(0).gameObject.SetActive(true);
                dialogPanel.GetChild(0).GetChild(0).GetComponent<btnDialog>().dialog = dialog;
                dialogPanel.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Show me";
                dialogPanel.GetChild(0).GetChild(1).gameObject.SetActive(true);
                dialogPanel.GetChild(0).GetChild(1).GetComponent<btnDialog>().dialog = dialog;
                dialogPanel.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "I don't need";

                break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator RewardPhrase(DialogScriptable dialog)
    {
        dialogSpots[0].gameObject.SetActive(true);
        dialogSpots[0].GetChild(0).GetComponent<SpriteRenderer>().sprite = dialog.npcIcon;
        StartCoroutine(ShowText(dialog.questScripteble.rewardPhrase, dialogSpots[0].GetChild(1).GetComponent<TextMeshProUGUI>()));
        while (true)
        {
            if (isWrite == false)
            {
                dialogPanel.GetChild(0).GetChild(0).gameObject.SetActive(true);
                dialogPanel.GetChild(0).GetChild(0).GetComponent<btnDialog>().dialog = dialog;
                dialogPanel.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Accept";
                dialogPanel.GetChild(0).GetChild(1).gameObject.SetActive(true);
                dialogPanel.GetChild(0).GetChild(1).GetComponent<btnDialog>().dialog = dialog;
                dialogPanel.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Decline";

                break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator NoCompletedPhrase(DialogScriptable dialog)
    {
        dialogSpots[0].gameObject.SetActive(true);
        dialogSpots[0].GetChild(0).GetComponent<SpriteRenderer>().sprite = dialog.npcIcon;
        StartCoroutine(ShowText(dialog.questScripteble.noCompletedPhrase, dialogSpots[0].GetChild(1).GetComponent<TextMeshProUGUI>()));
        while (true)
        {
            if (isWrite == false)
            {
                dialogPanel.GetChild(0).GetChild(0).gameObject.SetActive(true);
                dialogPanel.GetChild(0).GetChild(0).GetComponent<btnDialog>().dialog = dialog;
                dialogPanel.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "In process";
                dialogPanel.GetChild(0).GetChild(1).gameObject.SetActive(true);
                dialogPanel.GetChild(0).GetChild(1).GetComponent<btnDialog>().dialog = dialog;
                dialogPanel.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Leave";

                break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator QuestAddPhrases(DialogScriptable dialog)
    {
        int i = 0;
        while (i < dialog.questScripteble.questions.Length)
        {
            if (isSkiped)
            {
                break;
            }
            if (isWrite == false)
            {
                dialogSpots[i].gameObject.SetActive(true);
                dialogSpots[i].GetChild(0).GetComponent<SpriteRenderer>().sprite = dialog.npcIcon;
                StartCoroutine(ShowText(dialog.questScripteble.questions[i], dialogSpots[i].GetChild(1).GetComponent<TextMeshProUGUI>()));
                i++;
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
        while (true)
        {
            if (isSkiped)
            {
                for (int k = 0; k < dialog.questScripteble.questions.Length; k++)
                {
                    dialogSpots[k].gameObject.SetActive(true);
                    dialogSpots[k].GetChild(0).GetComponent<SpriteRenderer>().sprite = dialog.npcIcon;
                    dialogSpots[k].GetChild(1).GetComponent<TextMeshProUGUI>().text = dialog.questScripteble.questions[k];
                }
                for (int k = 0; k < dialog.questScripteble.answer.Length; k++)
                {
                    if (dialogPanel.GetChild(0).GetChild(k).name != "Skip")
                    {
                        dialogPanel.GetChild(0).GetChild(k).gameObject.SetActive(true);
                        dialogPanel.GetChild(0).GetChild(k).GetComponent<btnDialog>().dialog = dialog;
                        dialogPanel.GetChild(0).GetChild(k).GetChild(0).GetComponent<TextMeshProUGUI>().text = dialog.questScripteble.answer[k];
                    }
                }
                break;
            }
            if (isWrite == false)
            {
                for (int k = 0; k < dialog.questScripteble.answer.Length; k++)
                {
                    if (dialogPanel.GetChild(0).GetChild(k).name != "Skip")
                    {
                        dialogPanel.GetChild(0).GetChild(k).gameObject.SetActive(true);
                        dialogPanel.GetChild(0).GetChild(k).GetComponent<btnDialog>().dialog = dialog;
                        dialogPanel.GetChild(0).GetChild(k).GetChild(0).GetComponent<TextMeshProUGUI>().text = dialog.questScripteble.answer[k];
                    }
                }
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isDialogOpened = false;
        isStoregOpened = false;
        for (int i = 0; i < dialogPanel.childCount; i++)
        {
            if (dialogPanel.GetChild(i).name == "DialogSpot1")
            {
                dialogSpots.Add(dialogPanel.GetChild(i));
                dialogPanel.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < dialogPanel.GetChild(0).childCount; i++)
        {
            if (dialogPanel.GetChild(0).GetChild(i).name != "Skip")
            {
                dialogPanel.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < storePanel.childCount; i++)
        {
            if (storePanel.GetChild(i).GetComponent<StoreSlot>() != null)
            {
                storeSlots.Add(storePanel.GetChild(i).GetComponent<StoreSlot>());
            }
        }
        storePanel.gameObject.SetActive(false);
        dialogPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isWrite == true)
            {
                isMiniSkiped = true;
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isStoregOpened)
            {
                CloseStore();
            }
            else if (isDialogOpened)
            {
                CloseDialog();
            }
        }
    }



    public void CloseStore()
    {
        storePanel.gameObject.SetActive(false);
        ClearStore();

        isStoregOpened = false;
        player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
    }

    public void CloseDialog()
    {
        StopAllCoroutines();
        ClearDialog();
        player.GetComponent<PlayerContrl>().PlayerInUIPanel(false);
    }

    public void AddStoreSlots(DialogScriptable dialog)
    {
        for (int i = 0; i < dialog.storeItems.Count; i++)
        {
            if (!dialog.storeItems[i].isSold)
            {
                foreach (StoreSlot storeSlot in storeSlots)
                {
                    if (storeSlot.isEmpty == true)
                    {
                        storeSlot.storeItem = dialog.storeItems[i];
                        storeSlot.item = dialog.storeItems[i].item;
                        storeSlot.amount = dialog.storeItems[i].leftCount;
                        storeSlot.isEmpty = false;
                        storeSlot.SetIcon(dialog.storeItems[i].item.icon);
                        storeSlot.itemAmountText.text = storeSlot.amount.ToString();
                        break;
                    }
                }
            }
        }


    }

    public void DeleteStoreSlot(StoreSlot storeSlot)
    {
        storeSlot.item = null;
        storeSlot.amount = 0;
        storeSlot.isEmpty = true;
        storeSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        storeSlot.iconGO.GetComponent<Image>().sprite = null;
        storeSlot.itemAmountText.text = "";
    }

    public void ClearDialog()
    {
        for (int i = 0; i < dialogSpots.Count; i++)
        {
            dialogSpots[i].gameObject.SetActive(false);
            dialogSpots[i].GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
            dialogSpots[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }
        for (int k = 0; k < dialogPanel.GetChild(0).childCount; k++)
        {
            if (dialogPanel.GetChild(0).GetChild(k).name != "Skip")
            {
                dialogPanel.GetChild(0).GetChild(k).gameObject.SetActive(false);
            }
        }
        isWrite = false;
        isMiniSkiped = false;
        isSkiped = false;
        isDialogOpened = false;
        dialogPanel.gameObject.SetActive(false);
    }

    public void RefreshStoreItems(DialogScriptable dialog)
    {
        foreach (StoreItem storeItem in dialog.storeItems)
        {
            storeItem.isSold = false;
            storeItem.leftCount = storeItem.amount;
        }
    }

    public void ClearStore()
    {
        if (selectedSlot != null)
        {
            selectedSlot.GetComponent<Image>().sprite = notSelectedSlotSprite;
        }
        selectedSlot = null;

        foreach (StoreSlot storeSlot in storeSlots)
        {
            DeleteStoreSlot(storeSlot);
        }
    }

    public void StartQuestDialog(DialogScriptable dialog, string coroutineName)
    {
        dialogPanel.gameObject.SetActive(true);
        isDialogOpened = true;
        player.GetComponent<PlayerContrl>().PlayerInUIPanel(true);
        switch (coroutineName)
        {
            case "AddQuest":
                StartCoroutine(QuestAddPhrases(dialog));
                break;
            case "NoCompletedQuest":
                StartCoroutine(NoCompletedPhrase(dialog));
                break;
            case "RewardQuest":
                StartCoroutine(RewardPhrase(dialog));
                break;
            case "DefaultPhrase":
                StartCoroutine(DefaultPhrase(dialog));
                break;
            default:
                Debug.Log("ERROR");
                break;
        }


    }
}
