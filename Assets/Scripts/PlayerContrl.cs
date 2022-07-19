using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerContrl : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    public int viewIndex = 0;
    private float angle = 0;

    public bool haveItem;
    public Sprite notHaveItemSprite;
    public Sprite haveItemSprite;
    public ItemScriptableObject havedItem;

    public Vector2 notItemColliderOffset;
    public float notItemRadius;
    public Vector2 itemColliderOffset;
    public float itemRadius;
    private InventoryManager im;
    private InterfaceManager infM;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        infM = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
        haveItem = false;
        rb = GetComponent<Rigidbody2D>();
        ChangeState(false);
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
        var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="haveItem">True = HAVE. FALSE = DON'T HAVE</param>
    public void ChangeState(bool haveItem)
    {
        if (haveItem)
        {
            GetComponent<SpriteRenderer>().sprite = haveItemSprite;
            GetComponent<CircleCollider2D>().radius = itemRadius;
            GetComponent<CircleCollider2D>().offset = itemColliderOffset;
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            //infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = "0/0";
            GetComponent<SpriteRenderer>().sprite = notHaveItemSprite;
            GetComponent<CircleCollider2D>().radius = notItemRadius;
            GetComponent<CircleCollider2D>().offset = notItemColliderOffset;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void PlayerInUIPanel(bool inPanel)
    {
        if (inPanel)
        {
            ChangeState(false);
            GetComponent<PlayerContrl>().enabled = false;
            im.quickslotPanel.gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
            infM.ammoInfo.gameObject.SetActive(false);
            infM.reloadInfo.gameObject.SetActive(false);
            GetComponent<WeaponController>().enabled = false;
        }
        else
        {
            if (haveItem)
            {
                ChangeState(true);
            }
            GetComponent<PlayerContrl>().enabled = true;
            im.quickslotPanel.gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
            infM.ammoInfo.gameObject.SetActive(true);

            GetComponent<WeaponController>().enabled = true;
        }
    }
}
