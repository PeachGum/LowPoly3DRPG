using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Transform ShopItemsParents;
    [SerializeField]
    private List<ShopSlot> shopSlots = new List<ShopSlot>(32);
    // Start is called before the first frame update

    void OnDisable()
    {
        for(int i=0; i<shopSlots.Count; i++)
        {
            if(shopSlots[i].item != null)
            {
                shopSlots[i].item = null;
                shopSlots[i].icon.color = shopSlots[i].transparentColor;
            }
        }
    }
    void Start()
    {
        shopSlots = ShopItemsParents.GetComponentsInChildren<ShopSlot>().ToList();
    }

    // Update is called once per frame
    public void SetShop(List<Item> items)
    {
        for(int i=0; i<items.Count; i++)
        {
            shopSlots[i].AddItem(items[i]);
        }
    }
}
