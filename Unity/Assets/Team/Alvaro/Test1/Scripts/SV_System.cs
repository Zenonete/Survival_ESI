using UnityEngine;
using System.Collections;

public class SV_System : MonoBehaviour 
{

	#region Public Variables
	public static SV_System Instance;

	public bool allowPlay = false;
	public bool hasControllers = false;

	public SV_CameraData camData;
	public SV_CharData charData;
	public SV_InputData inputData;
	public SV_ArenaData arenaData;

	public SV_CameraController curCamCtrl;
	public SV_CharController curCharCtrl;
	//public SV_StatePatternCharacter curCharCtrl;

	public Camera wantedCamera;
	#endregion

	#region Private Variables
	#endregion
	
	#region Main Methods
	// Use this for initialization
	void Awake()
	{
		Instance = this;
	}

	void Start () 
	{
		hasControllers = true;
		if(inputData == null)
		{
			hasControllers = false;
		}

		if(curCamCtrl == null || camData == null)
		{
			hasControllers = false;
		}

		if(curCharCtrl == null || charData == null)
		{
			hasControllers = false;
		}

		if(arenaData == null)
		{
			hasControllers = false;
		}

		if(hasControllers)
		{
			inputData.InitInputs();
			curCharCtrl.curData = charData;
			curCamCtrl.curData = camData;
			curCharCtrl.curInput = inputData;
			if(wantedCamera)
			{
				curCamCtrl.curCamera = wantedCamera;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(allowPlay && hasControllers)
		{
			//Update Inputs
			inputData.UpdateInputs();

			//Update Character
			curCharCtrl.allowMovement = true;
			curCharCtrl.wantedHorizontalValue = inputData.horizontalValue;
			curCharCtrl.wantedVerticalValue = inputData.verticalValue;
			curCharCtrl.sidestep = inputData.sidestep;
			curCharCtrl.shoot = inputData.shoot;
			curCharCtrl.wantedHorizontalRotValue = inputData.horizontalRotValue;
			curCharCtrl.wantedVerticalRotValue = inputData.verticalRotValue;
			curCharCtrl.curData.gravity = arenaData.gravity;

			//Update Camera
			curCamCtrl.allowMovement = true;

		}
	}
	#endregion

	#region Utility Methods
	#endregion
}
