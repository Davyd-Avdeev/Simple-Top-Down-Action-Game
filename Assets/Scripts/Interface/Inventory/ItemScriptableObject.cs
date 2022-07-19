using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType {Default,Consumables,Weapon,Magazine}
public class ItemScriptableObject : ScriptableObject
{

    public string itemName;
    public int maximumAmount;
    public GameObject itemPrefab;
    public GameObject model;
    public Sprite icon;
    public ItemType itemType;
    [TextArea]
    public string itemDescription;
    public bool isConsumeable;

    [Header("Advanced Characteristics")]
    [Range(0f, 1f)]
    public float changeHealth;
    
    public bool isEmpty;
    

    [Header("Advanced Characteristi2cs")]
    public string calibre;
    public GameObject bullet;
    public ItemScriptableObject e_mag;
    public int bulletDrop;
    public int reloadTime;
    public float shootTime;
    public Vector2 spreadVariance;
}
