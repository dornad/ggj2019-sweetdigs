using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
    
	public bool visible = true;

	public Vector2 loc;

	public float r = 0;
	public float g = 0;
	public float b = 0;


	private SpriteRenderer sr;

	// Start is called before the first frame update
    void Start() {
		sr = GetComponent<SpriteRenderer>();
		r = Random.Range((float)0, (float)1);
		g = Random.Range((float)0, (float)1);
		b = Random.Range((float)0, (float)1);
		sr.color = new Color(r,g, b, 1);
		// sr.color = Color.blue;

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
