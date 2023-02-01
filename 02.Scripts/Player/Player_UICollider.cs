using UnityEngine;

public class Player_UICollider : MonoBehaviour
{
    // 0 : [E}  대화
    // 1 : 메세지 박스
    // 2 : [E] 이동
    // 3 : 선택지
    // 4 : [E] 획득
    // 5 : 상점 창
    // 6 : 일시정지 창
    // 7 : 옵션 창
    // 8 : 사망 테두리
    // 9 : 해골 이미지
    // 10 : 부활 선택 창
    // 11 : 일시정지 선택창
    // 12 : 정말 나가시겠습니까?
    // 13 : [E] 보스 소환

    public DialogueUIManager ui;

    public Player_Movement moveScript;
    //delete
    private GameObject targetColliderObject;

    public Transform itemRoot;

    
    //public Cine
    void Start()
    {
        ui = FindObjectOfType<DialogueUIManager>();
    }
    
    void OnAction()
    {
        //타이핑이 되고있을때 액션버튼을 누르면 텍스트가 한번에 나오면서 스킵됨
        if (ui.listUI[0].IsActive())
        {
            
            ui.OffUI(0);
            ui.OnUI(1);
            moveScript.PlayerStop();
            
            
        }
        //NPC와 대화 E키를 누르면 기존박스를 비활성화시키고 텍스트 다이어리 활성화
        else if (ui.listUI[1].IsActive())
        {
            ui.SkipText();
        }
        else if (ui.listUI[2].IsActive())
        {
            ui.OffUI(2);
            targetColliderObject.GetComponent<PortalScript>().MoveScene();
        }
        else if (ui.listUI[13].IsActive())
        {
            ui.OffUI(13);
            AnubisBoss_Summon bossScript = targetColliderObject.GetComponent<AnubisBoss_Summon>();
            bossScript.StartCoroutine(bossScript.SummonBoss());
        }
    }

    
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("NPC") || col.CompareTag("CHOICE_NPC"))
        {
            //NPC와 콜라이더 접촉 시, UI매니저에게 접촉한 NPC의 정보를 넘겨주고 [E]대화 이미지를 보여줌
            //대화창이 꺼져있을때만 e대화 활성화 (대화 중에 다른 npc가 다가왔을때 [E]대화 활성화 방지)

            if (!ui.listUI[1].IsActive() && !ui.listUI[4].IsActive()) //&& !ui.inventoryUI.inventoryParent.activeSelf)
            {
                ui.npcTargetObject = col.gameObject;
                ui.OnUI(0);
                if (col.gameObject.CompareTag("CHOICE_NPC"))
                {
                    ui.isChoice = true;
                }
            }
        }
        //아이템에 닿았을떄
        else if (col.CompareTag("ITEM"))
        {
            col.GetComponent<ItemScript>().PickUp(itemRoot);
        }

        //포탈에 닿았을떄
        else if (col.CompareTag("PORTAL"))
        {
            targetColliderObject = col.gameObject;
        }

        //보스 소환
        else if(col.CompareTag("BOSS_SUMMON"))
        {
            ui.OnUI(13);
            targetColliderObject = col.gameObject;
        }
        
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("NPC") || col.CompareTag("CHOICE_NPC") && col.gameObject == ui.npcTargetObject)
        {
            ui.npcTargetObject = col.gameObject;
            ui.OffAllUI();
        }
        else if (col.CompareTag("PORTAL") || col.CompareTag("ITEM") || col.CompareTag("BOSS_SUMMON"))
        {
            targetColliderObject = null;
            ui.OffAllUI();
        }
    }
}
