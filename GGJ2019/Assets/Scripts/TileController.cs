using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
    
	public bool visible = true;

	public Vector2 loc;

	private SpriteRenderer sr;

	// Start is called before the first frame update
    void Start() {
		sr = GetComponent<SpriteRenderer>();
        

    }

    // Update is called once per frame
    void Update() {

		this.transform.position = new Vector3(loc.x, loc.y, 0);


		//sr.enabled = visible;
		/*
		if (!visible) {
			sr.Color = Color.transparant;
		} else {
			sr.Color = Color.red;
		}
		*/
    }
}
