using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickslotInventory : MonoBehaviour
{
    // Объект у которого дети являются слотами
    public Transform quickslotParent;
    public InventoryManager im;
    public InterfaceManager infM;
    public int currentQuickslotID = 0;
    public bool slotActived = false;
    public Sprite selectedSprite;
    public Sprite notSelectedSprite;
    public WeaponController wpContrl;
    public PlayerContrl plContrl;

    public InventorySlot activeSlot = null;

    private Transform player;
    private GameObject go;



    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("Canvas").GetComponent<InventoryManager>();
        infM = GameObject.FindGameObjectWithTag("interfaceCanvas").GetComponent<InterfaceManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        wpContrl = player.GetComponent<WeaponController>();
        plContrl = player.GetComponent<PlayerContrl>();
        Cheker();
    }

    void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");

        if (mw > 0.1) // Используем колесико мышки
        {
            // Берем предыдущий слот и меняем его картинку на обычную

            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            slotActived = false;
            // Здесь добавляем что случится когда мы УБЕРАЕМ ВЫДЕЛЕНИЕ со слота (Выключить нужный нам предмет, поменять аниматор ...)

            // Если крутим колесиком мышки назад и наше число currentQuickslotID равно 0, то выбираем наш последний слот
            if (currentQuickslotID <= 0)
            {
                currentQuickslotID = quickslotParent.childCount - 1;
            }
            else
            {
                // Уменьшаем число currentQuickslotID на 1
                currentQuickslotID--;
            }
            // Берем предыдущий слот и меняем его картинку на "выбранную"

            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
            slotActived = true;
            activeSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();
            // Здесь добавляем что случится когда мы ВЫДЕЛЯЕМ слот (Включить нужный нам предмет, поменять аниматор ...)
            Cheker();
        }
        if (mw < -0.1)
        {

            // Берем предыдущий слот и меняем его картинку на обычную

            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            slotActived = false;
            // Здесь добавляем что случится когда мы УБЕРАЕМ ВЫДЕЛЕНИЕ со слота (Выключить нужный нам предмет, поменять аниматор ...)

            // Если крутим колесиком мышки вперед и наше число currentQuickslotID равно последнему слоту, то выбираем наш первый слот (первый слот считается нулевым)
            if (currentQuickslotID >= quickslotParent.childCount - 1)
            {
                currentQuickslotID = 0;
            }
            else
            {
                // Прибавляем к числу currentQuickslotID единичку
                currentQuickslotID++;
            }
            // Берем предыдущий слот и меняем его картинку на "выбранную"

            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
            slotActived = true;
            activeSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();
            // Здесь добавляем что случится когда мы ВЫДЕЛЯЕМ слот (Включить нужный нам предмет, поменять аниматор ...)
            Cheker();

        }
        // Используем цифры
        for (int i = 0; i < quickslotParent.childCount; i++)
        {
            // если мы нажимаем на клавиши 1 по 5 то...
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                // проверяем если наш выбранный слот равен слоту который у нас уже выбран, то
                if (currentQuickslotID == i)
                {
                    // Ставим картинку "selected" на слот если он "not selected" или наоборот
                    if (quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == notSelectedSprite)
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                        slotActived = true;
                        activeSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();

                        //foreach ...
                        Cheker();
                    }
                    else
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                        slotActived = false;
                        activeSlot = null;
                        //foreach ...
                        Cheker();
                    }
                }
                // Иначе мы убираем свечение с предыдущего слота и светим слот который мы выбираем
                else
                {
                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                    slotActived = false;
                    currentQuickslotID = i;

                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                    slotActived = true;
                    activeSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();
                    Cheker();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (activeSlot != null)
            {
                if (activeSlot.item != null)
                {
                    if (activeSlot.item.isConsumeable && !im.isOpened)
                    {
                        // Применяем изменения к здоровью (будущем к голоду и жажде) 
                        ChangeCharacteristics();

                        if (activeSlot.amount <= 1)
                        {
                            for (int i = 0; i < player.GetChild(0).childCount; i++)
                            {
                                if (activeSlot.item.model.name == player.GetChild(0).GetChild(i).GetComponent<Item>().item.model.name)
                                {
                                    Destroy(player.GetChild(0).GetChild(i).gameObject);
                                }
                            }
                            activeSlot.GetComponentInChildren<DragAndDropItem>().NullifySlotData(activeSlot);
                            player.GetComponent<PlayerContrl>().ChangeState(false);
                        }
                        else
                        {
                            activeSlot.amount--;
                            activeSlot.itemAmountText.text = activeSlot.amount.ToString();
                        }
                    }
                }
            }
        }
    }

    private void ChangeCharacteristics()
    {
        float hp = infM.HPBar.GetComponent<HPBar>().fill;
        // Если здоровье + добавленное здоровье от предмета меньше или равно 100, то делаем вычисления... 
        if (hp + activeSlot.item.changeHealth <= 1f)
        {
            float newHealth = hp + activeSlot.item.changeHealth;
            infM.HPBar.GetComponent<HPBar>().fill = newHealth;
        }
        // Иначе, просто ставим здоровье на 100
        else
        {
            infM.HPBar.GetComponent<HPBar>().fill = 1f;
        }
    }

    public void Cheker()
    {
        if (activeSlot != null)
        {
            if (activeSlot.item != null)
            {
                for (int i = 0; i < player.GetChild(0).childCount; i++)
                {
                    if (player.GetChild(0).GetChild(i).GetComponent<Item>().item != null)
                    {
                        if (activeSlot.item.name != player.GetChild(0).GetChild(i).GetComponent<Item>().item.name)
                        {
                            player.GetChild(0).GetChild(i).gameObject.SetActive(false);
                        }

                        if (activeSlot.item.name == player.GetChild(0).GetChild(i).GetComponent<Item>().item.name)
                        {
                            if (activeSlot.item.itemType == ItemType.Consumables)
                            {
                                continue;
                            }
                            player.GetChild(0).GetChild(i).gameObject.SetActive(true);
                            plContrl.ChangeState(true);
                            plContrl.haveItem = true;
                            if (activeSlot.item.itemType == ItemType.Weapon)
                            {
                                UpdateMagInfo();
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < player.GetChild(0).childCount; i++)
                {
                    player.GetChild(0).GetChild(i).gameObject.SetActive(false);
                    plContrl.ChangeState(false);
                    plContrl.haveItem = false;
                    wpContrl.WeaponInfoClear();
                }
            }
        }
        else
        {
            for (int i = 0; i < player.GetChild(0).childCount; i++)
            {
                player.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
            plContrl.ChangeState(false);
            plContrl.haveItem = false;
            wpContrl.WeaponInfoClear();

        }

    }

    public void UpdateMagInfo()
    {
        wpContrl.SwapWeapon();
        wpContrl.startReloadTime = activeSlot.item.reloadTime;
        wpContrl.timeReload = activeSlot.item.reloadTime;
        wpContrl.startShotTime = activeSlot.item.shootTime;
        wpContrl.bulletDrob = activeSlot.item.bulletDrop;
        wpContrl.bulletSpreadVariance = activeSlot.item.spreadVariance;
        wpContrl.canShoot = true;
        if (activeSlot.magItem != null)
        {
            infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = activeSlot.magAmount + "/" + activeSlot.magItem.maximumAmount;
        }
        else
        {
            infM.ammoInfo.GetComponent<TextMeshProUGUI>().text = activeSlot.magAmount + "/0";
        }
        
    }
}
