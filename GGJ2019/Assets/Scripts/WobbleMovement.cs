using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleMovement : MonoBehaviour {
  public bool increasing = true;
  public float wobble = 0;

    // Start is called before the first frame update
    void Start() {
        // Store reference to attached controller
       
    }

    // Update is called once per frame
    void Update() {

    if (increasing) {
      wobble += Time.deltaTime;
      if (wobble > 1) {
        increasing = false;
      }
    } else {
      wobble -= Time.deltaTime;
      if (wobble < 0) {
        increasing = true;
      }
    }

      transform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, -15), new Vector3(0, 0, 15), wobble);
    }

}
