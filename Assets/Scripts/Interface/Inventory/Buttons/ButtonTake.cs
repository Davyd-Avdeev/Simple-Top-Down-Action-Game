using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonTake : MonoBehaviour
{
    private Item takableItem;
    public GameObject text;
    private Transform player;
    public InventoryManager im;
    private InterfaceManager intefaceManager;
    private QuestManager qm;
    public float takeDistance;
    private bool isActive = false;

    private void Start()
    {
        takableItem = transform.gameObject.GetComponent<Item>();
        qm = GameObject.FindGameObjectWithTag("questCanvas").GetComponent<QuestManager>();
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
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
                    qm.QuestItemCheker(takableItem.item, takableItem.amount);
                    im.CheckHaveItem(transform.gameObject.GetComponent<Item>().item);
                    if (takableItem.item.itemType == ItemType.Weapon)
                    {
                        im.AddItem(takableItem.item, takableItem.amount, takableItem.magItem, takableItem.magAmount);
                    }
                    else
                    {
                        im.AddItem(takableItem.item, takableItem.amount, null, 0);
                    }                   
                    Destroy(transform.gameObject);
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


    private bool HaveWeapon()
    {
        for (int i = 0; i < player.GetChild(0).childCount; i++)
        {
            if (player.GetChild(0).GetChild(i).GetComponent<Item>() != null)
            {
                if (player.GetChild(0).GetChild(i).GetComponent<Item>().item.model.name == transform.gameObject.GetComponent<Item>().item.model.name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void QuestUpdate()
    {

    }

    private void OnMouseEnter()
    {
        isActive = true;
    }

    private void OnMouseExit()
    {
        isActive = false;
    }
}
