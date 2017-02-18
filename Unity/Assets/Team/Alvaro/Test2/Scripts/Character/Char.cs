using UnityEngine;
using System.Collections;

public class Char : FormOfLife 
{
	public SV_CharData curData;
	public SV_InputData inputData;
	//public SV_CameraData camData;

	public SV_CameraController curCamCtrl;

	public Animator animator;
	private IdleChar idleSmb;
	private MoveChar moveSmb;
	private JumpingChar jumpingSmb;
	private FallChar fallSmb;
	private DashChar dashSmb;
	private DeadChar deadSmb;

	public CharacterController curCharacterController;
	public CollisionFlags curCollisionFlags;
	public Vector3 m_MoveDir = Vector3.zero;
	public float m_StickToGroundForce = 1.01f;
	public float m_GravityMultiplier = 2;
	public bool m_Jumping = false;
	public bool m_PreviouslyGrounded;

	public float wantedHorizontalValue;
	public float wantedVerticalValue;

	public float wantedHorizontalRotValue;
	public float wantedVerticalRotValue;

	private Rigidbody curRB;

	public bool dash = false;
	public float dashTime = 0;
	public bool jump = false;
	public float currentJumpSpeed = 0;

	public GameObject curWeaponPrefab;
	public Transform weaponPosGO;
	[HideInInspector]
	public Weapon curWeapon;
	[HideInInspector]
	public bool isMortarMode = false;
	[HideInInspector]
	public GameObject mortarTargetGO;
	private float mortarTargetVelocity;

	[HideInInspector]
	public int level = 1;
	[HideInInspector]
	public int experience = 0;
	[HideInInspector]
	public int nextLevel = 100;

	void Awake () 
	{
		animator = GetComponent <Animator> ();
	}

	// Use this for initialization
	void Start () 
	{
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

		//animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
		//animator.SetIKPosition(AvatarIKGoal.RightHand, weaponPosGO.position);

		// Find a reference to the ExampleStateMachineBehaviour in Start since it might not exist yet in Awake.
		idleSmb = animator.GetBehaviour <IdleChar> ();
		moveSmb = animator.GetBehaviour <MoveChar> ();
		jumpingSmb = animator.GetBehaviour <JumpingChar> ();
		fallSmb = animator.GetBehaviour <FallChar> ();
		dashSmb = animator.GetBehaviour <DashChar> ();
		deadSmb = animator.GetBehaviour <DeadChar> ();

		// Set the StateMachineBehaviour's reference to an ExampleMonoBehaviour to this.
		idleSmb.charMB = this;
		moveSmb.charMB = this;
		jumpingSmb.charMB = this;
		fallSmb.charMB = this;
		dashSmb.charMB = this;
		deadSmb.charMB = this;

		if(inputData != null)
		{
			inputData.InitInputs();
		}

		curCamCtrl.allowMovement = true;

	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnAnimatorIK()
	{
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
		animator.SetIKPosition(AvatarIKGoal.RightHand, weaponPosGO.position);
	}

	public void GetInputData()
	{
		wantedHorizontalValue = inputData.horizontalValue;
		wantedVerticalValue = inputData.verticalValue;
		wantedHorizontalRotValue = inputData.horizontalRotValue;
		wantedVerticalRotValue = inputData.verticalRotValue;

		if(inputData.sidestep && dashTime == 0)
		{
			dash = true;
			dashTime = curData.dashTime;
		}
	}

	public void CheckJump()
	{
		// the jump state needs to read here to make sure it is not missed
		if (!jump && !m_Jumping)
		{
			jump = inputData.jump;
		}

		if (!m_PreviouslyGrounded && curCharacterController.isGrounded)
		{
			//m_MoveDir.y = 0f;
			m_Jumping = false;
		}
		/*if (!curCharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
		{
			m_MoveDir.y = 0f;
		}*/

		/*if (m_PreviouslyGrounded && !curCharacterController.isGrounded)
		{
			m_Jumping = true;
		}*/

		m_PreviouslyGrounded = curCharacterController.isGrounded;
	}

	public void RotateCharacter()
	{
		//if(!m_Jumping && !dash)
		//{
			//Rotating Player
		Vector3 turnDir;

		if(isMortarMode)
		{
			UpdateMortarTarget();
			turnDir = mortarTargetGO.transform.position - transform.position;
			turnDir.Normalize();
			DoRatation(turnDir);
		}
		else
		{
			turnDir = new Vector3(wantedHorizontalRotValue , 0f , -wantedVerticalRotValue);

			if (turnDir != Vector3.zero)
			{
				DoRatation(turnDir);
			}
			else
			{
				turnDir = new Vector3(wantedHorizontalValue , 0f , -wantedVerticalValue);

				if (turnDir != Vector3.zero)
				{
					DoRatation(turnDir);
				}
			}
		}	
		//}
	}

	private void DoRatation(Vector3 turnDir)
	{

		// Create a vector from the player to the point on the floor the raycast from the mouse hit.
		Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

		// Ensure the vector is entirely along the floor plane.
		playerToMouse.y = 0f;

		// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
		Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

		// Set the player's rotation to this new rotation.
		transform.rotation =  Quaternion.Slerp(curRB.rotation, newRotatation, curData.rotationSpeed);
	}

	private void UpdateMortarTarget()
	{
		mortarTargetGO.transform.position += new Vector3(wantedHorizontalRotValue , 0f , -wantedVerticalRotValue) * Time.deltaTime * mortarTargetVelocity;
	}

	public GameObject GenerateWeapon()
	{
		if(mortarTargetGO != null)
		{
			Destroy(mortarTargetGO);
		}

		GameObject go = Instantiate<GameObject>(curWeaponPrefab);
		go.transform.position = weaponPosGO.position;
		go.transform.rotation = weaponPosGO.rotation;
		go.transform.SetParent(this.transform);
		go.GetComponent<Weapon>().parentTag = gameObject.tag;

		curCamCtrl.distance = go.GetComponent<Weapon>().scope;

		isMortarMode = go.GetComponent<Weapon>().weaponType != Weapon.WeaponType.MORTAR ? false : true;

		if(isMortarMode)
		{
			mortarTargetVelocity = go.GetComponent<Weapon>().mortarTargetVelocity;
			mortarTargetGO = go.GetComponent<Weapon>().target;
			mortarTargetGO.transform.position = this.transform.position + (this.transform.forward * 2) + new Vector3(0,0.02f,0);
		}

		return go;
	}

	public void Shoot()
	{
		if(inputData.shoot)
		{
			curWeapon.ShootWeapon();
		}
		curWeapon.UpdateFireRate();
	}

	public void UpdateAnimatorParameters()
	{
		if(Mathf.Abs(wantedHorizontalValue) >= 0.01f || Mathf.Abs(wantedVerticalValue) >= 0.01f)
		{
			animator.SetBool("isMoving", true);
		}
		else
		{
			animator.SetBool("isMoving", false);
		}
		animator.SetBool("isJump", jump);
		animator.SetBool("isGrounded", m_PreviouslyGrounded);
		animator.SetBool("isDash", dash);
		animator.SetBool("isAlive", alive);
	}

	public void SetAttributes(bool up)
	{
		//Debug.Log(1+(int)Mathf.Floor(Mathf.Pow(nextLevel, 1/3)));

		//level = 50 * Mathf.Sqrt(nextLevel);

		if(up)
		{
			nextLevel += 50;
		}
		else
		{
			nextLevel = (int)(experience/50) + 1;
			//Debug.Log((nextLevel));
			nextLevel *= 50;
			//Debug.Log((nextLevel));
		}

		//Debug.Log((nextLevel/50)-1);
		level = (int)((nextLevel/50)-1);
		lifeMax = 100 + (5 * (level-1));
		if(life > 0)
		{
			life = lifeMax;
		}
	}

	public void AddExperience(int exp)
	{
		experience += exp;
		if(experience >= nextLevel)
		{
			SetAttributes(true);
		}
	}
}
//Lvl - NextExp - life
//1 - 100 - 100
//2 - 150 - 105
//3 - 200 - 110
//4 - 250 - 115
//5 - 300 - 120
//6 - 350 - 125
//7 - 400 - 130
//8 - 450 - 135
//9 - 500 - 140