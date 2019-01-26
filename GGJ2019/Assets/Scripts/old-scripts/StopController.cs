using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopController : MonoBehaviour {

	//public int ridersWaiting = 0;

	private Hashtable ridersWaiting;

	// The reason these variables are public is because it lets us interact with them in the unity editor;
	public string stopName;

	public float x;
	public float y;

	public float ridersArrivePerMinute = 2f;

	// We keep a reference of the Coroutine in order for it to serve as a lock
	// So that if we're running the sim from the editor or the stop being generated, we only run the coroutine once
	private Coroutine ridersLockout;

	private List<RouteController> routesOn;

	public void Init(float xCor, float yCor, string nom, float rps) {
		x = xCor;
		y = yCor;
		name = nom;
		stopName = nom;
		ridersArrivePerMinute = rps;

		// When we have lat and long, we'd have to adjust this;
		transform.position = new Vector3(x, y, 0);

		if (ridersLockout == null)
			ridersLockout = StartCoroutine(CreateRiders());
	}

	void Start() {
		ridersWaiting = new Hashtable();
		if (routesOn == null)
			routesOn = new List<RouteController>();

		if (ridersLockout == null)
			ridersLockout = StartCoroutine(CreateRiders());
	}

	// This is for debugging only
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space))
			LogInfo();
	}


	public string GetStopName() {
		return stopName;
	}

	public void AddRidersForStop(string stop, int numRiders) {
		if (ridersWaiting.ContainsKey(stop)) {
			int currentRiders = (int)ridersWaiting[stop];
			currentRiders += numRiders;
			ridersWaiting[stop] = currentRiders;
		}
		else {
			ridersWaiting.Add(stop, numRiders);
		}
	}

	public void RemoveRidersForStop(string stop, int numRiders) {
		if (ridersWaiting.ContainsKey(stop)) {
			int currentRiders = (int)ridersWaiting[stop];
			currentRiders -= numRiders;
			if (currentRiders < 0)
				currentRiders = 0;
			ridersWaiting[stop] = currentRiders;
		}
	}

	public int FindRidersForStop(string stop) {
		if (ridersWaiting.ContainsKey(stop))
			return (int)ridersWaiting[stop];
		else
			return 0;
	}

	public int GetTotalRiders() {
		string[] keys = new string[ridersWaiting.Keys.Count];
		ridersWaiting.Keys.CopyTo(keys, 0);
		int riders = 0;
		for (int i = 0; i < keys.Length; i++) {
			riders += (int)ridersWaiting[keys[i]];
		}
		return riders;
	}

	public string[] GetStopsBeingWaitedFor() {
		string[] keys = new string[ridersWaiting.Keys.Count];
		ridersWaiting.Keys.CopyTo(keys, 0);
		List<string> stopsWithPassegers = new List<string>();
		for (int i = 0; i < keys.Length; i++) {
			if ((int)ridersWaiting[keys[i]] > 0)
				stopsWithPassegers.Add(keys[i]);
		}
		return stopsWithPassegers.ToArray();
	}

	public void addRouteToList(RouteController r) {
		if (routesOn == null)
			routesOn = new List<RouteController>();
		if (routesOn.IndexOf(r) == -1)
			routesOn.Add(r);
	}

	public string GetRiderInfo() {
		string ridersString = "";
		string[] keys = new string[ridersWaiting.Keys.Count];
		ridersWaiting.Keys.CopyTo(keys, 0);
		for (int i = 0; i < keys.Length; i++) {
			ridersString += string.Format("{0}-- {1}    ", keys[i],(int)ridersWaiting[keys[i]]);
		}
		return ridersString;
	}
		
	// This is a coroutine so that we can have a little control over the wait time and see it more clearly.
	// Basically it just waits a few seconds and generates a new rider and does it forever.
	// The way you have to write 'yield return new WaitForSeconds()' is a bit ideosyncratic for writing a coroutine.
	private IEnumerator CreateRiders() {
		yield return new WaitForSeconds(1);
		while (true) {
			GenerateRiders(1, 0);
			yield return new WaitForSeconds(1f/(ridersArrivePerMinute * Random.Range(.7f, 1.3f)));
		}
	}


	private void GenerateRiders(int average, int range) {
		int minRiders = average - range < 0 ? 0 : average - range;
		int maxRiders = average + range;
		int newRiders = Random.Range(minRiders, maxRiders+1);

		// There is probably a better/more efficient way to do this, but oh well
		if (routesOn.Count > 0) {
			for (int i = 0; i < newRiders; i++) {
				int useRoute = Random.Range(0, routesOn.Count);
				StopController[] stops = routesOn[useRoute].GetStopsOnRoute();
				// We won't add a rider if the route chosen has only one stop (this current one)
				// The rider just won't be generated because if this stop is in a number of routes all with only this stop, it would block/break the scene.
				if (stops.Length > 2) {
					string stop = stops[Random.Range(0, stops.Length)].GetStopName();
					// We don't want to add the rider to the current stop;
					while (stop == this.stopName) {
						stop = stops[Random.Range(0, stops.Length)].GetStopName();
					}
					AddRidersForStop(stop, 1);	
				}
			}	
		}
	}

	private void LogInfo() {
		print("Stop " + GetStopName() + ",    Riders:" + GetTotalRiders());
	}
		
}
