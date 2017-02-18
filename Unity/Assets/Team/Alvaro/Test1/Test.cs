using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Translate(Vector3.down * Time.deltaTime);
	}

	void OnTriggerEnter(Collider c)
	{
		Debug.Log ("Pene");
	}
}
