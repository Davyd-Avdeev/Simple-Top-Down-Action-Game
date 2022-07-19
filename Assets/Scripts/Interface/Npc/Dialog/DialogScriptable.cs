using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogScriptable : ScriptableObject
{
    public string defaultPhrase;
    public Sprite npcIcon;
    public QuestScriptable questScripteble;
    public List<StoreItem> storeItems;
    
}

[System.Serializable]
public class StoreItem {
    public ItemScriptableObject item;
    public int amount;
    public bool isSold;
    public int leftCount;
}

