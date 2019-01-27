using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LeaderboardDisplay : MonoBehaviour
{

    public GameController gc;

    private Text text;

    // Start is called before the first frame update
    void Start() {
        text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string scoreText = "";
        List<GameController.ScoreBoardEntry> scores = gc.scoreBoard;
        for (int i=0; i<scores.Count; i++) {
            GameController.ScoreBoardEntry score = scores[i];
            scoreText += (score.name + ": " + score.score + "\n");
        }
        

        text.text = scoreText;
    }
}
