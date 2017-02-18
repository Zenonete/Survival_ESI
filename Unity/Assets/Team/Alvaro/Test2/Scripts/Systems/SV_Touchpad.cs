using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SV_Touchpad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerExitHandler 
{
	public float smoothing;
	public float maxOriginDistance;
	public float swipeDistance;
	public float tapTime;
	public bool fixedMovement = false;

	public Image joystick;
	public Image joystickBackground;

	[HideInInspector]
	public bool swipe;
	[HideInInspector]
	public bool tap;

	private Vector2 origin;
	private Vector2 direction;
	private Vector2 smoothDirection;
	private bool touched;
	private int pointerID;

	private float scaleFactor;
	public float maxOriginDistanceVal;
	public float swipeDistanceVal;

	private Vector2 currentPosition;
	private Vector2 lastPosition;
	private Vector2 directionRaw;
	private float lastClickTime;

	void Awake()
	{
		direction = Vector2.zero;
		touched = false;
	}

	// Use this for initialization
	void Start () 
	{
		joystick.enabled = false;
		joystickBackground.enabled = false;

		swipe = false;
		tap = false;

		//Inicializa factor de escala para las distancias de desplazamiento
		maxOriginDistanceVal = maxOriginDistance;
		swipeDistanceVal = swipeDistance;
		scaleFactor = transform.parent.GetComponent<Canvas>().scaleFactor;
		maxOriginDistance = maxOriginDistanceVal * scaleFactor;
		swipeDistance = swipeDistanceVal * scaleFactor;
	}
	
	// Update is called once per frame
	void Update () 
	{
#if UNITY_EDITOR
		//Calcula el factor de escala si cambia la resolución de pantalla
		scaleFactor = transform.parent.GetComponent<Canvas>().scaleFactor;
		maxOriginDistance = maxOriginDistanceVal * scaleFactor;
		swipeDistance = swipeDistanceVal * scaleFactor;
#endif

		//Vector2 v = GetDirection();
	}

	public void OnPointerDown (PointerEventData data) 
	{
		//GetClicks(data);

		if(!touched)
		{
			//Calcula el tap por tiempo
			if(Time.time - lastClickTime < tapTime)
			{
				Debug.Log("Tap!!!");
				tap = true;
				return;
			}
			lastClickTime = Time.time;

			//Si no es tap y tocamos habilitamos el toque e inicializamos los valores
			touched = true;
			pointerID = data.pointerId;
			origin = data.position;

			//Habilitamos el joystic
			joystick.enabled = true;
			joystickBackground.enabled = true;
			joystick.rectTransform.position = new Vector3(origin.x, origin.y, 0);
			joystickBackground.rectTransform.position = new Vector3(origin.x, origin.y, 0);

			//Actualizamos la posicion del joystic
			currentPosition = data.position;
			lastPosition = data.position;

		}

	}

	public void OnDrag (PointerEventData data) 
	{
		if(data.pointerId == pointerID)
		{
			//Si arrastramos obtenemos la dirección de movimiento
			lastPosition = currentPosition;
			currentPosition = data.position;
			directionRaw = currentPosition - origin;
			direction = directionRaw.normalized;
			//Debug.Log(directionRaw/maxOriginDistance);

			//Si pasamos de la distancia de swipe en un instante, hacemos swipe
			if((currentPosition - lastPosition).magnitude > swipeDistance)
			{
				//Debug.Log("Swipe!");
				swipe = true;
				return;
			}

			//Sino, comprobamos si el joystic se mueve lejos por si es necesario su centro que siga nuestro dedo
			if(directionRaw.magnitude > maxOriginDistance)
			{
				//Si el joystic no es fijo hacemos que el centro lo siga
				if(!fixedMovement)
				{
					origin = currentPosition - (direction * maxOriginDistance);
					joystickBackground.rectTransform.position = new Vector3(origin.x, origin.y, 0);
				}
				//Si es fijo limitamos la posicion del joystic
				else
				{
					Vector3 fixedPosition = origin + (direction * maxOriginDistance);
					joystick.rectTransform.position = new Vector3(fixedPosition.x, fixedPosition.y, 0);
					return;
				}
			}

			//Sino no movemos el centro y solo el joystic
			joystick.rectTransform.position = new Vector3(currentPosition.x, currentPosition.y, 0);
		}
	}

	public void OnPointerUp (PointerEventData data) 
	{
		//Si levantamos el dedo deshabilitamos el joystic
		if(data.pointerId == pointerID)
		{
			direction = Vector3.zero;
			directionRaw = Vector3.zero;
			touched = false;

			joystick.enabled = false;
			joystickBackground.enabled = false;

			tap = false;
			swipe = false;
		}
	}

	public void OnPointerExit (PointerEventData data) 
	{
		//Si salimos de la zona deshabilitamos el joystic
		if(data.pointerId == pointerID)
		{
			direction = Vector3.zero;
			directionRaw = Vector3.zero;
			touched = false;

			joystick.enabled = false;
			joystickBackground.enabled = false;

			tap = false;
			swipe = false;
		}
	}


	//***********************************************************************************
	//***********************************************************************************

	public Vector2 GetDirection()
	{
		//smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing);
		//Debug.Log(smoothDirection.magnitude);
		//return smoothDirection;
		return directionRaw/maxOriginDistance;
	}

}
