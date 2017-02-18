using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;

[Serializable]
public class SV_ArenaData : ScriptableObject 
{
	#region Public Variables
	public float gravity = -9.8f;
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
		gravity = EditorGUILayout.FloatField("Gravity: ", gravity);
	}
	#endif
	#endregion
}
