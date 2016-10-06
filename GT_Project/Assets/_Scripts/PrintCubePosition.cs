using UnityEngine;
using System.Collections;

public class PrintCubePosition : MonoBehaviour {

	int cpt = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<TextMesh>().text = "position X =" 
            + GameObject.Find("CubeMain").transform.position.x + 
            "\n position Z =" + GameObject.Find("CubeMain").transform.position.z;
        // + "\n cubearray =" + GameObject.Find("CubeMain");
	}
}
