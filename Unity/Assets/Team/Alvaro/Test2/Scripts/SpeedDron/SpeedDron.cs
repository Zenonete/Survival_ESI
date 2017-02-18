using UnityEngine;
using System.Collections;

public class SpeedDron : FormOfLife 
{
	[HideInInspector]
	public Animator animator;
	private SpeedDronSearch searchSmb;
	private SpeedDronMove moveSmb;
	private SpeedDronDead deadSmb;

	[HideInInspector]
	public Transform playerTR;
	[HideInInspector]
	public float playerDistance;

	private float rotationSpeed = 120;
	private Vector3 movePosition;
	[HideInInspector]
	public NavMeshAgent agent;

	public float waitingTime = 0;
	private float waitingTimeMax = 3;
	[HideInInspector]
	public bool waiting = false;

	public float shootTime = 0;
	private float shootRate = 5;
	//[HideInInspector]
	public bool reloading = true;
	public GameObject curWeaponPrefab;
	public Transform weaponPosGO;
	private Weapon curWeapon;

	public int expValue = 20;

	void Awake () 
	{
		animator = GetComponent <Animator> ();
		agent = GetComponent<NavMeshAgent>();
	}

	// Use this for initialization
	void Start () 
	{
		GameObject go = GenerateWeapon();
		curWeapon = go.GetComponent<Weapon>();

		// Find a reference to the ExampleStateMachineBehaviour in Start since it might not exist yet in Awake.
		searchSmb = animator.GetBehaviour <SpeedDronSearch> ();
		moveSmb = animator.GetBehaviour <SpeedDronMove> ();
		deadSmb = animator.GetBehaviour <SpeedDronDead> ();

		// Set the StateMachineBehaviour's reference to an ExampleMonoBehaviour to this.
		searchSmb.speedDronMB = this;
		moveSmb.speedDronMB = this;
		deadSmb.speedDronMB = this;

		playerTR = GameObject.Find("ThirdPersonController").transform;

		//agent.updateRotation = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public bool CheckDistance()
	{
		playerDistance = Mathf.Abs(Vector3.Distance(transform.position, playerTR.position));
		if(playerDistance < 10)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void RotateDron()
	{
		// Create a quaternion (rotation) based on looking down the vector from the dron to the player.
		Quaternion newRotatation = Quaternion.LookRotation((new Vector3(playerTR.position.x, transform.position.y, playerTR.position.z) - transform.position).normalized, Vector3.up);

		// Set the player's rotation to this new rotation.
		transform.rotation =  Quaternion.Slerp(transform.rotation, newRotatation, rotationSpeed);
	}

	public void Waiting()
	{
		waitingTime += Time.deltaTime;
		if(waitingTime >= waitingTimeMax || agent.remainingDistance < 0.5f)
		{
			waitingTime = 0;
			waiting = false;
		}
	}

	public void UpdateShootRate()
	{
		shootTime += Time.deltaTime;
		if(shootTime >= shootRate)
		{
			shootTime = 0;
			reloading = false;
		}
		curWeapon.UpdateFireRate();
	}

	public void Shoot()
	{
		curWeapon.ShootWeapon();
	}

	private GameObject GenerateWeapon()
	{
		GameObject go = Instantiate<GameObject>(curWeaponPrefab);
		go.transform.position = weaponPosGO.position;
		go.transform.rotation = weaponPosGO.rotation;
		go.transform.SetParent(this.transform);

		go.GetComponent<Weapon>().spawnPointBullet = weaponPosGO;
		go.GetComponent<Weapon>().parentTag = gameObject.tag;
		GameObject visibleMesh = go.GetComponent<Weapon>().visibleModelGO;
		Destroy(visibleMesh.GetComponent<Renderer>());
		Destroy(visibleMesh.GetComponent<MeshFilter>());

		return go;
	}

}
