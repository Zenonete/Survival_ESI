using UnityEngine;
using System.Collections;

public class Dispersion : Bullet 
{
	[HideInInspector]
	public Vector2 viewDir;
	[HideInInspector]
	public float dispersion;
	[HideInInspector]
	public float scope;

	void OnTriggerEnter(Collider collider)
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
			Vector2 targetDir = new Vector2(collider.transform.position.x, collider.transform.position.z) - new Vector2(transform.position.x, transform.position.z);
			float distance = targetDir.magnitude;
			targetDir = targetDir/distance;

			if(Vector2.Angle(viewDir, targetDir) <= dispersion)
			{
			//Debug.Log("Quitar vida");
				//Debug.Log("Distancia = " + distance + " - Daño = " + (damage + scope)/(distance/2));
				collider.gameObject.GetComponent<FormOfLife>().SubstractLife((damage + scope)/(distance/2));
			}
			//return;
		}
		/*//Si es del mismo bando
		else if(collider.gameObject.GetComponent<FormOfLife>() != null && collider.tag == bulletIgnoreTag)
		{
			return;
		}*/

		//Si es el escenario
	}
}
