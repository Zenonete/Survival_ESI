using UnityEngine;
using System.Collections;

public class DashChar : StateMachineBehaviour 
{
	public Char charMB;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		//Debug.Log("Estoy en dash");
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

	public void MoveCharacter()
	{
		Vector3 desiredMove = Vector3.zero;

		//dash
		desiredMove = charMB.transform.forward;

		// get a normal for the surface that is being touched to move along it
		RaycastHit hitInfo;
		Physics.SphereCast(charMB.transform.position, charMB.curCharacterController.radius, Vector3.down, out hitInfo, charMB.curCharacterController.height/2f);
		//Debug.DrawLine(hitInfo.point, transform.position + hitInfo.normal, Color.red);

		desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
		//Debug.DrawLine(transform.position + new Vector3(0,-1,0), (transform.position + new Vector3(0,-1,0)) + desiredMove, Color.green);

		charMB.m_MoveDir.x = desiredMove.x*charMB.curData.charSpeed;
		charMB.m_MoveDir.y = desiredMove.y*charMB.curData.charSpeed;
		charMB.m_MoveDir.z = desiredMove.z*charMB.curData.charSpeed;

		//prevent to enter in fall state in ground movement
		charMB.m_MoveDir.y = -charMB.m_StickToGroundForce;

		//movement
		charMB.curCollisionFlags = charMB.curCharacterController.Move(charMB.m_MoveDir*Time.fixedDeltaTime);

		//return of dash to other state
		charMB.dashTime -= Time.fixedDeltaTime;
		if(charMB.dashTime <= 0)
		{
			charMB.dashTime = 0;
			charMB.dash = false;
		}
	}
}
