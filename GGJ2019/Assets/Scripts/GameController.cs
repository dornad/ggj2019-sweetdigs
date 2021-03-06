﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

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

	public GameObject gameOverCanvas;
	public GameObject LeaderBoard;
	public GameObject ScoreInput;
	public GameObject LoadingCanvas;
	public InputField nameInput;

	public static bool playerDied = false;	

	// Kinda odd that I use a Vector4, but x, y are for position of item, z is for if it has been touched, and w is for item type
	public static List<Vector4> itemLocationsScores;

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
		GameController.itemLocationsScores = new List<Vector4>();
	}

    // Start is called before the first frame update
    void Start() {
        GameController.score = 0;
        GameController.playerDied = false;
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

					int itemType = (int)(cont.type/10);
					if (itemType > 0)  {
						itemLocationsScores.Add(new Vector4(cont.loc.x, cont.loc.y, 0, itemType));
					}

				}
			}
            //Bring in the... Kill Zoooone!
            GameController.killzone = Instantiate(killzonePrefab).GetComponent<KillzoneController>();
            GameController.killzone.transform.position = new Vector3(0, 7.5f, GameController.killzone.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update() {
        //Death Check
		if (!GameController.playerDied) {
			GameController.score = this.calculateScore();
		}

        if (pc.position.x <= killzone.killColumn && !GameController.playerDied) {
			playerDied = true;
            loseGame();
        }

		if (Input.GetKeyDown("p")) {
			playerDied = true;
			loseGame();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadScene(0);
		}

		if (nameInput != null) {
			GameController.UserID = nameInput.text;	
		}		
    }

	private int calculateScore() {
		int score =0;
		for (int i=0; i<GameController.itemLocationsScores.Count; i++) {
			Vector4 item = itemLocationsScores[i];
			// Only calculate the score on the item if it hasn't been passed
			if (GameController.killzone.killColumn < item.x) {
				// Only calculate if it's been touched
				Vector2 playerPos = new Vector2(this.pc.transform.position.x, this.pc.transform.position.y);
				
				if (item.z > 0) {
					if (item.w == 1) {
						score++;
					}
					else if (item.w == 2) {
						score++;
					}
					else if (item.w == 3) {
						score++;
					}
				}

			}
		}

		if (pc.hasItem()) {
			if (pc.itemType == 1) {
				score++;
			}
			else if (pc.itemType == 2) {
				score++;
			}
			else if (pc.itemType == 3) {
				score++;
			}
		}

		return score;
	}

    public void loseGame() {
        // Do other stuff
        pc.die();
        gameOverCanvas.SetActive(true);
		
    }

	private void getScoreLeaderboard() {
		playfabClient.GetScoreLeaderboard(result => { 
			this.scoreBoard = result;
			// TODO: Use the scoreboard (present it)
			LoadingCanvas.SetActive(false);
			LeaderBoard.SetActive(true);
		});
	}

	public void submitScorePlayfab() {
		// send the username
		if (GameController.UserID.Length > 3) {
			ScoreInput.SetActive(false);
			LoadingCanvas.SetActive(true);
			playfabClient.UpdateDisplayName(GameController.UserID, result => {
				// send the new score to PlayFab
				playfabClient.SubmitScore(score, submittedScore => {
					if (submittedScore) {
						this.getScoreLeaderboard();
					}
					else {
						print("Error submitting Score");
					} 
				});		
			});				
		}
	}
}
