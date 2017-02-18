using UnityEngine;
using System.Collections;

public class SpeedDronSearch : StateMachineBehaviour 
{
	public SpeedDron speedDronMB;
	private Vector3 movePosition;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		speedDronMB.animator.SetBool("isAlive", speedDronMB.alive);

		speedDronMB.animator.SetBool("ShootDistance", speedDronMB.CheckDistance());
		speedDronMB.RotateDron();

		if(!speedDronMB.waiting)
		{
			NextTarget();
			speedDronMB.waiting = true;
		}
		else
		{
			speedDronMB.Waiting();
		}
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

	private void NextTarget()
	{
		movePosition = speedDronMB.playerTR.position + new Vector3(Random.Range(-5,5), 0, Random.Range(-5,5));
		speedDronMB.agent.destination = movePosition;
	}
}
