using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotToolTip : MonoBehaviour
{
    public GameObject base_Outer;
    private RectTransform base_Outer_Rect;

    public Image img_ItemImage;
    public TextMeshProUGUI txt_ItemName;
    public TextMeshProUGUI txt_ItemDesc;
    public TextMeshProUGUI txt_ItemHowToUse;
    public TextMeshProUGUI txt_ItemPrice;

    private DialogueUIManager ui;
    [HideInInspector]
    public bool canTooltip = true;

    private void Start()
    {
        ui = GetComponent<DialogueUIManager>();
        base_Outer_Rect = base_Outer.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    public void ShowToolTip(Item item, Vector3 pos, bool isTop)
    {
        if(canTooltip && !base_Outer.activeSelf)
        {
            base_Outer.SetActive(true);
            
            if (isTop)
            {
                base_Outer_Rect.pivot = Vector2.up;
            }
            else if(!isTop)
            {
                base_Outer_Rect.pivot = Vector2.zero;
                //pos += new Vector3(Screen.width * 0.015f,0,0);
            }    
            

            base_Outer.transform.position = pos;
            //Vector3 mousePos = Input.mousePosition;
            //base_Outer.transform.position = mousePos;


            txt_ItemName.text = item.korName;
            txt_ItemDesc.text = item.itemDesc;
            txt_ItemPrice.text = item.itemPrice.ToString();


            //���� UI�� �������� ���
            if (!ui.listUI[5].gameObject.activeSelf)
            {
                if (item.itemType == Item.ItemType.Used)
                {
                    txt_ItemHowToUse.text = "<color=#00FF00>[��Ŭ��]</color> ����ϱ�";
                }
                else if (item.itemType == Item.ItemType.Equipment)
                {
                    txt_ItemHowToUse.text = "<color=#00FF00>[��Ŭ��]</color> �����ϱ�";
                }
                else if (item.itemType == Item.ItemType.ETC || item.itemType == Item.ItemType.Ingredient)
                {
                    txt_ItemHowToUse.text = "";
                }
            }
            else
            {
                txt_ItemHowToUse.text = "<color=#00FF00>[��Ŭ��]</color> �ŷ��ϱ�";
            }
            
            img_ItemImage.sprite = item.itemImage;
        }
    }

    public void HideToolTip()
    {
        base_Outer.SetActive(false);
    }

    public void ViewUp()
    {
        
    }

    public void ViewDown()
    {

    }
}
