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

    // �÷��̾�� �κ��丮 UI�� ���������� ī�޶� �������� ������ϰ� ������ �� �� ����
    // ���� ����� Player_Attack ��ũ��Ʈ���� �����ϰ� ī�޶�� �� ��ũ��Ʈ���� �����Ѵ�
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
                //���� â�� ����������
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

            //��� �������� ��� ������ ����
            if (item.itemType == Item.ItemType.Equipment)
            {
                Inventory.instance.inventorySlots[index].quantityText.text = "";
            }
            //�����ϰ� �ִ� ��� �������̶�� �� �κ��丮���� ������� �ʴ´�.
            else if(item.itemType != Item.ItemType.Equipment)//&& Player_Equipment.instance.FindEquipmentIndex(item) == -1)
            {
                int quickIndex = Inventory.instance.quickSlots.FindIndex(d => d.item == item);

                if (quickIndex != -1)
                {
                    Inventory.instance.quickSlots[quickIndex].UpdateQuantity(Inventory.instance.itemQuantity[index]);
                }
            }
            //�����Կ� �������� �ִ��� �˻� ������ �ش� �ε��� ��ȯ ������ - 1��ȯ
            
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