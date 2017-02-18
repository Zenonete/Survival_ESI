using UnityEngine;
using System.Collections;

public class SV_StatePatternCharacter : MonoBehaviour 
{
	#region Public Variables
	public bool allowMovement = false;
	
	public SV_CharData curData;
	public SV_InputData curInput;
	
	public float wantedHorizontalValue;
	public float wantedVerticalValue;
	
	public float wantedHorizontalRotValue;
	public float wantedVerticalRotValue;
	
	public bool jump = false;
	public bool sidestep = false;
	public bool shoot = false;
	
	public GameObject curWeaponPrefab;
	public Transform weaponPosGO;

	

	[HideInInspector] public SV_ICharacterState currentState;
	[HideInInspector] public SV_CharacterDashState dashState;
	[HideInInspector] public SV_CharacterJumpState jumpState;
	[HideInInspector] public SV_CharacterMoveState moveState;
	#endregion

	#region Private Variables
	public CharacterController curCharacterController;
	public CollisionFlags curCollisionFlags;
	public Vector3 m_MoveDir = Vector3.zero;
	public float m_StickToGroundForce = 1.01f;
	public float m_GravityMultiplier = 2;
	public bool m_Jumping = false;
	public bool m_PreviouslyGrounded;

	public Rigidbody curRB;
	public Vector3 wantedJumpForce;
	public Vector3 wantedVelocity;
	public bool isJumping = false;
	public bool onAir = false;
	public float jumpForce = 0;

	public float counter = 0;
	public bool dash = false;

	public Vector3 normal = Vector3.up;

	public Weapon curWeapon;

	#endregion


	private void Awake()
	{
		dashState = new SV_CharacterDashState (this);
		jumpState = new SV_CharacterJumpState (this);
		moveState = new SV_CharacterMoveState (this);
	}
	
	// Use this for initialization
	void Start () 
	{
		currentState = moveState;

		if(curCharacterController == null)
		{
			curCharacterController = GetComponent<CharacterController>();
		}

		if(curRB == null)
		{
			curRB = gameObject.GetComponent<Rigidbody>();
		}

		if(curWeapon == null)
		{
			GameObject go = GenerateWeapon();
			curWeapon = go.GetComponent<Weapon>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckGround();

		currentState.UpdateState ();
	}

	void FixedUpdate () 
	{
		Dash();

		currentState.FixedUpdateState ();
	}
	
	private void OnTriggerEnter(Collider other)
	{
		currentState.OnTriggerEnter (other);
	}

	private void CheckGround()
	{
		// the jump state needs to read here to make sure it is not missed
		if (!jump && !dash && !m_Jumping)
		{
			jump = curInput.jump;
		}

		if (!m_PreviouslyGrounded && curCharacterController.isGrounded)
		{
			m_MoveDir.y = 0f;
			m_Jumping = false;
		}
		if (!curCharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
		{
			m_MoveDir.y = 0f;
		}

		m_PreviouslyGrounded = curCharacterController.isGrounded;
	}

	void Dash()
	{
		if(sidestep && counter == 0)
		{
			dash = true;
			counter = curData.dashTime;
		}
	}

	GameObject GenerateWeapon()
	{
		GameObject go = Instantiate<GameObject>(curWeaponPrefab);
		go.transform.position = weaponPosGO.position;
		go.transform.rotation = weaponPosGO.rotation;
		go.transform.SetParent(this.transform);
		return go;
	}
}
