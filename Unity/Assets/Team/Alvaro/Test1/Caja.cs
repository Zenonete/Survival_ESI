using UnityEngine;
using System.Collections;

public class Caja : MonoBehaviour {

	Vector3 normal;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(Input.GetKey(KeyCode.T))
		{
			RaycastHit hit;
			Ray ray = new Ray(transform.position, Vector3.down);
			if (Physics.Raycast(ray, out hit)) {
				normal = hit.normal;
			}

			if(Vector3.Angle(Vector3.up, normal) > 40)
			{
				normal = Vector3.up;
			}

			//GetComponent<ConstantForce>().relativeForce = new Vector3(20, 0, 0);
			if(normal != Vector3.up)
			{
				GetComponent<Rigidbody>().velocity = Vector3.ProjectOnPlane(new Vector3(0, -9, 20), normal);
			}
			else
			{
				GetComponent<Rigidbody>().velocity = new Vector3(0,-9, 20);
			}
			//GetComponent<Rigidbody>().drag = 0f;
		}
		else
		{
			//GetComponent<ConstantForce>().relativeForce = new Vector3(0, 0, 0);
			GetComponent<Rigidbody>().velocity = new Vector3(0,-9, 0);
			//GetComponent<Rigidbody>().drag = Mathf.Infinity;
		}
	}
}
