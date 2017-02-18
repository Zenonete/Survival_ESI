using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;

[Serializable]
public class SV_CharData : ScriptableObject 
{
	#region Public Variables
	public float jumpSpeed = 10.0f;
	public float charSpeed = 5.0f;
	public float charMaxSpeed = 10.0f;
	public float maxSlope = 20f;
	public float gravity = -9.8f;
	public float dashTime = 0.5f;
	public float dashPower = 4;
	public float rotationSpeed = 0.25f;
	public bool shoot = false;

	public GameObject curWeaponPrefab;
	#endregion
	
	#region Private Variables
	#endregion
	
	#region Main Methods
	void OnEnable()
	{
		
	}
	#endregion
	
	#region Utility Methods
	#if UNITY_EDITOR
	public void OnEditorGUI()
	{
		jumpSpeed = EditorGUILayout.FloatField("Jump Speed: ", jumpSpeed);
		charSpeed = EditorGUILayout.FloatField("Speed: ", charSpeed);
		charMaxSpeed = EditorGUILayout.FloatField("Max Speed: ", charMaxSpeed);
		maxSlope = EditorGUILayout.FloatField("Max Slope: ", maxSlope);
		gravity = EditorGUILayout.FloatField("Gravity: ", gravity);
		dashTime = EditorGUILayout.FloatField("Dash Time: ", dashTime);
		dashPower = EditorGUILayout.FloatField("Dash Power: ", dashPower);
		rotationSpeed = EditorGUILayout.FloatField("Rotation Speed: ", rotationSpeed);
		shoot = (bool)EditorGUILayout.Toggle ("Shooting: ", shoot);
		curWeaponPrefab = (GameObject)EditorGUILayout.ObjectField("Current Weapon: ", curWeaponPrefab, typeof(GameObject), true);
	}
	#endif
	#endregion
}

