using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SV_System))]
public class SV_SystemEditor : Editor 
{
	#region Variables
	SV_System targetSystem;
	#endregion

	#region Main Methods
	void OnEnable()
	{
		targetSystem = (SV_System)target;
	}

	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector();
		GUILayout.BeginVertical();
		
		GUILayout.Space(10);
		
		EditorGUILayout.LabelField("System Properties: ", EditorStyles.boldLabel);
		targetSystem.allowPlay = EditorGUILayout.Toggle("Allow Play", targetSystem.allowPlay);
		targetSystem.wantedCamera = (Camera)EditorGUILayout.ObjectField("System Camera: ", targetSystem.wantedCamera, typeof(Camera), true);
		
		GUILayout.Space(10);
		
		EditorGUILayout.LabelField("Arena Properties: ", EditorStyles.boldLabel);
		if(targetSystem.arenaData != null)
		{
			targetSystem.arenaData.OnEditorGUI();
		}
		
		GUILayout.Space(10);
		
		EditorGUILayout.LabelField("Character Properties: ", EditorStyles.boldLabel);
		if(targetSystem.charData != null)
		{
			targetSystem.charData.OnEditorGUI();
		}
		
		GUILayout.Space(10);
		
		EditorGUILayout.LabelField("Camera Properties: ", EditorStyles.boldLabel);
		if(targetSystem.camData != null)
		{
			targetSystem.camData.OnEditorGUI();
		}
		
		GUILayout.Space(10);
		
		EditorGUILayout.LabelField("Input Properties: ", EditorStyles.boldLabel);
		if(targetSystem.inputData != null)
		{
			targetSystem.inputData.OnEditorGUI();
		}
		
		GUILayout.EndVertical();
		
		
		if(GUI.changed)
		{
			EditorUtility.SetDirty(targetSystem);
		}
		
		Repaint();
	}
	#endregion
	
	#region Utility Methods
	#endregion
}
