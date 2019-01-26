using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController controller;


	public GameObject tile;


	public Vector2 gridSize;

	public TileController[,] tc;



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
    	
		for (int i=0; i<gridSize.x; i++) {
			for (int j=0; j<gridSize.y; j++) {
				GameObject go = Instantiate(tile);
				TileController cont = go.GetComponent<TileController>();
				cont.loc.x = i;
				cont.loc.y = j;
			}
		}


    }

    // Update is called once per frame
    void Update() {
        
    }
}
