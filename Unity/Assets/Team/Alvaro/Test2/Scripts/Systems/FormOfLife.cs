using UnityEngine;
using System.Collections;

public class FormOfLife : MonoBehaviour 
{
	public float life = 100;
	public float lifeMax = 100;
	public bool alive = true;

	public void AddLife(float value)
	{
		life += value;
		if(life > lifeMax)
			life = lifeMax;
	}

	public void SubstractLife(float value)
	{
		life -= value;
		if(life < 0)
		{
			life = 0;
			alive = false;
		}
	}
}
