using UnityEngine;
using System.Collections;

public class ProjectileMortar : Bullet 
{
	public GameObject explosionGO;
	private GameObject explosionInstance;
	[HideInInspector]
	public float damageRadius;

	void OnTriggerEnter(Collider collider)
	{
		//No colisiona con otras balas
		if(collider.gameObject.GetComponent<Bullet>() != null)
		{
			return;
		}

		else 
		{
			explosionInstance = GameObject.Instantiate<GameObject>(explosionGO);
			explosionInstance.transform.position = transform.position;
			explosionInstance.transform.rotation = Quaternion.identity;

			Destroy(explosionInstance, 1f);

			Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

			for(int i = 0; i < colliders.Length; i++)
			{
				if(colliders[i].gameObject.GetComponent<FormOfLife>() != null && colliders[i].tag != bulletIgnoreTag)
				{
					//Debug.Log("Quitar vida");
					colliders[i].gameObject.GetComponent<FormOfLife>().SubstractLife(damage);
				}
			}
		}

		Destroy(this.gameObject);

	}

}
