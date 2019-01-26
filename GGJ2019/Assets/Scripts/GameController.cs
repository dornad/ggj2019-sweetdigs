using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController controller;


	public GameObject tile;

	public TileController[,] tr;



	void Awake() {
		if (GameController.controller == null) {
			GameController.controller = this;
		}
		else {
			// Destroy if it exists already.
			// this.gameObject;
		}
	}

    // Start is called before the first frame update
    void Start() {
    	


    }

    // Update is called once per frame
    void Update() {
        
    }
}
