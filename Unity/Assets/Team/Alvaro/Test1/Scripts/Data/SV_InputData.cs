using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[Serializable]
public class SV_InputData : ScriptableObject 
{
	#region Public Variables
	public bool useMouse = false;
	public bool useKeyboard = false;
	public bool useTouchScreen = false;
	
	public bool mouseMoving = false;
	public MouseButton leftMouseBtn;
	public MouseButton middleMouseBtn;
	public MouseButton rightMouseBtn;
	public Vector2 mousePos;
	public Vector2 mouseDelta;
	
	public KeyCode leftKey;
	public KeyCode rightKey;
	public KeyCode upKey;
	public KeyCode downKey;
	public KeyCode sidestepKey;
	public KeyCode jumpKey;
	public float horizontalValue;
	public float verticalValue;

	public KeyCode leftRotKey;
	public KeyCode rightRotKey;
	public KeyCode upRotKey;
	public KeyCode downRotKey;
	public float horizontalRotValue;
	public float verticalRotValue;

	public string joyLXAxisName = "Horizontal";
	public string joyLYAxisName = "Vertical";
	public string joyRXAxisName = "Horizontal2";
	public string joyRYAxisName = "Vertical2";

	public float keySensitivity;
	public bool shoot;
	public bool jump;
	public bool sidestep;

	#endregion
	
	#region Private Variables
	private Vector2 lastMousePos;
	private SV_Touchpad lPad, rPad;
	#endregion
	
	#region Structs
	public struct MouseButton
	{
		public bool buttonDown;
		public bool buttonHeld;
		public bool buttonUp;
	}
	#endregion
	
	#region Main Methods
	void OnEnable()
	{
		shoot = false;
		jump = false;
	}
	
	public void InitInputs()
	{
		leftMouseBtn = new MouseButton();
		middleMouseBtn = new MouseButton();
		rightMouseBtn = new MouseButton();
		
		lastMousePos = Vector2.zero;

		shoot = false;
		jump = false;
		//keySensitivity = 80.0f;

		lPad = GameObject.Find("Image_LJ").GetComponent<SV_Touchpad>();
		rPad = GameObject.Find("Image_RJ").GetComponent<SV_Touchpad>();
		//Debug.Log(lPad.name);
	}
	
	public void UpdateInputs()
	{
		if(useTouchScreen)
		{
			ProcessTouchScreen();
		}

		if(useMouse)
		{
			ProcessMouse();
		}
		
		if(useKeyboard)
		{
			ProcessKeyboard();
		}
	}
	#endregion
	
	#region Utility Methods
	void ProcessTouchScreen()
	{
		if(lPad.tap || rPad.tap)
		{
			jump = true;
		}
		else
		{
			jump = false;
		}

		if(lPad.swipe || rPad.swipe)
		{
			sidestep = true;
		}
		else
		{
			sidestep = false;
		}

		Vector2 L = lPad.GetDirection();
		Vector2 R = rPad.GetDirection();
		horizontalValue = L.x;
		verticalValue = -L.y;
		horizontalRotValue = R.x;
		verticalRotValue = -R.y;
		if(horizontalRotValue != 0 || verticalRotValue != 0)
		{
			shoot = true;
		}
		else
		{
			shoot = false;
		}
	}

	void ProcessMouse()
	{
		//Debug.Log ("Updating Mouse...");
		leftMouseBtn.buttonDown = Input.GetMouseButtonDown(0);
		leftMouseBtn.buttonHeld = Input.GetMouseButton(0);
		leftMouseBtn.buttonUp = Input.GetMouseButtonUp(0);
		
		middleMouseBtn.buttonDown = Input.GetMouseButtonDown(2);
		middleMouseBtn.buttonHeld = Input.GetMouseButton(2);
		middleMouseBtn.buttonUp = Input.GetMouseButtonUp(2);
		
		rightMouseBtn.buttonDown = Input.GetMouseButtonDown(1);
		rightMouseBtn.buttonHeld = Input.GetMouseButton(1);
		rightMouseBtn.buttonUp = Input.GetMouseButtonUp(1);
		
		mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		if(mousePos != lastMousePos)
		{
			mouseMoving = true;
			mouseDelta = lastMousePos - mousePos;
		}
		else
		{
			mouseMoving = false;
			mouseDelta = Vector2.zero;
		}
		
		lastMousePos = mousePos;
	}
	
	void ProcessKeyboard()
	{
		//Debug.Log ("Updating Keyboard...");
		sidestep = Input.GetKeyDown(sidestepKey);

		jump = Input.GetKeyDown(jumpKey);
		
		if(Input.GetKey(leftKey) && !Input.GetKey(rightKey))
		{
			horizontalValue = Mathf.Lerp(horizontalValue, -1f, Time.deltaTime * keySensitivity);
		}
		else if(!Input.GetKey(leftKey) && Input.GetKey(rightKey))
		{
			horizontalValue = Mathf.Lerp(horizontalValue, 1f, Time.deltaTime * keySensitivity);
		}
		else if(!Input.GetKey(leftKey) && !Input.GetKey(rightKey))
		{
			horizontalValue = Mathf.Lerp(horizontalValue, 0f, Time.deltaTime * keySensitivity);
		}
		else
		{
			horizontalValue = Mathf.Lerp(horizontalValue, 0f, Time.deltaTime * keySensitivity);
		}
		
		if(horizontalValue < 0.001f && horizontalValue > -0.001f)
		{
			horizontalValue = 0.0f;
		}

		if(Input.GetKey(upKey) && !Input.GetKey(downKey))
		{
			verticalValue = Mathf.Lerp(verticalValue, -1f, Time.deltaTime * keySensitivity);
		}
		else if(!Input.GetKey(upKey) && Input.GetKey(downKey))
		{
			verticalValue = Mathf.Lerp(verticalValue, 1f, Time.deltaTime * keySensitivity);
		}
		else if(!Input.GetKey(upKey) && !Input.GetKey(downKey))
		{
			verticalValue = Mathf.Lerp(verticalValue, 0f, Time.deltaTime * keySensitivity);
		}
		else
		{
			verticalValue = Mathf.Lerp(verticalValue, 0f, Time.deltaTime * keySensitivity);
		}
		
		if(verticalValue < 0.001f && verticalValue > -0.001f)
		{
			verticalValue = 0.0f;
		}

		//Rotation keys
		if(Input.GetKey(leftRotKey) && !Input.GetKey(rightRotKey))
		{
			horizontalRotValue = Mathf.Lerp(horizontalRotValue, -1f, Time.deltaTime * keySensitivity);
			shoot = true;
		}
		else if(!Input.GetKey(leftRotKey) && Input.GetKey(rightRotKey))
		{
			horizontalRotValue = Mathf.Lerp(horizontalRotValue, 1f, Time.deltaTime * keySensitivity);
			shoot = true;
		}
		else if(!Input.GetKey(leftRotKey) && !Input.GetKey(rightRotKey))
		{
			horizontalRotValue = Mathf.Lerp(horizontalRotValue, 0f, Time.deltaTime * keySensitivity);
			shoot = true;
		}
		else
		{
			horizontalRotValue = Mathf.Lerp(horizontalRotValue, 0f, Time.deltaTime * keySensitivity);
			shoot = true;
		}
		
		if(horizontalRotValue < 0.001f && horizontalRotValue > -0.001f)
		{
			horizontalRotValue = 0.0f;
		}
		
		if(Input.GetKey(upRotKey) && !Input.GetKey(downRotKey))
		{
			verticalRotValue = Mathf.Lerp(verticalRotValue, -1f, Time.deltaTime * keySensitivity);
			shoot = true;
		}
		else if(!Input.GetKey(upRotKey) && Input.GetKey(downRotKey))
		{
			verticalRotValue = Mathf.Lerp(verticalRotValue, 1f, Time.deltaTime * keySensitivity);
			shoot = true;
		}
		else if(!Input.GetKey(upRotKey) && !Input.GetKey(downRotKey))
		{
			verticalRotValue = Mathf.Lerp(verticalRotValue, 0f, Time.deltaTime * keySensitivity);
			shoot = true;
		}
		else
		{
			verticalRotValue = Mathf.Lerp(verticalRotValue, 0f, Time.deltaTime * keySensitivity);
			shoot = true;
		}
		
		if(verticalRotValue < 0.001f && verticalRotValue > -0.001f)
		{
			verticalRotValue = 0.0f;
		}

		if(!Input.GetKey(leftRotKey) && !Input.GetKey(rightRotKey) && !Input.GetKey(upRotKey) && !Input.GetKey(downRotKey))
		{
			shoot = false;
		}
	}
	
	#if UNITY_EDITOR
	public void OnEditorGUI()
	{
		useMouse = EditorGUILayout.Toggle("Use Mouse: ", useMouse);
		useKeyboard = EditorGUILayout.Toggle("Use Keyboard: ", useKeyboard);
		
		GUILayout.Space(5);
		leftKey = (KeyCode)EditorGUILayout.EnumPopup ("Left Key: ", leftKey);
		rightKey = (KeyCode)EditorGUILayout.EnumPopup ("Right Key: ", rightKey);
		upKey = (KeyCode)EditorGUILayout.EnumPopup ("Up Key: ", upKey);
		downKey = (KeyCode)EditorGUILayout.EnumPopup ("Down Key: ", downKey);
		sidestepKey = (KeyCode)EditorGUILayout.EnumPopup ("Change Weapon Key: ", sidestepKey);
		jumpKey = (KeyCode)EditorGUILayout.EnumPopup ("Jump Key: ", jumpKey);
		keySensitivity = EditorGUILayout.FloatField("Key Sensitivity: ", keySensitivity);
		
		GUILayout.Space(5);
		EditorGUILayout.Slider ("Horizontal Value: ", horizontalValue, -1f, 1f);
		EditorGUILayout.Slider ("Vertical Value: ", verticalValue, -1f, 1f);
		shoot = (bool)EditorGUILayout.Toggle ("Shooting: ", shoot);
		jump = (bool)EditorGUILayout.Toggle ("Jumping: ", jump);
		sidestep = (bool)EditorGUILayout.Toggle ("Sidestep: ", sidestep);

		//GUILayout.Space(5);
		//L_Pad = (SV_Touchpad)EditorGUILayout.ObjectField("L_Pad", L_Pad, typeof(SV_Touchpad), true);
		//R_Pad = (SV_Touchpad)EditorGUILayout.ObjectField("R_Pad", R_Pad, typeof(SV_Touchpad), true);
	}
	#endif
	#endregion
}
