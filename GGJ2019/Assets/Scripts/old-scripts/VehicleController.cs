using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour {

	public int maxRiders = 45;

	private Hashtable riders;

	public string vehicleName;
	public int onStop;

	public int serviceStartTime = 0;
	public int serviceEndTime = 24;

	public RouteController onRoute;

	// A utility boolean that would not let a user accidentally add a bus to a train route or vice versa
	public bool isBus;

	// This will be the lerp value to display it on screen
	// Should go from 0 to 1
	private float locationBetweenStops;

	private SpriteRenderer sr;
	private Collider2D col;

	public void Init(string iden, int maxR, int startTime, int endTime, RouteController onR, bool isB) {
		maxRiders = maxR;
		vehicleName = iden;
		onRoute = onR;
		isBus = isB;
		onStop = 0;
		serviceStartTime = startTime;
		serviceEndTime = endTime;
	}

	void Start() {
		riders = new Hashtable();
		sr = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
	}

	// This is for debugging only
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space))
			LogInfo();
	}



	// FixedUpdate gets called every .2 seconds
	void FixedUpdate () {


		// Increase the percentage between the stops
		// Only increase the percentage if the vehicle is in service
		if (IsInService() || (sr.color != Color.clear))
			locationBetweenStops += (1 / onRoute.GetTimeToNextStop(onStop)) * Time.fixedDeltaTime;


		// If it's reached its destination increase the stop and exchange passengers with new stop and reset locationBetweenStops
		if (locationBetweenStops >= 1) {

			// If the vehicle is out of service and at the end of the route, it will dump all the passengers in it.
			// It will also turn invisible and unselectable
			if (!IsInService() && (onStop >= onRoute.stops.Length - 2)) {

				if (onStop == onRoute.stops.Length - 1) {
					DumpAllPassengers(onRoute.GetStopsOnRoute()[0]);
				}
				else {
					onStop = onRoute.stops.Length - 1;
					DumpAllPassengers(onRoute.GetStopsOnRoute()[onStop]);
				}
					
				// get ready to start when it goes back in service again
				locationBetweenStops = .99f;
				// Turn inbisible and unselectable
				// It will be visibe again by the heatmap controller when it goes into service
				col.enabled = false;
				sr.color = Color.clear;
			}
			else {
				col.enabled = true;
				onStop = onStop < onRoute.stops.Length - 1 ? onStop + 1 : 0;
				ExchangePassengers(onRoute.stops[onStop]);
				locationBetweenStops = 0;
			}
		}

		// Get where we're going
		int vehicleGoingTo = onStop < onRoute.stops.Length - 1 ? onStop + 1 : 0;

		// Set the visual position of the bus the percentage between the positions of the stops
		// If you don't know Lerp (linear interpolation), look into it.  It's pretty useful
		transform.position = Vector3.Lerp(onRoute.stops[onStop].transform.position, onRoute.stops[vehicleGoingTo].transform.position, locationBetweenStops);

	}


	private void ExchangePassengers(StopController stop) {

		// First drop off all passengers going to this stop
		if (riders.ContainsKey(stop.GetStopName())) {
			riders[stop.GetStopName()] = 0;	
		}

		// Next load as many riders onto the stop as possible
		string[] ridersWaitingForStops = stop.GetStopsBeingWaitedFor();
		for (int i = 0; i < ridersWaitingForStops.Length; i++) {
			string destination = ridersWaitingForStops[i];
			if (onRoute.IsStopOnRoute(destination)) {
				int ridersToGetOn = stop.FindRidersForStop(destination);
				int currentRiders = GetNumRiders();
				if (ridersToGetOn + currentRiders > GetMaxRiders()) {
					ridersToGetOn = GetMaxRiders() - currentRiders;
				}
				AddRidersForStop(destination, ridersToGetOn);
				stop.RemoveRidersForStop(destination, ridersToGetOn);
			}
		}
	}

	private void DumpAllPassengers(StopController stop) {
		string[] keys = new string[riders.Keys.Count];
		riders.Keys.CopyTo(keys, 0);
		for (int i = 0; i < keys.Length; i++) {
			stop.AddRidersForStop(keys[i], (int)riders[keys[i]]);
			riders[keys[i]] = 0;
		}
	}

	public int GetRiderGoingToStop(string stop) {
		if (riders.ContainsKey(stop))
			return (int)riders[stop];
		else
			return 0;
	}

	public int GetMaxRiders() {
		return maxRiders;
	}

	public int GetNumRiders() {
		string[] keys = new string[riders.Keys.Count];
		riders.Keys.CopyTo(keys, 0);
		int numRiders = 0;
		for (int i = 0; i < keys.Length; i++) {
			numRiders += (int)riders[keys[i]];
		}
		return numRiders;
	}

	public void AddRidersForStop(string stop, int numRiders) {
		if (riders.ContainsKey(stop)) {
			int currentRiders = (int)riders[stop];
			currentRiders += numRiders;
			riders[stop] = currentRiders;
		}
		else {
			riders.Add(stop, numRiders);
		}
	}

	public void RemoveRidersForStop(string stop, int numRiders) {
		int currentRiders = (int)riders[stop];
		currentRiders -= numRiders;
		if (currentRiders < 0)
			currentRiders = 0;
		riders[stop] = currentRiders;
	}

	public string GetVehicleName() {
		return vehicleName;
	}

	public int GetOnStop() {
		return onStop;
	}

	public RouteController GetOnRoute() {
		return onRoute;
	}

	public string GetRiderInfo() {
		string ridersString = "";
		string[] keys = new string[riders.Keys.Count];
		riders.Keys.CopyTo(keys, 0);
		for (int i = 0; i < keys.Length; i++) {
			ridersString += string.Format("{0}-- {1}    ", keys[i],(int)riders[keys[i]]);
		}
		return ridersString;
	}

	public bool IsInService() {
		// In order to have overnight running buses, we can set the service start time to be later than the service end time
		if (serviceStartTime < serviceEndTime) {
			return serviceStartTime <= WeekTime.hour && WeekTime.hour < serviceEndTime;
		}
		else {
			return WeekTime.hour <= serviceEndTime || WeekTime.hour >= serviceStartTime;
		}
	}

	private void LogInfo() {
		print("Vehicle " + GetVehicleName() + ",    Riders: " + GetNumRiders());
	}

}
