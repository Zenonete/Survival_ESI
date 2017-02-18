using UnityEngine;
using System.Collections;

public class Parabolic : MonoBehaviour 
{

	public Vector3 bulletSpeed;
	public Transform bulletTransform;
	public float bulletFireSpeed;
	public float downwardSpeed;
	public Vector3 newPosition;

	private RaycastHit ray;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	void Update () 
	{
		//Check where the bullet needs to go this frame
		bulletSpeed = (bulletTransform.forward * (bulletFireSpeed * Time.deltaTime)) - (Vector3.up * Time.deltaTime * downwardSpeed);

		newPosition = transform.position + bulletSpeed;

		//raycast to ensure we wont go through terrain or object
		if (!Physics.Linecast(bulletTransform.position, newPosition, out ray))
		{
			bulletTransform.position = newPosition;
		}else
		{
			//Impact(ray);
		}


		downwardSpeed += 5.0f * Time.deltaTime;

	}
}
