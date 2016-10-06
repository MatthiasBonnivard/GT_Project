using UnityEngine;
using System.Collections;

public class ColorShape : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.color = new Color (1, 1, 60);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
