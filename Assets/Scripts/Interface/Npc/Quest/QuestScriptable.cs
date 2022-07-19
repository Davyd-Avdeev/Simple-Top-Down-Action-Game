using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType { Default, FindItem, Massacre }
public class QuestScriptable : ScriptableObject
{
    public string qName;
    public QuestType questType;
    public string qDescription;
    public string noCompletedPhrase;
    public string rewardPhrase;
    public string[] questions;
    public string[] answer = new string[2];
    public ItemScriptableObject needItem;
    public int count;
    public ItemScriptableObject rewardItem;
    public int countReward;
}
