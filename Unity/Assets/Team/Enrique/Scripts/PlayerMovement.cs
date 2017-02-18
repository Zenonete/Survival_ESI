using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	enum Modos 
	{
		Exploration,
		Combat,
		TargetCombat
	};
	enum Estados 
	{
		Idle,
		
		Walk,
		
		Run,
		
		JumpIdle,
		
		
		JumpRun,
		
		Fall,
		HightFall,
		DeadFall,
		
		MoveTarget,
		
		Dodge
	};

    private Modos modo;
	private Estados estado;
	public float walkSpeed = 3F;
	public float runSpeed = 6F;
	private float slideSpeed= 12F;
	private float surfSpeed=16F;
	private float controlableSurfSpeed=5F;
	public float speed = 6.0F;
	public float rotateSpeed = 8F;
    public float jumpSpeed = 4.0F;
    public float gravity = 30.0F;
	public bool saltando=false;
	public bool sliding=false;
	public bool surfing=false;
	public bool rayGrounded;
	public bool grounded;
    
	public Vector3 normalDirection;
	public Vector3 surfDirection=Vector3.zero;
	private float verticalSpeed = 0.0F;
	private Vector3 inAirVelocity = Vector3.zero;
    private Vector3 moveDirection =Vector3.zero;
	public Vector3 slideDirection=Vector3.zero;
	private Vector3 lastMoveDirection= Vector3.zero;
    CharacterController controller;
	// Use this for initialization
	
    void Start()
	{
		controller = GetComponent<CharacterController>();
		modo=Modos.Exploration;
		estado=Estados.Idle;
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) //Aqui se controla que pasa cuando el controller colisiona.
	{
		float pushPower = 3.0F;

		Rigidbody body = hit.collider.attachedRigidbody;
			if (body == null || body.isKinematic)
				return;
			
			if (hit.moveDirection.y < -0.3F)
				return;
		body.AddForce(new Vector3(moveDirection.x,1.0f,moveDirection.z)*pushPower, ForceMode.Impulse);
	}
	void FixedUpdate()
	{

	}
	// Update is called once per frame
	void Update ()
    {
		CheckFloor();
        CalculateMovement();//funcion para calcular el movimiento
		AplyGravity();//sumamos la gravedad al movimiento
		AplyMovement();//aplicamos el movimiento
		RelativeToCameraRotation(moveDirection);//funcion para orientar el player en la direccion de movimiento
	}
    void CalculateMovement()
    {
		
       //Calculamos el eje relativo a la camara
        Vector3 targetDirection = Tools.EjeRelativoCamara(InputManager.movementX,InputManager.movementY);
				  
		//Movemos el control
		if ((controller.isGrounded||rayGrounded)&&!saltando)//si estamos en el suelo y no estamos saltando...
        {
			if (!surfing)//si no estamos en una superficie de surf
			{
				if (!sliding)//si no estamos en una superficie de deslizamiento
				{
           			moveDirection = targetDirection.normalized;//la direccion de movimiento se iguala a la direccion objetivo y se normaliza,...
					CheckSpeed(targetDirection);
					moveDirection=Vector3.ProjectOnPlane(moveDirection,normalDirection);
            		moveDirection *= speed;//...para multiplicarla por la velocidad.
            		
					if (Input.GetButtonDown("Jump"))
            		{
						
						saltando=true;//activamos saltar
						moveDirection*=1.5F;//y aceleramos el vector de movimiento general para hacer un impulso hacia adelante
						moveDirection.y = jumpSpeed;//al saltar se suma la velocidad de salto a moveDirecction
						
						RelativeToCameraRotation(moveDirection);//y fuerza la rotacion para que cuando este en el aire siempre se oriente hacia donde se mueve.
            		}
					
				}
				else//si estamos en superficie de deslizamiento
				{
					slideDirection.Normalize();//normalizamos el vector de deslizamiento
					slideDirection*=slideSpeed;//lo multiplicamos por la velocidad de deslizamiento
					//slideDirection.y=yDirection;// e igualamos el y a la normal para que empuje hacia abajo y no salte.
					moveDirection=slideDirection;// se asigna el vector de deslizamiento a moveDirection.
				}
			}
			else//si estamos en superficie de surf
			{
				moveDirection=surfDirection.normalized;//igualamos moveDirection a surfDirection normalizado
				moveDirection=Vector3.ProjectOnPlane(moveDirection,normalDirection);//aplicamos la inclinacion de la normal para que empuje hacia abajo
				moveDirection*=surfSpeed;//aplicamos la velocidad
				if (Input.GetButtonDown("Jump"))
				{
					
					saltando=true;//activamos saltar
					//moveDirection*=1.5F;//y aceleramos el vector de movimiento general para hacer un impulso hacia adelante
					moveDirection.y = jumpSpeed*2;//al saltar se suma la velocidad de salto a moveDirecction
					
					RelativeToCameraRotation(moveDirection);//y fuerza la rotacion para que cuando este en el aire siempre se oriente hacia donde se mueve.
				}
				moveDirection+=targetDirection.normalized*controlableSurfSpeed;//sumamos targetDirection para tener control sobre el player 
			}
		}
		else if (controller.isGrounded&&saltando)//recogemos el momento en el que tocamos suelo despues de un salto por si hace flata comprobar algo...solo funcionara con controller.isGrounded
		{
			saltando=false;
		}
		
        		
    }
	void AplyGravity()
	{
		moveDirection.y -= gravity * Time.deltaTime;//se suma la gravedad,...
	}
	void AplyMovement()
	{
		controller.Move(moveDirection * Time.deltaTime);// y se aplica el movimiento;
	}
	void RelativeToCameraRotation(Vector3 dir)
	{
		//Rotamos el control
		
			Quaternion rot = transform.rotation;//se coge la rotacion del transform;
			Quaternion toTarget;//y se crea una variable para la rotacion objetivo.
		dir.y=0; // se anula el eje y de dir para que no tenga en cuenta la direccion de caida ni de salto
		if (controller.isGrounded||rayGrounded)
		{	
			if (dir.magnitude>0.5)
			{
				
				//Debug.Log(dir.magnitude);
				toTarget= Quaternion.LookRotation (dir);//si se esta moviendo to target es igual a la direccion de movimiento
				lastMoveDirection=moveDirection;//y se guarda la ultima direccion de movimiento.
				
			}
			else
			{
				toTarget=Quaternion.LookRotation (lastMoveDirection);//si no se mueve se aplica la ultima direccion de movimiento para que no se quede de espaldas a la camara.
			}
		
			
		}
		else
		{
			toTarget=Quaternion.LookRotation (lastMoveDirection);//si esta en el aire se aplica la ultima direccion de movimiento para que al saltar en el sitio no se ponga de espaldas a la camara
		}
		
		rot = Quaternion.Slerp (rot,toTarget,rotateSpeed*Time.deltaTime); //de aqui para abajo se calcula y aplica la rotacion.
		Vector3 euler = rot.eulerAngles;
		euler.z = 0;
		euler.x = 0;
		rot = Quaternion.Euler (euler);
		
		transform.rotation = rot;
	}
	void CheckSpeed (Vector3 direccion)//chequeamos la inclinacion de los sticks para determinar si andamos o corremos
	{
		if(direccion.magnitude<=0.2)
		{
			speed=0F;
		}
		else if(direccion.magnitude<=0.9)
		{
			speed = walkSpeed;
		}
		else
		{
			speed=runSpeed;
		}
	}
	void CheckFloor()//chequeamos el suelo constantemente.
	{
		RaycastHit hitInfo;
		Physics.Raycast(transform.position,Vector3.down,out hitInfo);//se lanza un rayo hacia abajo
		normalDirection=-hitInfo.normal;//recogemos la inclinacion de la normal del objeto con el que chocamos por si hace falta

		//Debug.Log (hitInfo.distance);
		if(hitInfo.distance<1.2F)//si la distancia del suelo es menor
		{
			slideDirection=new Vector3(hitInfo.normal.x,0,hitInfo.normal.z);//alamacenamos la direccion de deslizamiento constantemente
			slideDirection=Vector3.ProjectOnPlane(slideDirection,normalDirection);
			if(!saltando)//este if es para que raygrounded no entre en juego al desactivar el salto, ya que al ser mas largo que collider del personaje, lo desactivaria inmediatamente al saltar.
			{
				if (!rayGrounded)//ponemos raygrounded a true
				{
					rayGrounded=true;
				}
		
				if (hitInfo.collider.tag=="Slide")//comprobamos si el suelo es deslizante
				{
					if (!sliding)
					{
						sliding=true;
				
					}
				}
				else if (sliding)//si no es deslizante y esta activo lo desactivamos
				{
					sliding=false;
				}
				if (hitInfo.collider.tag=="Surf")//igual con surf
				{
					if (!surfing)
					{
						surfing=true;
					}
				}
				else if (surfing)
				{
					surfing=false;
				}
			}
		}		
		else //si la distancia es mayor... desactivamos raygrounded y desactivamos sliding. Surf no se desactiva aki por que tendra la opcion de salto y no quiero que acabe hasta que se acabe la secuencia completa de deslizamiento.
		{
			if (rayGrounded)
			{
				rayGrounded=false;
				//sliding=false;
			}
			
		}
		if(controller.isGrounded)//Esto solo sirve para visualizar isGrounded del character controller.
		{
			if (!grounded)
			{
				grounded=true;
			}
			
		}
		else if (grounded)
		{
			grounded=false;
		}
		
			
		
		
	}
	void OnTriggerEnter(Collider other)//como OnTriggerEnter es de doble direccion chequeamos en que triger entramos para evitar poner codigos en algunos objetos como en los "ObjSurfDirection"
	{
		if (other.gameObject.tag=="Surf")//si el trigger es de un surfObject...
		{
			surfDirection=other.transform.parent.transform.forward;//se asigna a surfDirection el vector al que apunta.
		}
	}
	
	
}
