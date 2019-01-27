using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour {

    public Text text;


    // Start is called before the first frame update
    void Start() {
        this.text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        text.text = "Score: " + GameController.score;
    }
}
