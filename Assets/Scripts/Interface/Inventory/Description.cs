using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Description : MonoBehaviour
{
    private GameObject text;
    private bool isActive = false;

    private void Start()
    {
        text = GameObject.FindGameObjectWithTag("Description");
    }

    private void Update()
    {
        if (isActive)
        {
            Debug.Log(text.GetComponent<TextMeshPro>().text);
        }
        else
        {
            //text.GetComponent<TextMeshPro>().text = "ssssss";
        }
    }

    private void OnMouseEnter()
    {
        isActive = true;
    }

    private void OnMouseOver()
    {
        isActive = true;
    }

    private void OnMouseExit()
    {
        isActive = false;
    }
}
