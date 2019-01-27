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
			sr.color = Color.grey;
		}
		else if (tileType == Globals.TUNNEL) {
			sr.color = Color.yellow;
		}
		else if (tileType == Globals.ROCK) {
			sr.color = Color.cyan;
		}
		else if (tileType == Globals.TOUGH_DIRT) {
			sr.color = Color.red;
		}
		else if (tileType == 4) {
			sr.color = Color.magenta;
		}
		else {
			sr.color = Color.black;
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
			this.itemType = 0;
			applyItemType();
			
		}
	}
	public bool hasItem(){
		return this.itemType > 0;
	}

	public void dropItem(int itemNumber) {

	}
}
