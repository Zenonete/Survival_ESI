using UnityEngine;
using System.Collections;

public class SnakeDronMove : StateMachineBehaviour 
{
	public SnakeDron snakeDronMB;
	private Vector3 movePosition;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		snakeDronMB.animator.SetBool("isAlive", snakeDronMB.alive);

		if(!snakeDronMB.reloading)
		{
			snakeDronMB.Shoot();

			if(snakeDronMB.numberOfShoots <= 0)
			{
				snakeDronMB.reloading = true;
				snakeDronMB.numberOfShoots = snakeDronMB.maxNumberOfShoots;
			}
			else
			{
				snakeDronMB.numberOfShoots -= Time.deltaTime;
			}
		}
		else
		{
			snakeDronMB.UpdateShootRate();
		}

		snakeDronMB.UpdateFireRateWeapon();

		snakeDronMB.RotateDron();

		if(!snakeDronMB.agent.pathPending && snakeDronMB.agent.remainingDistance <= snakeDronMB.agent.stoppingDistance)
		{
			NextTarget();
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
		snakeDronMB.pathPoint++;
		if(snakeDronMB.pathPoint == snakeDronMB.dronPath.pathPoints.Length)
		{
			Destroy(snakeDronMB.gameObject);
			return;
		}
		movePosition = snakeDronMB.dronPath.pathPoints[snakeDronMB.pathPoint].position;
		snakeDronMB.agent.destination = movePosition;
	}
}
