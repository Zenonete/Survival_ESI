using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour 
{

	public GameObject bullet;
	private GameObject bulletInstance;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			bulletInstance = GameObject.Instantiate<GameObject>(bullet);
			bulletInstance.transform.position = transform.position;
			bulletInstance.transform.rotation = transform.rotation;
		}
	}
}
