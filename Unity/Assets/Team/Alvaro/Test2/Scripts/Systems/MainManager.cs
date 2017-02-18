using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour 
{
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			GameObject.Find("DataManagement").GetComponent<DataManagement>().Save();
			Application.Quit();
		}
	}
}
