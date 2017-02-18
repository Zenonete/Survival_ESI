using UnityEngine;
using System.Collections;

public class SV_CharacterMoveState : SV_ICharacterState 
	
{
	private readonly SV_StatePatternCharacter character;
	
	public SV_CharacterMoveState (SV_StatePatternCharacter statePatterncharacter)
	{
		character = statePatterncharacter;
	}
	
	public void UpdateState()
	{

	}

	public void FixedUpdateState()
	{
		if(character.curData)
		{
			if(character.allowMovement)
			{
				MoveCharacter();
				
				RotateCharacter();
			}
		}
	}

	public void OnTriggerEnter (Collider other)
	{
		/*if (other.gameObject.CompareTag ("Player"))
			ToJumpState ();*/
	}
	
	public void ToMoveState()
	{
		//Debug.Log ("Can't transition to same state");
	}
	
	public void ToJumpState()
	{
		character.currentState = character.jumpState;
	}
	
	public void ToDashState()
	{
		character.currentState = character.dashState;
	}

	void MoveCharacter()
	{
		if(character.dash)
		{
			ToDashState();
			return;
		}

		if(character.jump)
		{
			ToJumpState();
			return;
		}

		Vector3 desiredMove = Vector3.zero;
		//CON CHARACTER CONTROLLER***********************************************************************************************************************************
		Vector2 m_Input = new Vector2(character.wantedHorizontalValue, -character.wantedVerticalValue);

		// normalize input if it exceeds 1 in combined length:
		if (m_Input.sqrMagnitude > 1)
		{
			m_Input.Normalize();
		}

		// always move along the camera forward as it is the direction that it being aimed at
		desiredMove = new Vector3(m_Input.x, 0f, m_Input.y);

		// get a normal for the surface that is being touched to move along it
		RaycastHit hitInfo;
		Physics.SphereCast(character.transform.position, character.curCharacterController.radius, Vector3.down, out hitInfo, character.curCharacterController.height/2f);
		//Debug.DrawLine(hitInfo.point, transform.position + hitInfo.normal, Color.red);

		desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
		//Debug.DrawLine(transform.position + new Vector3(0,-1,0), (transform.position + new Vector3(0,-1,0)) + desiredMove, Color.green);

		character.m_MoveDir.x = desiredMove.x*character.curData.charSpeed;
		character.m_MoveDir.y = desiredMove.y*character.curData.charSpeed;
		character.m_MoveDir.z = desiredMove.z*character.curData.charSpeed;

		//gravity and jump force
		if (character.curCharacterController.isGrounded)
		{
			character.m_MoveDir.y = -character.m_StickToGroundForce;
		}
		else
		{
			character.m_MoveDir += Physics.gravity*character.m_GravityMultiplier*Time.fixedDeltaTime;
		}

		//Debug.DrawLine(transform.position + new Vector3(0,-1,0), (transform.position + new Vector3(0,-1,0)) + m_MoveDir, Color.blue);
		character.curCollisionFlags = character.curCharacterController.Move(character.m_MoveDir*Time.fixedDeltaTime);
	}


	void RotateCharacter()
	{
		//Rotating Player
		Vector3 turnDir = new Vector3(character.wantedHorizontalRotValue , 0f , -character.wantedVerticalRotValue);

		if (turnDir != Vector3.zero)
		{
			DoRatation(turnDir);
		}
		else
		{
			turnDir = new Vector3(character.wantedHorizontalValue , 0f , -character.wantedVerticalValue);

			if (turnDir != Vector3.zero)
			{
				DoRatation(turnDir);
			}
		}
	}

	void DoRatation(Vector3 turnDir)
	{
		// Create a vector from the player to the point on the floor the raycast from the mouse hit.
		Vector3 playerToMouse = (character.transform.position + turnDir) - character.transform.position;

		// Ensure the vector is entirely along the floor plane.
		playerToMouse.y = 0f;

		// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
		Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

		// Set the player's rotation to this new rotation.
		character.curRB.MoveRotation(Quaternion.Slerp(character.curRB.rotation, newRotatation, character.curData.rotationSpeed));
	}
		
}
