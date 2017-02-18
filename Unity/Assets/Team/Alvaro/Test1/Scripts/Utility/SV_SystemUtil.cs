using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public static class SV_SystemUtils
{
	#if UNITY_EDITOR
	public static void CreateBaseSystem()
	{
		Debug.Log ("Creating base system...");
		
		SV_System[] systemGO = (SV_System[])GameObject.FindObjectsOfType(typeof(SV_System));
		if(systemGO.Length > 0)
		{
			EditorUtility.DisplayDialog("Survivalr System", "Base System already exits", "OK");
			return;
		}
		
		GameObject curGO = new GameObject("SV_System");
		if(curGO)
		{
			curGO.transform.position = Vector3.zero;
			SV_System curSystem = curGO.AddComponent<SV_System>();
			if(curSystem != null)
			{
				//Create Camera Asset
				CreateCameraData(curSystem);
				CreateCharacterData(curSystem);
				CreateInputData(curSystem);
				CreateArenaData(curSystem);
				
				//Create Camera Controller
				GameObject curCamGO = new GameObject("SV_CameraCtrl");
				if(curCamGO)
				{
					curCamGO.transform.parent = curGO.transform;
					SV_CameraController curCamCtrl = curCamGO.AddComponent<SV_CameraController>();
					if(curCamCtrl != null)
					{
						curSystem.curCamCtrl = curCamCtrl;
					}
				}
				
				//Create Character Controller
				GameObject curCharGO = new GameObject("SV_CharCtrl");
				if(curCharGO)
				{
					curCharGO.transform.parent = curGO.transform;

					Rigidbody curCharRigidbody = curCharGO.AddComponent<Rigidbody>();
					curCharRigidbody.mass = 1;
					curCharRigidbody.drag = Mathf.Infinity;
					curCharRigidbody.angularDrag = Mathf.Infinity;
					curCharRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
					curCharRigidbody.useGravity = false;

					SV_CharController curCharCtrl = curCharGO.AddComponent<SV_CharController>();
					//SV_StatePatternCharacter curCharCtrl = curCharGO.AddComponent<SV_StatePatternCharacter>();
					if(curCharCtrl != null)
					{
						curSystem.curCharCtrl = curCharCtrl;
					}
				}
				
				Selection.activeObject = curGO;
			}
		}
	}
	
	public static void CreateCameraData(SV_System curSystem)
	{
		//Check to see if data already exists
		SV_CameraData existingData = (SV_CameraData)AssetDatabase.LoadAssetAtPath ("Assets/Team/Alvaro/Database/CameraData.asset", typeof(SV_CameraData));
		if(existingData != null)
		{
			curSystem.camData = existingData;
			return;
		}
		
		//Create new data system
		SV_CameraData curData = (SV_CameraData)ScriptableObject.CreateInstance(typeof(SV_CameraData));
		if(curData != null)
		{
			AssetDatabase.CreateAsset(curData, "Assets/Team/Alvaro/Database/CameraData.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			
			if(curSystem != null)
			{
				curSystem.camData = curData;
			}
		}
		else
		{
			EditorUtility.DisplayDialog("Survival System", "Cannot create camera data", "OK");
		}
	}
	
	public static void CreateCharacterData(SV_System curSystem)
	{
		//Check to see if data already exists
		SV_CharData existingData = (SV_CharData)AssetDatabase.LoadAssetAtPath ("Assets/Team/Alvaro/Database/CharData.asset", typeof(SV_CharData));
		if(existingData != null)
		{
			curSystem.charData = existingData;
			return;
		}
		
		//Create new data system
		SV_CharData curData = (SV_CharData)ScriptableObject.CreateInstance(typeof(SV_CharData));
		if(curData != null)
		{
			AssetDatabase.CreateAsset(curData, "Assets/Team/Alvaro/Database/CharData.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			
			if(curSystem != null)
			{
				curSystem.charData = curData;
			}
		}
		else
		{
			EditorUtility.DisplayDialog("Survival System", "Cannot create character data", "OK");
		}
	}
	
	public static void CreateInputData(SV_System curSystem)
	{
		//Check to see if data already exists
		SV_InputData existingData = (SV_InputData)AssetDatabase.LoadAssetAtPath ("Assets/Team/Alvaro/Database/InputData.asset", typeof(SV_InputData));
		if(existingData != null)
		{
			curSystem.inputData = existingData;
			return;
		}
		
		//Create new data system
		SV_InputData curData = (SV_InputData)ScriptableObject.CreateInstance(typeof(SV_InputData));
		if(curData != null)
		{
			AssetDatabase.CreateAsset(curData, "Assets/Team/Alvaro/Database/InputData.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			
			if(curSystem != null)
			{
				curSystem.inputData = curData;
			}
		}
		else
		{
			EditorUtility.DisplayDialog("Survival System", "Cannot create input data", "OK");
		}
	}

	public static void CreateArenaData(SV_System curSystem)
	{
		//Check to see if data already exists
		SV_ArenaData existingData = (SV_ArenaData)AssetDatabase.LoadAssetAtPath ("Assets/Team/Alvaro/Database/ArenaData.asset", typeof(SV_ArenaData));
		if(existingData != null)
		{
			curSystem.arenaData = existingData;
			return;
		}

		//Create new data system
		SV_ArenaData curData = (SV_ArenaData)ScriptableObject.CreateInstance(typeof(SV_ArenaData));
		if(curData != null)
		{
			AssetDatabase.CreateAsset(curData, "Assets/Team/Alvaro/Database/ArenaData.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			
			if(curSystem != null)
			{
				curSystem.arenaData = curData;
			}
		}
		else
		{
			EditorUtility.DisplayDialog("Survival System", "Cannot create arena data", "OK");
		}
	}
	#endif
}

