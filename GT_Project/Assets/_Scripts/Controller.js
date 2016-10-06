#pragma strict

var moveUpX : KeyCode;
var moveDownX : KeyCode;
var moveUpZ : KeyCode;
var moveDownZ : KeyCode;
var space : KeyCode;

//var speed : float = 100;
var positionX : float = 3.0;
var positionZ : float = 3.0;
var increment : float = 1.0;
var maxPos : float = 3;
var aSources = GetComponents(AudioSource);

function Start () {
	
}

function Update () {	
	transform.rotation.x = 0;
	transform.rotation.y = 0;
	transform.rotation.z = 0;

	if (Input.GetKeyUp(moveUpX))
	{
		if (transform.position.x < maxPos)
		{
			//rigidbody.velocity.x = speed;
			positionX = positionX + increment;
			transform.position.x = positionX;
			GetComponent.<AudioSource>().Play();
			
		}
	}
	else if (Input.GetKeyUp(moveDownX))
	{
		if (transform.position.x > (maxPos*-1))
		{
			positionX = positionX - increment;
			transform.position.x = positionX ;
			GetComponent.<AudioSource>().Play();
		}
	}
	
	else if (Input.GetKeyUp(moveUpZ))
	{
		if (transform.position.z < maxPos)
		{
			positionZ = positionZ + increment;
			transform.position.z = positionZ;
			GetComponent.<AudioSource>().Play();
		}
	}
	else if (Input.GetKeyUp(moveDownZ))
	{
		if (transform.position.z > (maxPos*-1))
		{
			positionZ = positionZ - increment;
			transform.position.z = positionZ;
			GetComponent.<AudioSource>().Play();
		}
	}
}	

function FixedUpdate(){
}