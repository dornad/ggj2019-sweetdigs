using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapController : MonoBehaviour {

	private SpriteRenderer rend;

	private VehicleController vc;
	private StopController sc;

	private int whiteLimit = 10;
	private int yellowLimit = 20;
	private int redLimit = 30;

	// Use this for initialization
	void Start () {

		vc = GetComponent<VehicleController>();
		sc = GetComponent<StopController>();
		rend = GetComponent<SpriteRenderer>();
		
	}
	
	// Update is called once per frame
	void Update () {

		int riders = 0;

		if (vc != null) {
			riders = vc.GetNumRiders();
			int max = vc.GetMaxRiders();
			whiteLimit = max / 3;
			yellowLimit = max * 2 / 3;
			redLimit = max;
			if (vc.IsInService())
				SetRiderHeatmapColor(riders);
		}
		else if (sc != null) {
			riders = sc.GetTotalRiders();
			whiteLimit = 10;
			yellowLimit = 20;
			redLimit = 30;
			SetRiderHeatmapColor(riders);
		}
	}


	private void SetRiderHeatmapColor(int numRiders) {
		if (numRiders < whiteLimit)
			rend.color = Color.white;
		else if (numRiders < yellowLimit)
			rend.color = Color.Lerp(Color.white, Color.yellow, ((float)numRiders - (float)whiteLimit) / ((float)yellowLimit - (float)whiteLimit));
		else if (numRiders < redLimit)
			rend.color = Color.Lerp(Color.white, Color.red, ((float)numRiders - (float)yellowLimit) / ((float)redLimit - (float)yellowLimit));
		else
			rend.color = Color.red;
	}	

}
