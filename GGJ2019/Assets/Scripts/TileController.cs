﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

	public static TileController tileController;
    
	public int type=0;

	public Sprite[] sprites;

	public bool visible = true;

	public Vector2 loc;

	public int tileType;

	public float r = 0;
	public float g = 0;
	public float b = 0;


	private SpriteRenderer sr;

	// Start is called before the first frame update
    void Start() {
		sr = GetComponent<SpriteRenderer>();
		parseType();
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

	private void parseType() {
		tileType = type % 10;

		// try {
		// 	sr.sprite = sprites[tileType];
		// }
		// catch {

		// }

		if (tileType == Globals.DIRT) {
			sr.color = Color.grey;
		}
		else if (tileType == Globals.TUNNEL) {
			sr.color = Color.yellow;
		}
		else if (tileType == Globals.ROCK) {
			sr.color = Color.cyan;
		}
		else if (tileType == Globals.TOUGH_DIRT) {
			sr.color = Color.red;
		}
		else if (tileType == 4) {
			sr.color = Color.magenta;
		}
		else {
			sr.color = Color.magenta;
		}
	}
}
