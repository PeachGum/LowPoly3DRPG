using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;

public class InventoryUI : MonoBehaviour
{
    public Transform quickInventoryParent;
    public GameObject inventoryParent, EquipmentInventoryParent;
    public DragSlotImage dragSlotImage;
    public TextMeshProUGUI coinText;
    private Vector3 inventoryInitPosition;

    public Player_Attack playerAttack;
    public CameraManager cameraManager;
    [HideInInspector]
    public DialogueUIManager dialogueUIManager;
    [HideInInspector]
    public SlotToolTip sizeToolTip;

    int quickIndex = -1;

    void Start()
    {
        Inventory.instance.onItemChangedCallback += UpdateUI;
        Inventory.instance.onQuantityChangedCallback += UpdateUIQuantity;
        Inventory.instance.onOnUpdateMoneyCallback += UpdateMoney;
        Inventory.instance.money = 3000;

        UpdateMoney();
        dialogueUIManager = GetComponent<DialogueUIManager>();
        inventoryInitPosition = inventoryParent.transform.position;
        sizeToolTip = GameObject.Find("UIManager_Scripts").GetComponent<SlotToolTip>();

    }

    void Update()
    {
        PressNumberForQuickUse();
    }
    void PressNumberForQuickUse()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) quickIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) quickIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) quickIndex = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) quickIndex = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5)) quickIndex = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha6)) quickIndex = 5;
        else if (Input.GetKeyDown(KeyCode.Alpha7)) quickIndex = 6;
        else if (Input.GetKeyDown(KeyCode.Alpha8)) quickIndex = 7;

        if(quickIndex != -1)
        {
            Inventory.instance.quickSlots[quickIndex].UseItem();

            quickIndex = -1;
        }
        
    }

    // 플레이어는 인벤토리 UI가 켜져있을때 카메라 움직임을 제어당하고 공격을 할 수 없다
    // 공격 제어는 Player_Attack 스크립트에서 제어하고 카메라는 이 스크립트에서 제어한다
    public void OnInventory()
    {
        if(cameraManager.movingCamera.gameObject.activeSelf || cameraManager.stopCamera.gameObject.activeSelf)
        {
            //inventoryUI.SetActive(!inventoryUI.activeSelf);
            inventoryParent.SetActive(!inventoryParent.activeSelf);
            Cursor.visible = inventoryParent.activeSelf;
            cameraManager.StopCamera(inventoryParent.activeSelf);
            if (inventoryParent.activeSelf)
            {
                IconOnEnable();
                Cursor.lockState = CursorLockMode.None;
                Player_Equipment.instance.playerAttack.canAttack = false;
            }
            else if (!inventoryParent.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                sizeToolTip.HideToolTip();
                //상점 창이 열려있을때
                if (dialogueUIManager.listUI[5].gameObject.activeSelf)
                {
                    dialogueUIManager.listUI[5].gameObject.SetActive(false);
                }

                Player_Equipment.instance.playerAttack.canAttack = true;
                Player_Equipment.instance.playerMovement.canMove = true;


            }
        }
    }


    private void IconOnEnable()
    {
        for(int i=0; i<= Inventory.instance.items.Count; i++)
        {
            if (Inventory.instance.items[i] != null)
            {
                Inventory.instance.inventorySlots[i].icon.enabled = true;
                Inventory.instance.inventorySlots[i].icon.color = Inventory.instance.inventorySlots[i].iconColor;
            }
            else
            {
                break;
            }
        }
    }

    void UpdateUI(int index)
    {
        if (Inventory.instance.items[index] != null)
        {
            Inventory.instance.inventorySlots[index].AddItem(Inventory.instance.items[index]);
        }
        
    }

    void UpdateUIQuantity(Item item, int index, int val)
    {
        if(index != -1)
        {
            Inventory.instance.itemQuantity[index] += val;


            Inventory.instance.inventorySlots[index].UpdateQuantity(Inventory.instance.itemQuantity[index]);

            //장비 아이템일 경우 갯수를 숨김
            if (item.itemType == Item.ItemType.Equipment)
            {
                Inventory.instance.inventorySlots[index].quantityText.text = "";
            }
            //착용하고 있는 장비 아이템이라면 퀵 인벤토리에서 사라지지 않는다.
            else if(item.itemType != Item.ItemType.Equipment)//&& Player_Equipment.instance.FindEquipmentIndex(item) == -1)
            {
                int quickIndex = Inventory.instance.quickSlots.FindIndex(d => d.item == item);

                if (quickIndex != -1)
                {
                    Inventory.instance.quickSlots[quickIndex].UpdateQuantity(Inventory.instance.itemQuantity[index]);
                }
            }
            //퀵슬롯에 아이템이 있는지 검사 있으면 해당 인덱스 반환 없으면 - 1반환
            
        }
        
    }

    public void UpdateAllSlot()
    {
        foreach(var slot in Inventory.instance.inventorySlots)
        {
            slot.UpdateSlot();
        }
    }

    public void UpdateMoney()
    {
        coinText.text = Inventory.instance.money.ToString();
    }

    public void ResetInventoryPosition()
    {
        inventoryParent.transform.position = inventoryInitPosition;
    }
}