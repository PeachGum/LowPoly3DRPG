using UnityEngine;


public class ChoiceButton : MonoBehaviour
{
    public DialogueUIManager ui;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        index = transform.GetSiblingIndex();
    }

    public void ClickFromIndex()
    {
        ui.choiceDialogue = ui.npcTargetObjectData.choiceDialogue[index];
        ui.OffUI(3);
        if (ui.npcTargetObjectScript.isShop && index == 0 && ui.npcTargetObjectScript.sellItems != null)
        {
            ui.OffAllUI();
            ui.OnUI(5);
            ui.inventoryUI.ResetInventoryPosition();
            ui.shopUI.SetShop(ui.npcTargetObjectScript.sellItems);

            //인벤토리가 꺼져있을떄 켜줌
            if (!ui.inventoryUI.inventoryParent.activeSelf)
            {
                ui.inventoryUI.OnInventory();
            }
            return;
        }
        else if(ui.npcTargetObjectScript.isShop && index == 1)
        {
            ui.OffAllUI();
        }
    }
}
