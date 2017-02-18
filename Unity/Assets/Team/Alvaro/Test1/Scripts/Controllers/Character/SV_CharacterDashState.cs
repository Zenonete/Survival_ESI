using UnityEngine;
using System.Collections;

public class SV_CharacterDashState : SV_ICharacterState 
	
{
	
	private readonly SV_StatePatternCharacter character;
	
	
	public SV_CharacterDashState (SV_StatePatternCharacter statePatterncharacter)
	{
		character = statePatterncharacter;
	}
	
	public void UpdateState()
	{

	}

	public void FixedUpdateState()
	{
		Dash();
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
		//character.currentState = character.jumpState;
	}
	
	public void ToDashState()
	{
		//Debug.Log ("Can't transition to same state");
	}
	
	private void Dash()
	{
		character.counter -= Time.fixedDeltaTime;
		if(character.counter <= 0)
		{
			character.counter = 0;
			character.dash = false;
			ToMoveState();
			return;
		}

		Vector3 desiredMove = Vector3.zero;
		//CON CHARACTER CONTROLLER***********************************************************************************************************************************

		desiredMove = character.transform.forward;

		// get a normal for the surface that is being touched to move along it
		RaycastHit hitInfo;
		Physics.SphereCast(character.transform.position, character.curCharacterController.radius, Vector3.down, out hitInfo, character.curCharacterController.height/2f);
		//Debug.DrawLine(hitInfo.point, transform.position + hitInfo.normal, Color.red);

		desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
		//Debug.DrawLine(transform.position + new Vector3(0,-1,0), (transform.position + new Vector3(0,-1,0)) + desiredMove, Color.green);

		character.m_MoveDir.y = desiredMove.y*character.curData.dashPower;
		character.m_MoveDir.z = desiredMove.z*character.curData.dashPower;


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
	
}