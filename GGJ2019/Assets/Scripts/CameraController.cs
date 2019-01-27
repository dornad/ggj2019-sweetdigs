
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Camera cam;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

		// if (Input.mouseScrollDelta.y > 0) {
		// 	cam.orthographicSize++;
		// }
		//     else if (Input.mouseScrollDelta.y < 0 && cam.orthographicSize > 1) {
		// 	    cam.orthographicSize--;
        // }
        
        //follow player
        transform.position = new Vector3(FindObjectOfType<PlayerController>().transform.position.x, transform.position.y, transform.position.z);
    
	}
}
