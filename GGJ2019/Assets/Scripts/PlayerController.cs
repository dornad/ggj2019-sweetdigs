using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 3;

    public Vector3 position;

    public Vector3 startPosition;

    private bool isMoving = false;

    public int itemType = 0; 

    public GameObject[] items;


    // Start is called before the first frame update
    void Start() {
        // Store reference to attached controller
        position = startPosition;
    }

    // Update is called once per frame
    void Update() {
        if (!isMoving) {
            changePosition();
            if (Input.GetKeyDown("e")){
                dropItem();
            }
        }
        this.transform.position = position;

    }

    // public void move() {
    //     // NEXT: Vector2.Lerp + time.deltaTime
    //     this.transform.position = new Vector3(position.x, position.y, 0);
    // }

    public void changePosition() {

        /*
            make (static public) tilecontroller accessible to PlayerController.
            find "next" tile
            change move speed according to type of tile.
            if fn in tile --> move player 
            pick up hat
         */

        TileController[,] tiles = GameController.tcArray;

        int toX = 0;
        int toY = 0;

        if (Input.GetKeyDown("w")){ // || Input.GetKeyDown(KeyCode.Joystick1Button0)){ //|| Input.GetAxis("Vertical") == -1) {
            toY = 1;
        }
        else if (Input.GetKeyDown("a")){ //|| Input.GetAxis("Horizontal") == -1) {
            toX = -1;
        }
        else if (Input.GetKeyDown("s")){ //|| Input.GetAxis("Vertical") == 1) {
            toY = -1;
        }
        else if (Input.GetKeyDown("d")){ //|| Input.GetAxis("Horizontal") == 1) {
            toX = 1;            
        }

        if (Mathf.Abs(toX) > 0 || Mathf.Abs(toY) > 0) {
            Vector2Int toPosition = new Vector2Int((int)position.x + toX, (int) position.y + toY);
            
            if (toPosition.x >= 0 && toPosition.y >=0 && toPosition.y < GameController.rows && toPosition.x < GameController.columns) {
                
                TileController tc = tiles[toPosition.x, toPosition.y];            
                if (tc.tileType != Globals.ROCK && (!this.hasItem() || !tc.hasItem() )) {
                    float speed = findSpeedMultipler(tc);
                    StartCoroutine(Move(tc, (int)toPosition.x, (int)toPosition.y, speed));
                }
            }
        }        
    }

    private float findSpeedMultipler(TileController tc) {
        switch (tc.tileType) {
            case Globals.DIRT:
                return this.moveSpeed * 0.5f;
            case Globals.TUNNEL:
                return this.moveSpeed;
            case Globals.TOUGH_DIRT:
                return this.moveSpeed * 0.25f;             
            default:
                return this.moveSpeed;
        }        
    }

    public IEnumerator Move(TileController tC, float x, float y, float speed) {
        isMoving = true;
        Vector3 startMovePosition = new Vector3(position.x, position.y, 0);
        Vector3 endMovePosition = new Vector3(x, y, 0);        
        bool hasUpdated = false;

        for (float i =0; i<1; i += speed * Time.deltaTime) {
            position = Vector3.Lerp(startMovePosition, endMovePosition, i);
            
            if (i > 0.65 && !hasUpdated) {
                hasUpdated = true;                
                tC.updateTile(this);
            }

            yield return null;
        }
        position = endMovePosition;
        isMoving = false;
    }

    public void setItemType(int newItemType) {

        for (int i=0; i<items.Length; i++) {
            items[i].SetActive(false);
        }
    
        if (newItemType > 0 && newItemType - 1 < items.Length) {
            items[newItemType-1].SetActive(true);
        }

        this.itemType = newItemType;
    }

    public bool hasItem(){ 
        return this.itemType > 0;
    }

    public void dropItem(){

        TileController[,] tiles = GameController.tcArray;


        Vector2Int dropPos = new Vector2Int((int)position.x, (int) position.y - 1);
        
        if (dropPos.x >= 0 && dropPos.y >=0 && dropPos.y < GameController.rows && dropPos.x < GameController.columns) {
            
            TileController tc = tiles[dropPos.x, dropPos.y];            
            if (tc.tileType == Globals.TUNNEL && !tc.hasItem() ) {
                tc.putItem(this.itemType);
                this.setItemType(0);
            }
        }
        
    }


    public void die() {
        // TODO

    }
}
