#pragma strict

var space : KeyCode;

function Start () {

}

function Update () {
	if (Input.GetKeyUp(space)){
		GetComponent.<AudioSource>().Play();
	}
}