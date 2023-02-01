using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCScript : MonoBehaviour
{
    /*
     NPC 추가하는 방법
     1. NPC 오브젝트를 만들고 이름을 번호_이름 방식으로 수정
     2. json에 해당 번호에 알맞게 id를 설정 후, 그 뒤에 이름과 다이얼로그를 만듬(이때 문법에 오류가 없게 주의!)
     
     3. (option) 움직이는 NPC일 경우 인스펙터 창에서 isMoveNpc를 지정해줄것.
     4. (option) 선택지가 있는 NPC일 경우 인스펙터 창에서 isChoice를 true로 해줄것
     */
    [HideInInspector]
    public NPCData selfData;
    private MovingNPC isMoveNpc;
    private Quaternion selfRotation;

    
    //바라볼 방향 쿼터니언 변수
    private Quaternion lookDirection;
    
    //플레이어를 쳐다볼것인가?, 선택지가 있는 NPC인지, 상점 NPC인지
    public bool isLookTarget, isChoice, isShop;
    //상점 npc라면 팔고 있는 아이템 목록
    public List<Item> sellItems;
    private int objectID;
    //삭제예정
    //private float turnLookSpeed = 0.01f;

    //시간 계산 중간 변수
    float deltaTime;
    
    //시간 계산을 위한 변수
    private float startTime;
    
    // 몇 초 안에 플레이어를 향해 회전할 것인지 시간
    private float totalTime = 0.5f;
    
    [System.Serializable]
    public class NPCData
    {
        public int id;
        public string name;
        public string kname;
        public List<string> dialogue;
        public List<List<string>> choiceDialogue;
        
        //json은 2차원 배열을 지원 안하기 때문에 임시로 저장할 변수
        public List<string> choice;
        public NPCData(NPCData aa)
        {
            id = aa.id;
            name = aa.name;
            kname = aa.kname;
            dialogue = aa.dialogue;
        }
        
    }

    [System.Serializable]
    public class NPCDataArray
    {
        public NPCData[] NPCArray;
    }
    void Start()
    {
        isMoveNpc = GetComponent<MovingNPC>();
        string jsonFilePath = "Json/NpcData";

        //NpcData.json 파일을 TextAsset 형식으로 전부다 불러옴
        var jsonTextFile = Resources.Load<TextAsset>(jsonFilePath);

        //json 파일을 문자열로 불러온 후 그것을 클래스 배열에 넣음
        NPCDataArray npcArray = JsonUtility.FromJson<NPCDataArray>(jsonTextFile.ToString());
        
        //NPC의 이름 형태는 숫자_이름 식으로 숫자부분은 고유의 id부분
        objectID = int.Parse(this.name.Split('_')[0]);
        
        selfData = new NPCData(npcArray.NPCArray[objectID]);

        if (isChoice)
        {
            selfData.choice = npcArray.NPCArray[objectID].choice;
            selfData.choiceDialogue = new List<List<string>>();
            
            
            arrayTo2DArray(selfData.choice);
        }
        else
        {
            selfData.choice = null;
            selfData.choiceDialogue = null;
        }
    }

    public void LookAtTransform(Transform target)
    {
        //플레이어를 쳐다보게 하고싶지 않을 경우
        if (!isLookTarget)
        {
            return;
        }
        
        //움직이는 npc일 경우 멈추게하고 nav를 일시정지한다
        if (isMoveNpc != null)
        {
            isMoveNpc.nav.speed = 0f;
            isMoveNpc.nav.velocity = new Vector3(0f,0f,0f);
            isMoveNpc.StopMove(true);
        }
        
        lookDirection = Quaternion.LookRotation(target.position - transform.position);
        lookDirection = Quaternion.Euler(transform.rotation.eulerAngles.x, lookDirection.eulerAngles.y, transform.rotation.eulerAngles.z);

        selfRotation = transform.rotation;
        StartCoroutine(RepeatLook(transform.rotation, lookDirection));
    }


    private IEnumerator RepeatLook(Quaternion from, Quaternion to)
    {
        startTime = Time.time;   
        do
        {
            deltaTime = Time.time - startTime;
            transform.rotation = Quaternion.Lerp(from, to, deltaTime / totalTime);
            yield return null;
        } while (deltaTime < totalTime);
    }

    //플레이어를 바라본 후 다시 방향을 초기화
    //DialogueUIManager에서 사용중
    public void ResetLook()
    {
        //플레이어를 쳐다보게 하고싶지 않을 경우
        if (!isLookTarget)
        {
            return;
        }
        if (isMoveNpc != null)
        {
            isMoveNpc.nav.speed = 1.5f;
            isMoveNpc.StopMove(false);
        }
        StartCoroutine(RepeatLook(transform.rotation, selfRotation));
    }

    public void arrayTo2DArray(List<string> lst)
    {
        List<string> temp = new List<string>();

        foreach (var st in lst)
        {
            if (!st.Equals(""))
            {
                temp.Add(st);
            }
            else
            {
                selfData.choiceDialogue.Add(temp);
                temp = new List<string>();
            }
        }
        selfData.choiceDialogue.Add(temp);
    }
    
}
