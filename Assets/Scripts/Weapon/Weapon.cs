using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;
    [SerializeField]
    private GameObject ammoInfo;
    private InventoryManager im;
    [SerializeField]
    private InterfaceManager infM;
    public ItemScriptableObject magItem;
    public int magAmount;

    public int bulletIn—hamber;
    public int bulletDrob;

    List<Quaternion> bullets;

    private string calibre;
    public float startReloadTime;
    public float timeReload;

    public float startShotTime;
    private float timeShot;

    bool buttonPresed;
    bool isReloaded;
    bool disabled;

    private TrailRenderer bulletTrail;

    private float LastShootTime;

    [SerializeField]
    private Vector3 bulletSpreadVariance = new Vector2(0.1f, 0.1f);

    private Vector2 GetDirection()
    {
        Vector2 direction = transform.right;

        if (true)
        {
            direction += new Vector2(
                Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y)
            );

            direction.Normalize();
        }

        return direction;
    }

    private void Awake()
    {
        ammoInfo = GameObject.FindGameObjectWithTag("ammoInfo");
        infM = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
    }

    private void Start()
    {
        infM = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
        bulletTrail = bullet.GetComponent<TrailRenderer>();
        calibre = GetComponent<Item>().item.calibre;
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        bullets = new List<Quaternion>(bulletDrob);
        for (int i = 0; i < bulletDrob; i++)
        {
            bullets.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    IEnumerator ReloadingCoroutine()
    {
        while (timeReload > 0)
        {
            yield return null;
            timeReload -= Time.deltaTime;
            infM.reloadInfo.gameObject.SetActive(true);
            infM.reloadInfo.GetComponent<TextMeshProUGUI>().text = "Reloading " + Mathf.CeilToInt(timeReload);            
        }

        //reloadInfo.GetComponent<TextMeshProUGUI>().text = "Ready";
        infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = magAmount + "/" + magItem.maximumAmount;
        isReloaded = false;
        infM.reloadInfo.gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit2D Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while (time < 0.5f)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = Hit.point;

        Destroy(Trail.gameObject, Trail.time);
        Debug.Log("I DIED");
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && magAmount != bulletIn—hamber)
        {
            Reload();
            timeReload = startReloadTime;
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            buttonPresed = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            buttonPresed = false;
        }

        if (isReloaded)
        {
            return;
        }

        if (timeShot <= 0 && buttonPresed)
        {
            timeShot = startShotTime;
            Shoot();
        }
        else
        {
            timeShot -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        if (magItem == null)
        {
            Reload();
            return;
        }
        else
        {
            if (magAmount < 1) // (bulletCount < 1)
            {
                Reload();
                return;
            }
        }

        for (int i = 0; i < bulletDrob; i++)
        {
            Vector2 direction = GetDirection();

            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, direction);



            if (hitInfo.collider != null)
            {
                TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hitInfo));

                LastShootTime = Time.time;
            }
        }

        magAmount--;
        infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = magAmount + "/" + magItem.maximumAmount;
    }

    void Reload()
    {
        // »˘ÂÏ Ï‡„‡ÁËÌ ‚ ËÌ‚ÂÌÚÓÂ
        foreach (InventorySlot slot in im.slots)
        {
            if (slot.item != null)
            {
                if (slot.item.itemType == ItemType.Magazine)
                {
                    if (slot.item.calibre == calibre)
                    {
                        if (!slot.item.isEmpty)
                        {
                            if (slot.amount >= 1)
                            {
                                // ¬˚„ÛÁÍ‡ Ï‡„‡ÁËÌ‡
                                if (magItem != null)
                                {
                                    Debug.Log("w");
                                    if (magAmount >= 1)
                                    {
                                        im.AddItem(magItem, magAmount, null, 0);
                                    }
                                    else
                                    {
                                        im.AddItem(magItem.e_mag, 1, null, 0);
                                    }
                                }

                                magItem = slot.item;
                                magAmount = slot.amount;
                                bulletIn—hamber = slot.item.maximumAmount;
                                slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData(slot);

                                isReloaded = true;
                                timeReload = startReloadTime;
                                StartCoroutine(ReloadingCoroutine());
                                return;
                            }
                        }
                    }
                }
            }
        }

        //reloadInfo.GetComponent<TextMeshProUGUI>().text = "Not magazine";
    }


    private void OnDisable()
    {
        disabled = true;
        buttonPresed = false;
        //infM.ammoInfo.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (disabled && isReloaded)
        {
            StartCoroutine(ReloadingCoroutine());
        }
        disabled = false;
        //infM.ammoInfo.gameObject.SetActive(true);

        if (magItem != null)
        {
            infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = magAmount + "/" + magItem.maximumAmount;
        }
        else
        {
            infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = "0/0";
        }
    }

    
}
