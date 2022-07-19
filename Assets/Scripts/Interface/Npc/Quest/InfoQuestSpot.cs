using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoQuestSpot : MonoBehaviour
{
    public bool isEmpty;
    public TextMeshProUGUI qText;
    public btnQuestSlot quest;

    private void Start()
    {
        qText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void OnChanged()
    {        
        transform.GetChild(0).GetComponent<TextMeshPro>().text = qText.text;
    }
}
