using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
    
	public bool visibile = true;

	private SpriteRenderer sr;

	// Start is called before the first frame update
    void Start() {
		sr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
