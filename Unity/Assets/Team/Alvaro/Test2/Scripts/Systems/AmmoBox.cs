using UnityEngine;
using System.Collections;

public class AmmoBox : MonoBehaviour 
{
	public GameObject weaponPrefab;

	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.GetComponent<Char>() != null && collider.tag == "Player")
		{
			Debug.Log("Toca al player");
			Destroy(collider.gameObject.GetComponent<Char>().curWeapon.gameObject);
			collider.gameObject.GetComponent<Char>().curWeaponPrefab = weaponPrefab;
			GameObject go = collider.gameObject.GetComponent<Char>().GenerateWeapon();
			collider.gameObject.GetComponent<Char>().curWeapon = go.GetComponent<Weapon>();
			Destroy(this.gameObject);
		}
	}
}
