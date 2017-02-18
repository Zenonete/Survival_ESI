using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	//Variables que recogen los Input de los Sticks y los gatillos.
	
	//Variables para los GadGets	
	static public bool gadgetPull;//Move sirve para detectar si alguno de los sticks o gatillos se esta pulsando;
	static public float gadgetX;//El resto recogera en update el valor de cada eje del stick.
	static public float gadgetY;
	
	static public bool dodgePull;
	static public float dodgeX;
	static public float dodgeY;
	//Variables para el movimiento
	static public bool movementPull;
	static public float movementX;
	static public float movementY;
	//Gatillos tarseros
	static public bool triggerPull;
	static public float triggerX;
	
	
	//Variables que recogen los Inputs de botones.
	static public bool touched;//Recogera el boton 0 del raton y los touch en android e Ios;
	static public bool jump;
	
	// Update is called once per frame
	void Update () 
	{
		//Este if sirve para separar los input segun la plataforma, cuando se compile, ignorara el resto de codigo y solo se quedara con el que le corresponde.
		#if UNITY_EDITOR
		//Unity Editor sirve para el editor, pc, web y flash...Creo.
		
		//Recogemos los ejes para el movimiento
		movementX=Input.GetAxis("Horizontal");
		movementY=Input.GetAxis("Vertical");
		if (movementX==0&&movementY==0)
		{
			if(!movementPull)
			{
				
			}
			else
			{
				movementPull=false;
			}
		}
		else if(movementPull)
		{
			
		}
		else
		{
			movementPull=true;
		}
		//recogemos los ejes para la esquiva
		dodgeX=Input.GetAxis("EsquivaH");
		dodgeY=Input.GetAxis("EsquivaV");
		if (dodgeX==0&&dodgeY==0)
		{
			if(!dodgePull)
			{
				
			}
			else
			{
				dodgePull=false;
			}
		}
		else if(dodgePull)
		{
			
		}
		else
		{
			dodgePull=true;
		}
		
		//recogemos los gatillos (sin funcionalidad oficial definida pero por ahora giraran la camara)
		triggerX=Input.GetAxis("GiroCam");
		
		if (triggerX==0)
		{
			if(!triggerPull)
			{
				
			}
			else
			{
				triggerPull=false;
			}
		}
		else if(triggerPull)
		{
			
		}
		else
		{
			triggerPull=true;
		}
		# endif
	
	}
}
