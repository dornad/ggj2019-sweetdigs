using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController controller;

	public GameObject tile;

	public Vector2 gridSize;

	public string levelFile;

	public TileController[,] tcArray;



	void Awake() {
		if (GameController.controller == null) {
			GameController.controller = this;
		}
		else {
			//Destroy if it exists already.
			//GameObject.Destroy(this.gameObject);
		}
	}

    // Start is called before the first frame update
    void Start() {
		TextAsset level = (TextAsset)Resources.Load(levelFile);
		if (level == null) {
			/*
			Debug.LogError("Error (LoadPlayBoard): 'Using Specific Game Board' is checked but the file '" + specificGridFileName + "' doesn't exist in the Resources Folder -- Filling Board Randomly");
			SetUpGridEdgePieces(true);
			FillHalfBoardRandom();
			*/
			tcArray = new TileController[(int)gridSize.x, (int)gridSize.y]; 
			for (int i=0; i<gridSize.x; i++) {
				for (int j=0; j<gridSize.y; j++) {
					GameObject go = Instantiate(tile);
					TileController cont = go.GetComponent<TileController>();
					tcArray[i,j]= cont;
					cont.loc.x = i;
					cont.loc.y = j;
				}
			}
		}
		else {
			string boardString = boardFile.text.Trim();
			char[] lineDelimiter = { '\n' };
			string[] rowStrings = boardString.Split(lineDelimiter);
			int rows = rowStrings.length;
			char[] columnDelimiter = { ',' };
			int columns = boardString.Split(columnDelimiter);
			for (i=0; i<rows; i++) {


			}

		}
    }

    // Update is called once per frame
    void Update() {
        
    }
}
