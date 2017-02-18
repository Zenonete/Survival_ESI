using UnityEngine;
using System.Collections;

[System.Serializable]
public class Weapon : MonoBehaviour
{

	#region Public Variables
	public float damage;
	public float fireRate;
	public float ammo;
	public float scope;
	public float dispersion;
	public float penetration;
	public float bulletVelocity;
	public float mortarTargetVelocity;
	public float mortarDamageRadius;
	public string parentTag;

	public enum WeaponType 
	{
		BURST,
		DISPERSION,
		RAY,
		MORTAR
	};

	public WeaponType weaponType;

	public enum Quality
	{
		LOW,
		MEDIUM,
		HIGH
	};

	public Quality weaponQuality;

	public GameObject prefabGO;
	public GameObject visibleModelGO;
	public GameObject bulletGO;
	public Transform spawnPointBullet;
	public GameObject target;
	#endregion
	
	#region Private Variables
	private GameObject bulletInstance;

	//private SV_CharController currentCharacter;
	private Char currentCharacter;

	private float shootTime = 0;
	#endregion

	#region Main Methods
	void Awake()
	{
		if(currentCharacter == null)
		{
			//currentCharacter = GameObject.Find("SV_CharCtrl").GetComponent<SV_CharController>();
			currentCharacter = GameObject.Find("ThirdPersonController").GetComponent<Char>();
			target.transform.SetParent(currentCharacter.transform.parent);
			target.transform.position = new Vector3(currentCharacter.transform.position.x, -0.95f, currentCharacter.transform.position.z);
			target.transform.localScale = new Vector3(dispersion, dispersion, dispersion); 
		}
	}

	void Update()
	{
		/*switch (weaponType)
		{
		case WeaponType.MORTAR:
			UpdateTarget();
			break;
		}*/
	}

	public void ShootWeapon()
	{
		if(ammo > 0)
		{
			switch (weaponType)
			{
			case WeaponType.BURST:
				ShootBurst();
				break;

			case WeaponType.DISPERSION:
				ShootDispersion();
				break;

			case WeaponType.RAY:
				ShootRay();
				break;

			case WeaponType.MORTAR:
				ShootMortar();
				break;
			}
		}
	}

	public void UpdateFireRate()
	{
		if(shootTime > 0)
		{
			shootTime -= Time.deltaTime;
			if(shootTime < 0)
			{
				shootTime = 0;
			}
		}
	}

	public void UpdateAmmo()
	{
		ammo--;
		if(ammo<=0)
			ammo=0;
	}
	#endregion
	
	#region Utility Methods
	void ShootBurst()
	{
		if(shootTime <= 0)
		{
			shootTime = fireRate;
			UpdateAmmo();
			bulletInstance = GameObject.Instantiate<GameObject>(bulletGO);
			bulletInstance.transform.position = spawnPointBullet.position;
			bulletInstance.transform.rotation = spawnPointBullet.rotation;
			bulletInstance.GetComponent<Bullet>().damage = damage;
			bulletInstance.GetComponent<Bullet>().bulletIgnoreTag = parentTag;
			//bulletInstance.GetComponent<Rigidbody>().AddForce(Vector3.forward * bulletVelocity) + (Vector3.right * Random.Range(-dispersion/100, dispersion/100)));
			bulletInstance.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(-dispersion, dispersion), 0, bulletVelocity));
			if(bulletInstance != null)
			{
				Destroy(bulletInstance, scope);
			}
		}
	}

	void ShootDispersion()
	{
		if(shootTime <= 0)
		{
			shootTime = fireRate;
			UpdateAmmo();
			bulletInstance = GameObject.Instantiate<GameObject>(bulletGO);
			bulletInstance.transform.position = transform.parent.position;
			bulletInstance.transform.rotation = transform.parent.rotation;
			bulletInstance.transform.localScale = bulletInstance.transform.localScale * scope;
			bulletInstance.GetComponent<Dispersion>().damage = damage;
			bulletInstance.GetComponent<Dispersion>().bulletIgnoreTag = parentTag;
			bulletInstance.GetComponent<Dispersion>().viewDir = new Vector2(transform.parent.forward.x, transform.parent.forward.z);
			bulletInstance.GetComponent<Dispersion>().dispersion = dispersion;
			bulletInstance.GetComponent<Dispersion>().scope = scope;
			Destroy(bulletInstance, 0.1f);
		}
	}

	void ShootRay()
	{
		if(shootTime <= 0)
		{
			if(!GetComponent<LineRenderer>())
				gameObject.AddComponent<LineRenderer>();

			shootTime = fireRate;
			UpdateAmmo();

			RaycastHit hitInfo;
			Vector3 endPos = spawnPointBullet.position + spawnPointBullet.parent.parent.forward * scope;
			Vector3[] midPos;
			if(Physics.Raycast(spawnPointBullet.position, spawnPointBullet.parent.parent.forward, out hitInfo, scope))
			{
				endPos = spawnPointBullet.position + spawnPointBullet.parent.parent.forward * hitInfo.distance;
				midPos = new Vector3[(int)hitInfo.distance];
			}
			else
			{
				midPos = new Vector3[(int)scope];
			}

			GetComponent<LineRenderer>().SetWidth(1f,1f);
			GetComponent<LineRenderer>().material = Resources.Load ("Rayo", typeof(Material)) as Material;
			GetComponent<LineRenderer>().SetVertexCount((midPos.Length-1)+2);
			GetComponent<LineRenderer>().SetPosition (0, spawnPointBullet.position);
			for(int i=0; i<midPos.Length; i++)
			{
				GetComponent<LineRenderer>().SetPosition (i+1, spawnPointBullet.position + spawnPointBullet.parent.parent.forward * (i+1.0f));
			}
			//GetComponent<LineRenderer>().SetPosition (1, spawnPointBullet.position + spawnPointBullet.parent.parent.forward * scope);
			GetComponent<LineRenderer>().SetPosition ((midPos.Length-1)+1, endPos);

			if(hitInfo.collider != null)
			{
				if(hitInfo.collider.gameObject.GetComponent<FormOfLife>() != null)
				{
					hitInfo.collider.gameObject.GetComponent<FormOfLife>().SubstractLife(damage);
				}
			}

			Destroy(GetComponent<LineRenderer>(), 0.1f);
		}
	}

	void ShootMortar()
	{
		if(shootTime <= 0)
		{
			shootTime = fireRate;
			UpdateAmmo();
			bulletInstance = GameObject.Instantiate<GameObject>(bulletGO);
			bulletInstance.transform.position = spawnPointBullet.position;
			bulletInstance.GetComponent<Bullet>().damage = damage;
			bulletInstance.GetComponent<Bullet>().bulletIgnoreTag = parentTag;
			bulletInstance.GetComponent<ProjectileMortar>().damageRadius = mortarDamageRadius;
			StartCoroutine(SimulateProjectile(bulletInstance));
		}
	}

	IEnumerator SimulateProjectile(GameObject bulletInstance)
	{
		// Short delay added before Projectile is thrown
		//yield return new WaitForSeconds(0.5f);
		
		// Move projectile to the position of throwing object + add some offset if needed.
		bulletInstance.transform.position = spawnPointBullet.position + new Vector3(0, 0.0f, 0);

		// Calculate distance to target
		//float target_Distance = Vector3.Distance(bulletInstance.position, Target.position);
		/*float target_Distance = Vector2.Distance(new Vector2(currentCharacter.transform.position.x, currentCharacter.transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z));

		// Calculate the velocity needed to throw the object to the target at specified angle.
		float projectile_Velocity = target_Distance / (Mathf.Sin(2 * 45 * Mathf.Deg2Rad) / -Physics.gravity.y);
		Debug.Log (projectile_Velocity);
		// Extract the X  Y componenent of the velocity
		float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(45 * Mathf.Deg2Rad);
		float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(45 * Mathf.Deg2Rad);

		// Calculate flight time.
		float flightDuration = target_Distance / Vx;

		// Rotate projectile to face the target.
		bulletInstance.transform.rotation = Quaternion.LookRotation((spawnPointBullet.position + spawnPointBullet.parent.parent.forward * scope) - bulletInstance.transform.position);
		
		float elapse_time = 0;
		
		while (elapse_time < flightDuration)
		{
			bulletInstance.transform.Translate(0, (Vy - (-Physics.gravity.y * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
			
			elapse_time += Time.deltaTime;
			
			yield return null;
		}*/
		Vector3 impactPosition;
		if(Vector3.Distance(spawnPointBullet.position, target.transform.position) <= scope)
		{
			impactPosition = target.transform.position;
		}
		else
		{
			Vector3 dir = (target.transform.position - spawnPointBullet.position);
			dir.Normalize();
			impactPosition = dir * scope;
		}



		Vector3 throwSpeed = CalculateBestThrowSpeed(spawnPointBullet.position, new Vector3(impactPosition.x + Random.Range(-dispersion/2, dispersion/2), target.transform.position.y , impactPosition.z + Random.Range(-dispersion/2, dispersion/2)) , scope/4);
		bulletInstance.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.VelocityChange);
		Destroy(bulletInstance, scope/4);
		yield return null;
	}  

	private Vector3 CalculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
	{
		//Calcular vectores
		Vector3 toTarget = target - origin;
		Vector3 toTargetXZ = toTarget;
		toTargetXZ.y = 0;

		//Calcular xz e y
		float y = toTarget.y;
		float xz = toTargetXZ.magnitude;

		//Calcular velocidades iniciales para xz e y.
		//deltaX = v0 * t + 1/2 * a * t * t
		//a = -gravity en y, 0 en xz
		//xz = v0xz * t => v0xz = xz/t
		//y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
		float t = timeToTarget;
		float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
		float v0xz = xz / t;

		//Crea el vector resultante de las velociades iniciales calculadas
		Vector3 result = toTargetXZ.normalized; //Obtener la direccion xz con magnitud 1
		result *= v0xz;	//Obtiene la magnitud de xz a v0xz(velocidad inicial en xz)
		result.y = v0y;	//Obtiene y como v0y (velocidad inicial y)

		return result;
	}

	/*void UpdateTarget()
	{
		if(currentCharacter.wantedHorizontalRotValue != 0 || currentCharacter.wantedVerticalRotValue != 0)
		{
			if(!target.activeSelf)
			{
				target.SetActive(true);
			}
			Vector2 pos = new Vector2(currentCharacter.wantedHorizontalRotValue, currentCharacter.wantedVerticalRotValue);
			pos = pos.normalized * scope;

			target.transform.position = new Vector3(currentCharacter.transform.position.x + pos.x , target.transform.position.y, currentCharacter.transform.position.z - pos.y);
		}
		else
		{
			if(target.activeSelf)
			{
				target.SetActive(false);
			}
		}
	}*/
	#endregion
}
