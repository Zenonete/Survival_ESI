using UnityEngine;
using System.Collections;

public class PruebaStatica : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(InputManager.movementPull)
		{
			Vector3 targetDirection=Tools.EjeRelativoCamara(InputManager.movementX,InputManager.movementY);
			float angulo=Tools.AnguloRelativoObjeto(transform,targetDirection);
			string orientacion=Tools.AnguloRelativoObjetoST(transform,targetDirection);
			Debug.Log (angulo+"  "+orientacion);
		}
	}
	
}
