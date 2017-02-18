using UnityEngine;
using System.Collections;

public class MoveChar : StateMachineBehaviour 
{

	public Char charMB;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		//Debug.Log("Estoy Moviendome");
		charMB.inputData.UpdateInputs();
		charMB.GetInputData();
		charMB.CheckJump();
		charMB.RotateCharacter();
		charMB.Shoot();
		charMB.UpdateAnimatorParameters();
		MoveCharacter();
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	public void MoveCharacter()
	{
		Vector3 desiredMove = Vector3.zero;
		Vector2 m_Input = new Vector2(charMB.wantedHorizontalValue, -charMB.wantedVerticalValue);

		// normalize input if it exceeds 1 in combined length:
		if (m_Input.sqrMagnitude > 1)
		{
			m_Input.Normalize();
		}

		// always move along the camera forward as it is the direction that it being aimed at
		desiredMove = new Vector3(m_Input.x, 0f, m_Input.y);

		// get a normal for the surface that is being touched to move along it
		RaycastHit hitInfo;
		Physics.SphereCast(charMB.transform.position, charMB.curCharacterController.radius, Vector3.down, out hitInfo, charMB.curCharacterController.height/2f);
		//Debug.DrawLine(hitInfo.point, transform.position + hitInfo.normal, Color.red);

		//desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
		desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal);
		//Debug.DrawLine(transform.position + new Vector3(0,-1,0), (transform.position + new Vector3(0,-1,0)) + desiredMove, Color.green);
		//Debug.Log(desiredMove.magnitude);
		charMB.animator.SetFloat("Velocity", desiredMove.magnitude);

		charMB.m_MoveDir.x = desiredMove.x*charMB.curData.charSpeed;
		charMB.m_MoveDir.y = desiredMove.y*charMB.curData.charSpeed;
		charMB.m_MoveDir.z = desiredMove.z*charMB.curData.charSpeed;

		//prevent to enter in fall state in ground movement
		charMB.m_MoveDir.y = -charMB.m_StickToGroundForce;
		//to jump state
		if (charMB.jump && !charMB.m_Jumping)
		{
			charMB.currentJumpSpeed = charMB.curData.jumpSpeed;
			charMB.m_MoveDir.y = charMB.currentJumpSpeed;
			charMB.m_Jumping = true;
		}

		//movement
		charMB.curCollisionFlags = charMB.curCharacterController.Move(charMB.m_MoveDir*Time.fixedDeltaTime);
	}
}
