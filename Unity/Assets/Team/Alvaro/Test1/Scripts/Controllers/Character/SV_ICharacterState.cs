using UnityEngine;
using System.Collections;

public interface SV_ICharacterState
{
	
	void UpdateState();

	void FixedUpdateState();
	
	void OnTriggerEnter (Collider other);
	
	void ToMoveState();
	
	void ToJumpState();
	
	void ToDashState();
}
