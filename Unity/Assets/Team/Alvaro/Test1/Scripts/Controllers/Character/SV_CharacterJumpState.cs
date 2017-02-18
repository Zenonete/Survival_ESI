using UnityEngine;
using System.Collections;

public class SV_CharacterJumpState : SV_ICharacterState 
	
{
	private readonly SV_StatePatternCharacter character;
	//private float searchTimer;
	
	public SV_CharacterJumpState (SV_StatePatternCharacter statePatterncharacter)
	{
		character = statePatterncharacter;
	}
	
	public void UpdateState()
	{

	}

	public void FixedUpdateState()
	{
		Jump();
	}

	public void OnTriggerEnter (Collider other)
	{
		
	}
	
	public void ToMoveState()
	{
		character.currentState = character.moveState;
	}
	
	public void ToJumpState()
	{
		//Debug.Log ("Can't transition to same state");
	}
	
	public void ToDashState()
	{

	}
	
	private void Jump()
	{
		if(character.dash)
		{
			ToDashState();
			return;
		}

		if (character.curCharacterController.isGrounded)
		{
			ToMoveState();
			return;
		}

		Vector3 desiredMove = Vector3.zero;
		//CON CHARACTER CONTROLLER***********************************************************************************************************************************

		/*Vector2 m_Input = new Vector2(character.wantedHorizontalValue, -character.wantedVerticalValue);

		// normalize input if it exceeds 1 in combined length:
		if (m_Input.sqrMagnitude > 1)
		{
			m_Input.Normalize();
		}

		// always move along the camera forward as it is the direction that it being aimed at
		desiredMove = new Vector3(m_Input.x, 0f, m_Input.y);*/

		//gravity and jump force
		character.m_MoveDir += Physics.gravity*character.m_GravityMultiplier*Time.fixedDeltaTime;

		//Debug.DrawLine(transform.position + new Vector3(0,-1,0), (transform.position + new Vector3(0,-1,0)) + m_MoveDir, Color.blue);
		character.curCollisionFlags = character.curCharacterController.Move(character.m_MoveDir*Time.fixedDeltaTime);
	}

}
