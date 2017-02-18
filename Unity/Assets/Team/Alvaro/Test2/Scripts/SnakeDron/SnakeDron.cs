using UnityEngine;
using System.Collections;

public class SnakeDron : FormOfLife 
{
	[HideInInspector]
	public Animator animator;
	private SnakeDronMove moveSmb;
	private SnakeDronDead deadSmb;

	[HideInInspector]
	public Transform playerTR;

	private float rotationSpeed = 120;
	private Vector3 movePosition;
	[HideInInspector]
	public NavMeshAgent agent;

	public float shootTime = 0;
	private float shootRate = 5;
	[HideInInspector]
	public bool reloading = true;
	[HideInInspector]
	public float numberOfShoots;
	public float maxNumberOfShoots = 5;
	public GameObject curWeaponPrefab;
	public Transform weaponPosGO;
	private Weapon curWeapon;

	public int expValue = 20;

	[HideInInspector]
	public Path dronPath;
	[HideInInspector]
	public int pathPoint = 0;

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
		moveSmb = animator.GetBehaviour <SnakeDronMove> ();
		deadSmb = animator.GetBehaviour <SnakeDronDead> ();

		// Set the StateMachineBehaviour's reference to an ExampleMonoBehaviour to this.
		moveSmb.snakeDronMB = this;
		deadSmb.snakeDronMB = this;

		playerTR = GameObject.Find("ThirdPersonController").transform;

		numberOfShoots = maxNumberOfShoots;
		//agent.updateRotation = false;
	}

	// Update is called once per frame
	void Update () 
	{

	}

	public void RotateDron()
	{
		// Create a quaternion (rotation) based on looking down the vector from the dron to the player.
		Quaternion newRotatation = Quaternion.LookRotation((new Vector3(playerTR.position.x, transform.position.y, playerTR.position.z) - transform.position).normalized, Vector3.up);

		// Set the player's rotation to this new rotation.
		transform.rotation =  Quaternion.Slerp(transform.rotation, newRotatation, rotationSpeed);
	}

	public void UpdateShootRate()
	{
		shootTime += Time.deltaTime;
		if(shootTime >= shootRate)
		{
			shootTime = 0;
			reloading = false;
		}
	}

	public void Shoot()
	{
		curWeapon.ShootWeapon();
	}

	public void UpdateFireRateWeapon()
	{
		curWeapon.UpdateFireRate();
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
