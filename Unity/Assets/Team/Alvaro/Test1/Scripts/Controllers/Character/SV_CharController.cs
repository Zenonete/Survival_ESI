using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SV_CharController : MonoBehaviour 
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
	#endregion
	
	#region Private Variables
	private CharacterController curCharacterController;
	private CollisionFlags curCollisionFlags;
	private Vector3 m_MoveDir = Vector3.zero;
	private float m_StickToGroundForce = 1.01f;
	private float m_GravityMultiplier = 2;
	private bool m_Jumping = false;
	private bool m_PreviouslyGrounded;

	private Rigidbody curRB;
	private Vector3 wantedJumpForce;
	private Vector3 wantedVelocity;
	private bool isJumping = false;
	private bool onAir = false;
	private float jumpForce = 0;

	private float counter = 0;
	private bool dash = false;

	private Vector3 normal = Vector3.up;

	private Weapon curWeapon;
	
	#endregion
	
	#region Main Methods
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
	}

	// Update is called once per frame
	private void Update()
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

	// Update is called once per frame
	void FixedUpdate () 
	{
		if(curData)
		{
			if(allowMovement)
			{
				CounterDash();

				Dash();

				MoveCharacter();

				RotateCharacter();

				if(shoot)
				{
					curWeapon.ShootWeapon();
				}
				curWeapon.UpdateFireRate();
			}
		}
	}
	#endregion
	
	#region Utility Methods
	void CounterDash()
	{
		if(dash)
		{
			counter -= Time.fixedDeltaTime;
			if(counter <= 0)
			{
				counter = 0;
				dash = false;
			}
		}
	}

	void Dash()
	{
		if(sidestep && counter == 0)
		{
			dash = true;
			counter = curData.dashTime;
		}
	}

	void MoveCharacter()
	{
		Vector3 desiredMove = Vector3.zero;
		//CON CHARACTER CONTROLLER***********************************************************************************************************************************
		if(!dash)
		{
			Vector2 m_Input = new Vector2(wantedHorizontalValue, -wantedVerticalValue);
			
			// normalize input if it exceeds 1 in combined length:
			if (m_Input.sqrMagnitude > 1)
			{
				m_Input.Normalize();
			}

			// always move along the camera forward as it is the direction that it being aimed at
			desiredMove = new Vector3(m_Input.x, 0f, m_Input.y);
		}
		else
		{
			desiredMove = transform.forward;
		}

		if(!m_Jumping)
		{
			// get a normal for the surface that is being touched to move along it
			RaycastHit hitInfo;
			Physics.SphereCast(transform.position, curCharacterController.radius, Vector3.down, out hitInfo, curCharacterController.height/2f);
			//Debug.DrawLine(hitInfo.point, transform.position + hitInfo.normal, Color.red);

			desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
			//Debug.DrawLine(transform.position + new Vector3(0,-1,0), (transform.position + new Vector3(0,-1,0)) + desiredMove, Color.green);

			if(!dash)
			{
				m_MoveDir.x = desiredMove.x*curData.charSpeed;
				m_MoveDir.y = desiredMove.y*curData.charSpeed;
				m_MoveDir.z = desiredMove.z*curData.charSpeed;
			}
			else
			{
				m_MoveDir.y = desiredMove.y*curData.dashPower;;
				m_MoveDir.z = desiredMove.z*curData.dashPower;
			}
		}

		//gravity and jump force
		if (curCharacterController.isGrounded)
		{
			m_MoveDir.y = -m_StickToGroundForce;
			
			if (jump && !dash)
			{
				m_MoveDir.y = curData.jumpSpeed;
				jump = false;
				m_Jumping = true;
			}
		}
		else
		{
			m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
		}

		//Debug.DrawLine(transform.position + new Vector3(0,-1,0), (transform.position + new Vector3(0,-1,0)) + m_MoveDir, Color.blue);
		curCollisionFlags = curCharacterController.Move(m_MoveDir*Time.fixedDeltaTime);
	}


	void RotateCharacter()
	{
		if(!m_Jumping && !dash)
		{
			//Rotating Player
			Vector3 turnDir = new Vector3(wantedHorizontalRotValue , 0f , -wantedVerticalRotValue);
			
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
	}

	void DoRatation(Vector3 turnDir)
	{
		// Create a vector from the player to the point on the floor the raycast from the mouse hit.
		Vector3 playerToMouse = (transform.position + turnDir) - transform.position;
		
		// Ensure the vector is entirely along the floor plane.
		playerToMouse.y = 0f;
		
		// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
		Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);
		
		// Set the player's rotation to this new rotation.
		curRB.MoveRotation(Quaternion.Slerp(curRB.rotation, newRotatation, curData.rotationSpeed));
	}

	void OnCollisionEnter (Collision collision)
	{
		if(onAir)
		{
			foreach (ContactPoint contact in collision.contacts) 
			{
				if (Vector3.Angle (contact.normal, Vector3.up) < curData.maxSlope) 
				{
					onAir = false;
					jumpForce = 0;
				}
			}
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

	void OnDrawGizmos()
	{
		
	}
	#endregion
}

