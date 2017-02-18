using UnityEngine;
using System.Collections;

public class Bezier : MonoBehaviour 
{
	public float startTime;	//Define la velocidad de movimiento

	public float c; 		//Valor para t de la ecuacion de la curva cuadratica
	public Vector3 q0, q1;	//Vectores para guardar el inicio y fin para cada segmento de la curva

	public Transform start, end, handle1, handle2; 

	// Use this for initialization
	void Start () 
	{
		startTime = Time.time;

		c = 0.0f; 			//Para la primera curva c es 0
		q0 = CalculateBezierPoint(c, start.position, handle1.position, handle2.position, end.position);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(c <= 1)		//Definimos 100 pasos para dibujar la curva
		{
			c += 0.01f;	//100 pasos para dibujar cada curva bezier

			q1 = CalculateBezierPoint(c, start.position, handle1.position, handle2.position, end.position);

			Debug.DrawLine(q0, q1, Color.red, 1000);

			transform.position = Vector3.Lerp(q0, q1, (Time.time - startTime) * 0.5f); //Mueve un objeto con la velocidad dada

			q0 = q1;
		}
	}

	//Calcula las coordenadas de los puntos de una curva cuadratica 
	//[x,y,z] = (1-t)^3 p0 + 3(1-t)^2 t p1 + 3(1-t)t^2 p2 + t^3 p3
	//t es el rango de 0, punto inicial de la curva a 1, punto final de la curva
	//p0 es el punto inicial de la curva
	//p3 es el punto final de la curva
	//p1,p2 son las asas del bezier
	public Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float u, uu, uuu, tt, ttt;
		Vector3 p;

		u = 1 - t;
		uu = u * u;
		uuu = uu * u;

		tt = t * t;
		ttt = tt * t;

		p = uuu * p0; 					//Primer termino de la ecuacion
		p += 3 * uu * t * p1; 			//Segundo termino de la ecuacion
		p += 3 * u * tt * p2;			//Tercer termino de la ecuacion
		p += ttt * p3;					//Cuarto termino de la ecuacion

		return p;
	}
}
