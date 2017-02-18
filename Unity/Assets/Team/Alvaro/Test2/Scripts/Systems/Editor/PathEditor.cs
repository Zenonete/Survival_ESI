using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor 
{

	void OnSceneGUI() 
	{
		Path points = target as Path;

		if(points.pathPoints.Length > 2)
		{
			Handles.color = Color.white;
			for(int i = 0; i < points.pathPoints.Length - 1; i++)
			{
				//Handles.DrawSolidDisc(points.pathPoints[i].position, Vector3.up, 0.25f);
				Handles.DrawLine(points.pathPoints[i].position, points.pathPoints[i + 1].position);
			}
		}
		else
		{
			Debug.LogWarning("Path need two or more points.");
		}
	}
}
