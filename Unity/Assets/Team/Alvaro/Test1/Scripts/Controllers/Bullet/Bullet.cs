using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	[HideInInspector]
	public float damage;
	[HideInInspector]
	public string bulletIgnoreTag;

	/*void OnTriggerEnter(Collider collider)
	{
		//No colisiona con otras balas
		if(collider.gameObject.GetComponent<Bullet>() != null)
		{
			return;
		}

		//Si es enemigo o player
		//Si es del bando contrario
		if(collider.gameObject.GetComponent<FormOfLife>() != null && collider.tag != bulletIgnoreTag)
		{
			//Debug.Log("Quitar vida");
			collider.gameObject.GetComponent<FormOfLife>().SubstractLife(damage);
			Destroy(this.gameObject);
			return;
		}
		//Si es del mismo bando
		else if(collider.gameObject.GetComponent<FormOfLife>() != null && collider.tag == bulletIgnoreTag)
		{
			return;
		}

		//Si es el escenario
		Destroy(this.gameObject);
	}*/
}
