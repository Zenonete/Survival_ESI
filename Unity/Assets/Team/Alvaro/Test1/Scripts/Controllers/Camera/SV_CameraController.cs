using UnityEngine;
using System.Collections;

public class SV_CameraController : MonoBehaviour {

	#region Public Variables
	public bool allowMovement = false;
	public Camera curCamera;
	public SV_CameraData curData;
	
	public Transform camTarget;
	public float distance = 2;
	#endregion
	
	#region Private Variables
	private Vector3 wantedPosition = Vector3.zero;
	//private Vector3 lastTargetPosition = Vector3.zero;
	//private Vector3 curTargetVelocity = Vector3.zero;
	//private Vector3 origPosition = Vector3.zero;
	private Char playerScript;
	#endregion

	#region Main Methods
	// Use this for initialization
	void Start () 
	{
		//origPosition = transform.position;
		playerScript = camTarget.GetComponent<Char>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(curData != null)
		{
			if(allowMovement && camTarget)
			{
				if(!playerScript.isMortarMode)
				{
					//curTargetVelocity = camTarget.position - lastTargetPosition;
					//wantedPosition = (camTarget.position + (camTarget.forward * 3)) + new Vector3(0f, curData.heightFromTarget, curData.distanceFromTarget); //+ 
					wantedPosition = (camTarget.position + (camTarget.forward * 3)) + transform.TransformDirection(curCamera.transform.forward * -distance); //+ 
					//(new Vector3 (curTargetVelocity.x, 0f, 0f) * curData.velocityOffset);
				}
				else
				{
					Vector3 targetPos;

					float targetDistance = Vector3.Distance(camTarget.position, playerScript.mortarTargetGO.transform.position);
					float scope = playerScript.curWeaponPrefab.GetComponent<Weapon>().scope;
					if(targetDistance > scope)
					{
						wantedPosition = (camTarget.position + (camTarget.forward * 3)) + transform.TransformDirection(curCamera.transform.forward * -(distance + (scope/2)));
					}
					else
					{
						targetPos = (camTarget.position + playerScript.mortarTargetGO.transform.position)/2;
						wantedPosition = (targetPos + (camTarget.forward * 3)) + transform.TransformDirection(curCamera.transform.forward * - (distance + targetDistance));
					}


				}

				curCamera.transform.position = Vector3.Lerp(curCamera.transform.position, wantedPosition, Time.deltaTime * curData.cameraSpeed);
				//lastTargetPosition = camTarget.position;
			}
		}
	}
	#endregion
	
	#region Utility Methods
	void OnDrawGizmos()
	{
		
	}
	#endregion
}
