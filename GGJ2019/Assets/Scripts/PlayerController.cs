using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 1;

    public Vector3 position;

    private bool isMoving;

    // Start is called before the first frame update
    void Start() {
        // Store reference to attached controller
        position = new Vector3(2,2,0);
    }

    // Update is called once per frame
    void Update() {
        if (!isMoving) {
            changePosition();
        }
        this.transform.position = new Vector3(position.x, position.y, 0);

    }

    // public void move() {
    //     // NEXT: Vector2.Lerp + time.deltaTime
    //     this.transform.position = new Vector3(position.x, position.y, 0);
    // }

    public void changePosition() {
        if (Input.GetKeyDown("w")) {
            StartCoroutine(Move((int)position.x, (int)position.y+1, this.moveSpeed));
        }
        else if (Input.GetKeyDown("a")) {
            StartCoroutine(Move((int)position.x-1, (int)position.y, this.moveSpeed));
        }
        else if (Input.GetKeyDown("s")) {
            StartCoroutine(Move((int)position.x, (int)position.y-1, this.moveSpeed));
        }
        else if (Input.GetKeyDown("d")) {
            StartCoroutine(Move((int)position.x+1, (int)position.y, this.moveSpeed));
        }
    }

    public IEnumerator Move(float x, float y, float speed) {
        isMoving = true;
        Vector3 startPosition = new Vector3(position.x, position.y, 0);
        Vector3 endPosition = new Vector3(x, y, 0);
        // float pos =0;
        for (float i =0; i<1; i += speed * Time.deltaTime) {
            position = Vector3.Lerp(startPosition, endPosition, i);
            yield return null;
        }
        position = endPosition;
        isMoving = false;

    }
}
