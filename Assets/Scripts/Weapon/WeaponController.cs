using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint;
    private QuickslotInventory qm;
    private InventoryManager im;
    public InterfaceManager infM;
    public int bulletDrob;
    List<Quaternion> bullets;

    private string calibre;
    public float startReloadTime;
    public float timeReload;

    public float startShotTime;
    private float timeShot;

    public bool canShoot = false;

    public bool buttonPresed;
    public bool isReload;
    public bool disabled;

    private TrailRenderer bulletTrail;

    private float LastShootTime;

    [SerializeField]
    public Vector3 bulletSpreadVariance = new Vector2(0.1f, 0.1f);
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

    IEnumerator ReloadingCoroutine(InventorySlot slot)
    {
        isReload = true;
        while (timeReload > 0)
        {
            yield return null;
            timeReload -= Time.deltaTime;
            
            infM.reloadInfo.GetComponent<TextMeshProUGUI>().text = "Reloading " + Mathf.CeilToInt(timeReload);
        }
        // загрузка
        if (qm.activeSlot.magItem != null)
        {
            Debug.Log("w");
            if (qm.activeSlot.magAmount >= 1)
            {
                im.AddItem(qm.activeSlot.magItem, qm.activeSlot.magAmount, null, 0);
            }
            else
            {
                im.AddItem(qm.activeSlot.magItem.e_mag, 1, null, 0);
            }
        }
        // загрузка
        qm.activeSlot.magItem = slot.item;
        qm.activeSlot.magAmount = slot.amount;
        slot.GetComponentInChildren<DragAndDropItem>().NullifySlotData(slot);
        bulletTrail = qm.activeSlot.magItem.bullet.GetComponent<TrailRenderer>();
        canShoot = false;
        timeReload = startReloadTime;
        infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = qm.activeSlot.magAmount + "/" + qm.activeSlot.magItem.maximumAmount;
        canShoot = true;
        isReload = false;
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

    }

    private void Awake()
    {
        infM = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
        qm = GameObject.FindGameObjectWithTag("QuickslotInventory").GetComponent<QuickslotInventory>();
    }

    private void Start()
    {
        isReload = false;
        infM = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        bullets = new List<Quaternion>(bulletDrob);
        for (int i = 0; i < bulletDrob; i++)
        {
            bullets.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            timeReload = startReloadTime;
            Reload();            
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

        if (isReload)
        {
            return;
        }

        if (timeShot <= 0 && buttonPresed && canShoot)
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
        if (qm.activeSlot.magItem == null)
        {
            Reload();
            return;
        }
        else
        {
            if (qm.activeSlot.magAmount < 1) // (bulletCount < 1)
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
                TrailRenderer trail = Instantiate(qm.activeSlot.magItem.bullet.GetComponent<TrailRenderer>(), firePoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hitInfo));

                LastShootTime = Time.time;
            }
        }

        qm.activeSlot.magAmount--;
        infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = qm.activeSlot.magAmount + "/" + qm.activeSlot.magItem.maximumAmount;
    }

    void Reload()
    {
        // Ищем магазин в инвенторе
        foreach (InventorySlot slot in im.slots)
        {
            if (slot.item != null)
            {
                if (slot.item.itemType == ItemType.Magazine)
                {
                    if (slot.item.calibre == qm.activeSlot.item.calibre)
                    {
                        if (!slot.item.isEmpty)
                        {
                            if (slot.amount >= 1)
                            {
                                infM.reloadInfo.gameObject.SetActive(true);
                                StartCoroutine(ReloadingCoroutine(slot));
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
        //disabled = true;
        buttonPresed = false;
        StopAllCoroutines();
        infM.reloadInfo.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //if (disabled && isReloaded)
        //{
        //    StartCoroutine(ReloadingCoroutine());
        //}
        //disabled = false;

        //if (magItem != null)
        //{
        //    infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = qm.activeSlot.magAmount + "/" + qm.activeSlot.magItem.maximumAmount;
        //}
        //else
        //{
        //    infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = "0/0";
        //}
    }

    public void SwapWeapon()
    {
        StopAllCoroutines();
        isReload = false;
        canShoot = true;
    }

    public void WeaponInfoClear()
    {
        startReloadTime = 0;
        timeReload = 0;
        startShotTime = 0;
        bulletDrob = 0;
        bulletSpreadVariance = Vector2.zero;
        canShoot = false;
        infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = "0/0";   
    }
}
