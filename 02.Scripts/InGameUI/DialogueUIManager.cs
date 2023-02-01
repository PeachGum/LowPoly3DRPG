using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Image = UnityEngine.UI.Image;

public class DialogueUIManager : MonoBehaviour
{
    // 0 : [E} 대화
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
    
    public TextMeshProUGUI mainTextBox, nameTextBox;
    public GameObject player, nextText;

    public InventoryUI inventoryUI;
    [HideInInspector]
    public ShopUI shopUI;


    //코루틴 관련
    private Coroutine _DisplayCharText;
    private IEnumerator _BlinkNextText;
    private WaitForSeconds waitForTextCoroutine, waitForNextTextCoroutine;

    [HideInInspector]
    public GameObject npcTargetObject;
    [HideInInspector]
    public NPCScript npcTargetObjectScript;
    [HideInInspector]
    public NPCScript.NPCData npcTargetObjectData;

    
    //메세지 출력 관련
    //NPCData 클래스에서 다이얼로그 문자열이 인덱스 수 
    [HideInInspector]
    public int choiceCurrentLine=0 , nowStringLength = 0, nowIndex;
    //텍스트가 타이핑 되고 있는지
    [HideInInspector]
    public bool typingText, isChoice;
    [HideInInspector]
    private string nowString;
    [HideInInspector]
    public List<string> choiceDialogue, nowListString;

    

    public List<Image> listUI = new List<Image>();
    public List<Button> choiceButton;
    public List<TextMeshProUGUI> choiceButtonText;

    private bool isPause = false;


    private void Start()
    {
        waitForTextCoroutine = new WaitForSeconds(0.025f);
        waitForNextTextCoroutine = new WaitForSeconds(0.5f);
        inventoryUI = GetComponent<InventoryUI>();
        shopUI = GetComponent<ShopUI>();
        _BlinkNextText = BlinkNextText();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnPause()
    {
        if(Player_Equipment.instance.playerMovement.canMove)
        {
            OffAllUI();
            Pause();
            if (isPause)
            {
                OnUI(6);
                OnUI(11);
            }
            else if (!isPause)
            {
                OffUI(6);
                OffUI(11);
            }
        }
        else if (listUI[1].IsActive() || listUI[5].IsActive())
        {
            //대화창 열려있을떄 전부 닫기
            OffAllUI();
        }
        
    }
    public void Pause()
    {
        isPause = !isPause;
        if(isPause)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else if(!isPause)
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnUI(int num)
    {
        listUI[num].gameObject.SetActive(true);
        if (num == 1)
        {
            //npc 정보를 넘겨받음
            npcTargetObjectScript = npcTargetObject.GetComponent<NPCScript>();
            npcTargetObjectData = npcTargetObjectScript.selfData;
            npcTargetObjectScript.LookAtTransform(player.transform);
            
            ReceiveList(npcTargetObjectData.kname, npcTargetObjectData.dialogue);
            nowStringLength = npcTargetObjectData.dialogue.Count;
        }

        if(num == 1 || num == 5)
        {
            Player_Effect.instance.playerMovement.canMove = false;
            Player_Equipment.instance.playerAttack.canAttack = false;
        }
    }

    public void OffUI(int num)
    {
        if (listUI[num].IsActive())
        {
            listUI[num].gameObject.SetActive(false);
        }

        else if (!listUI[num].IsActive())
        {
            return;
        }
        //선택지를 위해 현재 UI종료
        if (num == 3)
        {
            nowListString = choiceDialogue;
            nowStringLength = nowListString.Count;
            nowIndex = 0;
            ReceiveList(null, nowListString);
            
        }

        if (num == 1 || num == 5)
        {
            Player_Effect.instance.playerMovement.canMove = true;
            Player_Equipment.instance.playerAttack.canAttack = true;
        }
    }

    public void OffAllUI()
    {
        if (isChoice)
        {
            foreach (var val in choiceButton)
            {
                val.gameObject.SetActive(false);
            }
        }
        //대화창이 열려있었을때만
        if (listUI[1].IsActive())
        {
            npcTargetObjectScript.ResetLook();
            StopCoroutine(_DisplayCharText);
            StopCoroutine(_BlinkNextText);
        }
        foreach (var val in listUI)
        {
            val.gameObject.SetActive(false);
        }

        nowIndex = 0;
        nowStringLength = 0;
        nowListString = null;
        typingText = false;
        isChoice = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inventoryUI.cameraManager.StopCamera(false);
        Player_Effect.instance.playerMovement.canMove = true;
        Player_Equipment.instance.playerAttack.canAttack = true;
    }

    public void SkipText()
    {
        ////타이핑이 되고 있을때 액션 키 눌러 스킵
         if (typingText == true)
         {
             StopCoroutine(_DisplayCharText);
             mainTextBox.text = nowString;
             typingText = false;
             StartCoroutine(_BlinkNextText);
         }
        
        //타이핑이 되고 있지 않을때 액션 키 눌러 스킵
        //else 
        else if (typingText == false)
        {
            nowIndex += 1;
            if (nowIndex == nowStringLength)
            {
                OffAllUI();
            }
            else
            {
                ReceiveList(npcTargetObjectData.kname, nowListString);
                nextText.SetActive(false);
                StopCoroutine(_BlinkNextText);
            }
            
        }
    }

    //코루틴의 다중처리를 방지하기위해 currentLine이라는 변수를 사용하여 List첨자로 사용
    public void ReceiveList(string kname, List<string> lst)
    {
        if (kname != null)
        {
            nameTextBox.text = kname;
        }
        if (nowListString == null) nowListString = lst;

        if (nowListString[nowIndex] != null)
        {
            if (nowListString[nowIndex].ToCharArray()[0].Equals('?') && isChoice)
            {
                ChoiceProcess(nowListString);
            }
            else
            {
                if (typingText == false) _DisplayCharText = StartCoroutine(DisplayCharText(nowListString[nowIndex]));

            }
        }
    }

    
    //문자열 타입의 스트링을 매개변수로 받아 한 글자씩 일정한 시간 간격으로 출력
    private IEnumerator DisplayCharText(string st) 
    {
        typingText = true;
        mainTextBox.text = "";
        nowString = st;
        foreach (var ch in st.ToCharArray())
        {
            mainTextBox.text += ch;
            yield return waitForTextCoroutine;
        }
        typingText = false;
        StartCoroutine(_BlinkNextText);
    }
    
    //타이핑이 되고 있지 않을때 즉, 텍스트가 전부 입력되었을때 다음 표시가 깜빡임
    public IEnumerator BlinkNextText()
    {
        if(_DisplayCharText != null)
            StopCoroutine(_DisplayCharText);
        
        
        while (typingText == false)
        {
            yield return waitForNextTextCoroutine;
            nextText.SetActive(!nextText.activeSelf);
        }
        
    }

    public IEnumerator BlinkObject(GameObject obj)
    {
        while (true)
        {
            yield return waitForNextTextCoroutine;
            obj.SetActive(!obj.activeSelf);
        }
    }

    public void ChoiceProcess(List<string> lst)
    {
        inventoryUI.cameraManager.StopCamera(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OnUI(3);
        string[] choice = lst[nowIndex].Split(',');
        //한개의 선택지당 가로 길이는 70f 고정, 세로 길이는 항목당 20f로 설정.
        listUI[3].rectTransform.sizeDelta = new Vector2(70f, (choice.Length-1) * 20f);
                
        //선택지 대화 중에서 구분하기 위한 맨앞 ?를 제외한 나머지 대사들을 반복문으로 버튼에게 전달
        for (int i = 1; i < choice.Length; i++)
        {
            choiceButton[i -1].gameObject.SetActive(true);
            choiceButtonText[i -1].text = choice[i];
        }

        choiceButton[0].Select();
    }
    
    public void DieForUI()
    {
        OffAllUI();
        if(inventoryUI.inventoryParent.activeSelf)
        {
            inventoryUI.OnInventory();
        }
        OnUI(6);
        Player_Equipment.instance.playerAttack.canAttack = false;
        Player_Equipment.instance.playerMovement.canMove = false;
        Player_Equipment.instance.playerMovement.playerCollider.enabled = false;
        Player_Equipment.instance.playerMovement.rigid.useGravity = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Player_Equipment.instance.playerAttack.anim.SetTrigger("Die");
        inventoryUI.cameraManager.StopCamera(true);
        StartCoroutine(FadeInImage(8));
        StartCoroutine(FadeInImage(9));
    }
    public IEnumerator FadeInImage(int i)
    {
        OnUI(i);
        listUI[i].color = new Color(listUI[i].color.r, listUI[i].color.g, listUI[i].color.b, 0);
        while (listUI[i].color.a < 1.0f)
        {
            listUI[i].color = new Color(listUI[i].color.r, listUI[i].color.g, listUI[i].color.b, listUI[i].color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FadeoutImage(i));
    }

    IEnumerator FadeoutImage(int i)
    {
        listUI[i].color = new Color(listUI[i].color.r, listUI[i].color.g, listUI[i].color.b, 1f);
        while (listUI[i].color.a > 0.0f)
        {
            listUI[i].color = new Color(listUI[i].color.r, listUI[i].color.g, listUI[i].color.b, listUI[i].color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
        OffUI(i);
        if (i == 8)
        {
            OnUI(10);
        }
    }
}
