using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	public int back = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("b")) {
			if (back == 0) {
				transform.position = new Vector3 ((float)-7.5, (float)15.5, (float)0);
//				transform.rotation = Quaternion.AngleAxis(315, Vector3.left);
				transform.rotation = Quaternion.AngleAxis(90, Vector3.right);

				back = 1;
			} else {
				transform.position = new Vector3 ((float)-7.5, (float)10.5, (float)-10.5);
				transform.rotation = Quaternion.AngleAxis(45, Vector3.right);
				back = 0;
			}
		}
	}
}
