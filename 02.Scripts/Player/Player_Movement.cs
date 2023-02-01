using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour 
{
	public float turnSpeed = 10f, walkSpeed = 4f, runSpeed = 8f;
	private float speed, nowWalkSpeed, nowRunSpeed;

    [HideInInspector]
	public bool runCheck = false, canMove = true, freezing = false;
    [HideInInspector]
    public int slowCount;
    [HideInInspector]
    public Animator anim;
	private Camera mainCamera;
    [HideInInspector]
    public Rigidbody rigid;

    [HideInInspector]
    public CapsuleCollider playerCollider;

    private Vector3 targetDirection;
	private Vector2 input;
	private Quaternion freeRotation;
	public DialogueUIManager ui;

	public Transform Aim;
	private string walking = "Player_Walking", running = "Player_Running", freezeStart = "Player_FreezeStart", freezeEnd = "Player_FreezeEnd";
    [HideInInspector]
	public readonly int hashRun = Animator.StringToHash("Run"), hashWalk = Animator.StringToHash("Move"), hashMoveX = Animator.StringToHash("MoveX"), hashMoveY = Animator.StringToHash("MoveY");



    //WASD키로 이동 = InputManager
    void OnMove(InputValue val)
	{
		if (!ui.listUI[1].IsActive() && !ui.listUI[5].IsActive())
		{
			input = val.Get<Vector2>();
		}
	}

	public void OnRun()
	{
		//활을 쏘고 있지 않을 때
		if(!Player_Equipment.instance.playerAttack.anim.GetBool(Player_Equipment.instance.playerAttack.hashBowStartCharging)
			&& !Player_Equipment.instance.playerAttack.anim.GetBool(Player_Equipment.instance.playerAttack.hashBowChargingDone))
		{
            runCheck = !runCheck;
            anim.SetBool(hashRun, runCheck);
        }
		
	}

		

		// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		mainCamera = Camera.main;
		rigid = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
		nowWalkSpeed = walkSpeed;
		nowRunSpeed = runSpeed;
    }
			// Update is called once per frame
	void Update()
	{
		CheckForStamina();
		MoveSFX();

    }
	void FixedUpdate()
	{
		if(canMove)
		{
            if (Player_Equipment.instance.playerAttack.anim.GetBool(Player_Equipment.instance.playerAttack.hashBowStartCharging) || Player_Equipment.instance.playerAttack.anim.GetBool(Player_Equipment.instance.playerAttack.hashBowChargingDone))
            {
                ZoomingMoving();
            }
            else
            {
                Turn();
            }
        }
		
	}
	private void Turn()
	{
		//애니메이션 
		anim.SetFloat(hashWalk, input.magnitude);

		//월드 좌표계 기준으로 카메라의 x축 방향 벡터 값
		var forward = mainCamera.transform.TransformDirection(Vector3.forward);
		forward.y = 0;
		//월드 좌표계 기준으로 카메라의 y축 방향 벡터 값
		var right = mainCamera.transform.TransformDirection(Vector3.right);
		//인풋매니저의 x,y 값과 카메라의 x,y 좌표 값을 곱하여 플레이어가 바라봐야할 방향을 계산 
		targetDirection = input.x * right + input.y * forward;
		//magnitude : 벡터의 길이를 반환
		if (input != Vector2.zero && targetDirection.magnitude > 0.1f)
		{

			//바라봐야할 방향 벡터를 정규화함 ex) (5.2f, 0f, 2.2f) => (1f, 0f, 1f)
			targetDirection.Normalize();

			freeRotation = Quaternion.LookRotation(targetDirection, transform.up);

			
			float diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
			float eulerY = transform.eulerAngles.y;
			if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
			Vector3 euler = new Vector3(0, eulerY, 0);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler),turnSpeed * Time.deltaTime);
			speed = runCheck ? nowRunSpeed : nowWalkSpeed;
			Vector3 direction = rigid.rotation * Vector3.forward;
			rigid.MovePosition(rigid.position + direction * speed * Time.deltaTime);

		}
	}

    public void ZoomingMoving()
    {
		//Turn
		Quaternion lookRotation = Quaternion.identity;
        lookRotation.eulerAngles = new Vector3(0, mainCamera.transform.eulerAngles.y + 90f, 0);
		transform.rotation = lookRotation;

        //Move
        //월드 좌표계 기준으로 카메라의 x축 방향 벡터 값
        var forward = mainCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        //월드 좌표계 기준으로 카메라의 y축 방향 벡터 값
        var right = mainCamera.transform.TransformDirection(Vector3.right);

        targetDirection = input.x * right + input.y * forward;
        speed = runCheck ? nowRunSpeed : nowWalkSpeed;
        rigid.MovePosition(rigid.position + targetDirection * speed * Time.deltaTime);

		anim.SetFloat(hashMoveX, input.x);
		anim.SetFloat(hashMoveY, input.y);

    }

    void CheckForStamina()
	{
		if (runCheck && input != Vector2.zero && canMove)
		{
			if (Player_HP_Stamina.instance.stamina > 0)
			{
                //뛰는중
                Player_HP_Stamina.instance.DecreaseStamina();
			}
			else
			{
				//스테미나 모두 소모
				OnRun();
			}
		}
	}

	void MoveSFX()
	{
		if(input == Vector2.zero)
		{
			AudioManager.instance.SFXStop(walking);
			AudioManager.instance.SFXStop(running);
		}
		else
		{
            if (runCheck)
            {
                AudioManager.instance.SFXStop(walking);
                AudioManager.instance.SFXPlay(running);
            }
            else
            {
                AudioManager.instance.SFXStop(running);
                AudioManager.instance.SFXPlay(walking);
            }
        }

        
    }
		
	public void PlayerStop()
	{
		input = Vector2.zero;
		anim.SetFloat(hashWalk, 0);
		
	}

	public void SetSpeed(float walk, float run)
	{
		nowWalkSpeed = walk;
		nowRunSpeed = run;
	}

	public void Respawn()
	{
		anim.SetTrigger("Respawn");
		canMove = true;
        Player_Equipment.instance.playerMovement.playerCollider.enabled = true;
        Player_Equipment.instance.playerMovement.rigid.useGravity = true;

		ui.OffAllUI();
        //Player_Equipment.instance.playerAttack.inventoryUI.dialogueUIManager.Pause();
    }

	private void OnParticleCollision(GameObject other)
	{
		if(other.CompareTag("FREEZE") && !freezing)
		{
            StartCoroutine(FreezeMagicCoroutine(3f));
        }
		
	}

	IEnumerator FreezeMagicCoroutine(float freezeTime)
	{
		freezing = true;
        canMove = false;
        Player_Effect.instance.PlayEffect(Player_Effect.instance.freezePrison);
		AudioManager.instance.SFXPlay(freezeStart);
		anim.speed = 0.0f;

		yield return new WaitForSeconds(freezeTime);

		freezing = false;
        canMove = true;
        Player_Effect.instance.StopEffect(Player_Effect.instance.freezePrison);
        AudioManager.instance.SFXPlay(freezeEnd);
        anim.speed = 1.0f;
    }
}
