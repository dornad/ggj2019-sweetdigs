using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 position;

    // Start is called before the first frame update
    void Start() {
        // Store reference to attached controller
        position = new Vector2(2,2);
    }

    // Update is called once per frame
    void Update() {
        changePosition();
        move();
    }

    public void move() {
        // NEXT: Vector2.Lerp + time.deltaTime
        this.transform.position = new Vector3(position.x, position.y, 0);
    }

    public void changePosition() {
        if (Input.GetKeyDown("w")) {
            position.y++;
        }
        else if (Input.GetKeyDown("a")) {
            position.x--;
        }
        else if (Input.GetKeyDown("s")) {
            position.y--;
        }
        else if (Input.GetKeyDown("d")) {
            position.x++;
        }
    }
}
