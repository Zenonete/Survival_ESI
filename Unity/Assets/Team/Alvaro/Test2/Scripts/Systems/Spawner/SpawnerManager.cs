using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour 
{
	//public int MaxNumberOfEnemys = 100;
	//public int enemysSpawned = 0;
	public SpawnPoint[] spawnPoints;
	public GameObject[] enemys;

	private GameObject instance;
	//public string[] enemyType;

	//public int maxEnemys;
	//private List<GameObject> listOfEnemys = new List<GameObject>();
	//private Dictionary<string, SpawnPoint> spawners = new Dictionary<string, SpawnPoint>();


	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			spawnPoints[i].StartCounter();
			if(spawnPoints[i].secuencial)
			{
				spawnPoints[i].StartSecuenceCounter();
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			spawnPoints[i].Countdown();
			if(spawnPoints[i].spawn)
			{
				if(!spawnPoints[i].secuencial)
				{
					SpawnEnemy(spawnPoints[i]);
					spawnPoints[i].spawn = false;
					spawnPoints[i].StartCounter();
				}
				else //For Snake Drons
				{
					//SpawnEnemy(spawnPoints[i]);
					spawnPoints[i].CountdownSecuence();
					if(spawnPoints[i].spawnSecuence)
					{
						SpawnEnemy(spawnPoints[i]);
						spawnPoints[i].enemySecuenced++;
						spawnPoints[i].StartSecuenceCounter();
						spawnPoints[i].spawnSecuence = false;
						if(spawnPoints[i].enemySecuenced == spawnPoints[i].enemysInSecuence)
						{
							spawnPoints[i].enemySecuenced = 0;
							spawnPoints[i].spawn = false;
							spawnPoints[i].StartCounter();
						}
					}
				}
			}
		}
	}

	private void SpawnEnemy(SpawnPoint sp)
	{
		//if(enemysSpawned < MaxNumberOfEnemys)
		//{
			if(sp.randomEnemy)
			{
				if(sp.burst)
				{
					for(int i = 0; i < sp.burstNumber; i++)
					{
						sp.typeOfEnemy = Random.Range(0, enemys.Length);
						InstantiateEnemy(sp);
					}
				}
				else
				{
					sp.typeOfEnemy = Random.Range(0, enemys.Length);
					InstantiateEnemy(sp);
				}
			}
			else
			{
				if(sp.burst)
				{
					for(int i = 0; i < sp.burstNumber; i++)
					{
						InstantiateEnemy(sp);
						if(!sp.fixedEnemy)
						{
							sp.typeOfEnemy ++;
						}
						if(sp.typeOfEnemy > enemys.Length)
						{
							sp.typeOfEnemy = 0;
						}
					}
				}
				else
				{
					InstantiateEnemy(sp);
					if(!sp.fixedEnemy)
					{
						sp.typeOfEnemy ++;
					}
					if(sp.typeOfEnemy > enemys.Length)
					{
						sp.typeOfEnemy = 0;
					}
				}
			}

			//enemysSpawned ++;
		//}
	}

	private void InstantiateEnemy(SpawnPoint sp)
	{
		//Vector3 instancePosition;
		instance = GameObject.Instantiate<GameObject>(enemys[sp.typeOfEnemy]);

		if(!sp.secuencial)
		{
			if(!sp.AreaEffector2D)
			{
				instance.transform.position = sp.transform.position;
			}
			else
			{
				instance.transform.position = new Vector3(sp.transform.position.x + Random.Range(-sp.areaDimensions.x, sp.areaDimensions.x), 
														sp.transform.position.y, 
														sp.transform.position.z + Random.Range(-sp.areaDimensions.y, sp.areaDimensions.y));
			}

			/*NavMeshHit closestHit;
			if(NavMesh.SamplePosition(instancePosition, out closestHit, 500, NavMesh.AllAreas))
			{
				instancePosition = closestHit.position;
			}
			else
			{
				Debug.Log("NO ENCUENTRO EL NAVMESH!!!!!!");
			}

			instance = GameObject.Instantiate<GameObject>(enemys[sp.typeOfEnemy]);
			instance.transform.position = instancePosition;*/
			instance.transform.rotation = sp.transform.rotation;
		}
		else
		{
			if(sp.path != null)
			{
				instance.transform.position = sp.path.pathPoints[0].position;
				instance.transform.rotation = sp.path.pathPoints[0].rotation;
				if(instance.GetComponent<SnakeDron>() != null)
				{
					instance.GetComponent<SnakeDron>().dronPath = sp.path;
				}
			}
		}
	}
}


