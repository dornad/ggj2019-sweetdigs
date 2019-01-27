using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController controller;

	public static int score;
	
	public static string UserID = "EnterName";

	public GameObject tile;
    public GameObject killzonePrefab;

    public Vector2 gridSize;

	public string levelFile;

    public static KillzoneController killzone;

    public static TileController[,] tcArray;
	
	
	public static int rows;
	public static int columns;

    public PlayerController pc;
	private PlayfabClient playfabClient;

	public struct ScoreBoardEntry {
		public string name; // or unique identifier
		public int score;
		public bool isPlayer;
		public ScoreBoardEntry(string p1, int p2, bool p3 = false) {
        	name = p1;
        	score = p2;
			isPlayer = p3;
    	}
	}
	public List<ScoreBoardEntry> scoreBoard = new List<ScoreBoardEntry>();

	void Awake() {
		if (GameController.controller == null) {
			GameController.controller = this;
		}
		else {
			//Destroy if it exists already.
			//GameObject.Destroy(this.gameObject);
		}

        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		playfabClient = this.GetComponent<PlayfabClient>();
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
			// required...
			string boardString = level.text.Trim();
			char[] lineDelimiter = { '\n' };
			char[] columnDelimiter = { ',' };
			string[] rowStrings = boardString.Split(lineDelimiter);

			// Get the number of rows and columns
			int numRows = rowStrings.Length;
			int numColumns = rowStrings[0].Split(columnDelimiter).Length; // assuming data structure is not jaggy
			
			GameController.rows = numRows;
			GameController.columns = numColumns;
			// print(numRows);
			// print(numColumns);
			// START
			tcArray = new TileController[numColumns, numRows];
			for (int i = numRows-1; i >= 0; i--) {
				string row = rowStrings[numRows-(i+1)];
				string[] elements = row.Split(columnDelimiter);
				for (int j = 0; j < numColumns; j++) {
					string element = elements[j];

					GameObject go = Instantiate(tile);
					TileController cont = go.GetComponent<TileController>();
					tcArray[j,i]= cont;
					cont.loc.x = j;
					cont.loc.y = i;
					
					cont.type = int.Parse(element);
					// cont.r = Random.Range((float)0, float.Parse(element));
					// cont.g = Random.Range((float)0, float.Parse(element));
					// cont.b = Random.Range((float)0, float.Parse(element));
				}
			}
            //Bring in the... Kill Zoooone!
            killzone = Instantiate(killzonePrefab).GetComponent<KillzoneController>();
            killzone.transform.position = new Vector3(0, 7.5f, killzone.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update() {
        //Death Check
        if (pc.position.x <= killzone.killColumn)
        {
            loseGame();
        }
    }

    public void loseGame() {
        // Do other stuff
        pc.die();
        // send the new score to PlayFab
		playfabClient.SubmitScore(score, submittedScore => {
			if (submittedScore) {
				this.getScoreLeaderboard();
			} 			
		});		
    }

	private void getScoreLeaderboard() {
		playfabClient.GetScoreLeaderboard(result => { 
			this.scoreBoard = result;
			// TODO: Use the scoreboard (present it)
		});
	}
}
