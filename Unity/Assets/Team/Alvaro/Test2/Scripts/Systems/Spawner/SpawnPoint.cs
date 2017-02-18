using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
	public bool randomEnemy = false;
	public bool fixedEnemy = false;
	public int typeOfEnemy = 0;

	public bool randomRate = false;
	public int rate = 60;
	public int minRate = 20;
	public int maxRate = 90;

	public bool burst = false;
	public int burstNumber = 3;

	public bool secuencial = false;
	[HideInInspector]
	public int enemySecuenced = 0;
	public int enemysInSecuence = 8;
	public float secuenceRate = 0.5f;
	public Path path;

	public bool AreaEffector2D = false;
	public Vector2 areaDimensions = new Vector2(0.5f,0.5f);

	public bool spawn = false;
	public bool spawnSecuence = false;

	private float counter;
	private float counterSecuence;

	public void StartCounter()
	{
		if(randomRate)
		{
			counter = (float)Random.Range(minRate, maxRate);
		}
		else
		{
			counter = (float)rate;
		}
	}

	public void StartSecuenceCounter()
	{
		counterSecuence = secuenceRate;
	}

	public void Countdown()
	{
		counter -= Time.deltaTime;
		if(counter <= 0)
		{
			spawn = true;
		}
	}

	public void CountdownSecuence()
	{
		counterSecuence -= Time.deltaTime;
		if(counterSecuence <= 0)
		{
			spawnSecuence = true;
		}
	}
}