using UnityEngine;
using System.Collections;

public static class Tools//Clase herramientas que contiene metodos estaticos generales, normalmente de calculo.
{
	public static Vector3 EjeRelativoCamara(float h,float v)//Este metodo calcula la direccion en el plano horizontal relativo al pesonaje de la inclinacion de cualquier palanca que se le mande.
	{
		//Cogemos el transform de la cámara;
        Transform cameraTransform = Camera.main.transform;
        //Forward vector relativo a la cámara;
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        //Right vector relativo a la cámara;
        Vector3 right =new Vector3(forward.z, 0, -forward.x);

        //Cogemos los ejes de movimiento;
        /*float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");*/
        
        // Vector de direccion relativo a la cámara
        Vector3 targetDirection = h * right + v * forward;
		return targetDirection;
	}
	public static float AnguloRelativoObjeto(Transform obj,Vector3 direction)
	{
		float fAngulo;
		float anguloF=Vector3.Angle(obj.forward,direction);
		float anguloR=Vector3.Angle(obj.right,direction);
		if(anguloR>90F)
		{
			fAngulo=359-anguloF;
		}
		else
		{
			fAngulo=0+anguloF;
		}
		return fAngulo;
	}
	public static string AnguloRelativoObjetoST(Transform obj,Vector3 direction)
	{
		string sAngulo;
		
		float anguloF=Vector3.Angle(obj.forward,direction);
		float anguloR=Vector3.Angle (obj.right,direction);
		if(anguloR>90F)
		{
			
			if(anguloF<=45)
			{
				sAngulo="Front";
			}
			else if (anguloF>45&&anguloF<=135)
			{
				sAngulo="Left";
			}
			else{sAngulo="Back";}
			
		}
		else
		{
			
			if(anguloF<=45)
			{
				sAngulo="Front";
			}
			else if (anguloF>45&&anguloF<=135)
			{
				sAngulo="Right";
			}
			else{sAngulo="Back";}
			
		}
		return sAngulo;
	}
}
