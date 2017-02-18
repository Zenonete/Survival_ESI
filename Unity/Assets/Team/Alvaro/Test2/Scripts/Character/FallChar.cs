using UnityEngine;
using System.Collections;

public class FallChar : StateMachineBehaviour 
{
	public Char charMB;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		charMB.dashTime = 0;
		charMB.dash = false;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		//Debug.Log("Estoy cayendo");
		charMB.inputData.UpdateInputs();
		charMB.GetInputData();
		charMB.CheckJump();
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

	void MoveCharacter()
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

		charMB.m_MoveDir.x = desiredMove.x*charMB.curData.charSpeed;
		charMB.m_MoveDir.z = desiredMove.z*charMB.curData.charSpeed;

		//gravity
		charMB.m_MoveDir += Physics.gravity*charMB.m_GravityMultiplier*Time.fixedDeltaTime;
		charMB.curCollisionFlags = charMB.curCharacterController.Move(charMB.m_MoveDir*Time.fixedDeltaTime);
	}
}
