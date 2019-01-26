using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleMovement : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        // Store reference to attached controller
       
    }

    // Update is called once per frame
    void Update() {
      float wobble =  Random.Range(-9,9);
      transform.eulerAngles = new Vector3(0, 0, wobble);
    }

}
