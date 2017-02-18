using UnityEngine;
using UnityEditor;
using System.Collections;

public class SV_ToolWindow : EditorWindow 
{
	#region Variables
	static SV_ToolWindow curWindow;
	static int guiID = 0;
	static GUIContent content;
	#endregion
	
	#region Main Methods
	[MenuItem("Survival/Create Base System")]
	static void InitSideScrollerWindow()
	{
		curWindow = (SV_ToolWindow)EditorWindow.GetWindow(typeof(SV_ToolWindow));
	}
	
	public static void InitSideScrollerWindow(int wantedGUI)
	{
		curWindow = (SV_ToolWindow)EditorWindow.GetWindow(typeof(SV_ToolWindow));
		content = new GUIContent();
		content.text = "Survival";
		curWindow.titleContent = content;
		guiID = wantedGUI;
	}
	
	void OnGUI()
	{
		switch(guiID)
		{
		case 0:
			DrawDefaultGUI();
			break;
			
		case 1:
			DrawCustomGUI();
			break;
			
		default:
			break;
		}
	}
	#endregion
	
	#region Utility Methods
	void DrawDefaultGUI()
	{
		if(GUILayout.Button("Create Base System", GUILayout.Height(40)))
		{
			SV_SystemUtils.CreateBaseSystem();
		}
	}
	
	void DrawCustomGUI()
	{
		
	}
	#endregion
}
