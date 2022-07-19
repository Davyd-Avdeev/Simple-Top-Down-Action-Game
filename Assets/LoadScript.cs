using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScript : MonoBehaviour
{

    void Start()
    {
        GameObject.FindGameObjectWithTag("Canvas").SetActive(true);
        GameObject.FindGameObjectWithTag("npcCanvas").SetActive(true);
    }
}
