using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

	public const int NUM_TYPES = 3;

	public static TileController tileController;
    
	public int type=0;

	public Sprite[] sprites;

	public bool visible = true;

	public Vector2 loc;

	public int tileType;

	public float r = 0;
	public float g = 0;
	public float b = 0;

	public GameObject[] items;

	public int itemType;


	private SpriteRenderer sr;

	// Start is called before the first frame update
    void Start() {
			sr = GetComponent<SpriteRenderer>();
			this.tileType = type % 10;
			if (this.tileType > NUM_TYPES){
				this.tileType = NUM_TYPES;
			}
			applyTileType();
			
			this.itemType = (int)(type/10);
			if ( this.itemType > items.Length){
				this.itemType = items.Length; 
			}
			applyItemType();
    }

    // Update is called once per frame
    void Update() {

		this.transform.position = new Vector3(loc.x, loc.y, 0);


		//sr.enabled = visible;
		/*
		if (!visible) {
			sr.Color = Color.transparant;
		} else {
			sr.Color = Color.red;
		}
		*/
    }

	private void applyTileType() {
		if (tileType == Globals.DIRT) {
			sr.color = Color.clear;
			sr.sortingOrder = 0;
		}
		else if (tileType == Globals.TUNNEL) {
			sr.sprite = sprites[1];
			sr.color = Color.white;
			sr.sortingOrder = 1;
		}
		else if (tileType == Globals.ROCK) {
			sr.sprite = sprites[2];
			sr.color = Color.white;
			sr.sortingOrder = 2;
		}
		else if (tileType == Globals.TOUGH_DIRT) {
			sr.sprite = sprites[3];
			sr.color = Color.white;
			sr.sortingOrder = 0;
		}
		else if (tileType == 4) {
			sr.sprite = sprites[0];
			sr.color = Color.magenta;
			sr.sortingOrder = 2;
		}
		else {
			sr.sprite = sprites[0];
			sr.color = Color.black;
			sr.sortingOrder = 0;
		}	
	}

	private void applyItemType() {
		
		for (int i=0; i<items.Length; i++) {
			items[i].SetActive(false);
		}
		
		if (itemType > 0 && itemType - 1 < items.Length) {
			items[itemType-1].SetActive(true);
		}
	}

	public void updateTile(PlayerController pc) {
		if (this.tileType == Globals.DIRT || this.tileType == Globals.TOUGH_DIRT) {
			// dirt || tought_dirt -> tunnel
			this.tileType = Globals.TUNNEL;
			applyTileType();
		} 
		if (this.itemType > 0){
			pc.setItemType(this.itemType);

			for (int i=0; i< GameController.itemLocationsScores.Count; i++) {
				Vector4 item = GameController.itemLocationsScores[i];
				if ((int)item.x == (int)loc.x && (int)item.y == (int)loc.y) {
					GameController.itemLocationsScores.RemoveAt(i);
					break;
				}
			}

			this.itemType = 0;
			applyItemType();
			
		}
	}
	public bool hasItem(){
		return this.itemType > 0;
	}

	public void putItem(int newItemType) {
		this.itemType = newItemType;
		applyItemType();

	}
}
