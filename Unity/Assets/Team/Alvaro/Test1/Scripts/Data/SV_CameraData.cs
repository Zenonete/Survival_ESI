using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;

[Serializable]
public class SV_CameraData : ScriptableObject 
{
	#region Public Variables
	public float heightFromTarget = 5.0f;
	public float distanceFromTarget = -10.0f;
	public float velocityOffset = 2.0f;
	public float cameraSpeed = 3.0f;
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
		heightFromTarget = EditorGUILayout.FloatField("Height Offset: ", heightFromTarget);
		distanceFromTarget = EditorGUILayout.FloatField("Distance Offset: ", distanceFromTarget);
		velocityOffset = EditorGUILayout.FloatField("Velocity Offset: ", velocityOffset);
		cameraSpeed = EditorGUILayout.FloatField("Camera Speed: ", cameraSpeed);
	}
	#endif
	#endregion
}
