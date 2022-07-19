using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest")]
public class QuestCreator : QuestScriptable
{
    private void Start()
    {
        questType = QuestType.Default;
    }
}
