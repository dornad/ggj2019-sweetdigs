using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteController : MonoBehaviour {

	public string routeName;
	public Color routeColor;

	public StopController[] stops;
	public float[] timeBetweenStops;

	private Hashtable vehicles;

	// A utility boolean that would not let a user accidentally add a bus to a train route or vice versa
	public bool isBusRoute;

	private LineRenderer lineRend;

	public void Init(string nom, Color col, bool isBus) {
		routeName = nom;
		routeColor = col;
		isBusRoute = isBus;
		if (lineRend == null) {
			lineRend = GetComponent<LineRenderer>();
			UpdateLineRenderer();
		}
	}

	void Start() {
		if (lineRend == null) {
			lineRend = GetComponent<LineRenderer>();
			UpdateLineRenderer();
		}

		vehicles = new Hashtable();

		// The following is for pre-made scenes that already have stops and routes assigned
		GameObject[] vehicleObjs = GameObject.FindGameObjectsWithTag("Vehicle");
		for (int i = 0; i < vehicleObjs.Length; i++) {
			VehicleController vehicle = vehicleObjs[i].GetComponent<VehicleController>();
			if (vehicle.onRoute == this) {
				vehicles.Add(vehicle.GetVehicleName(), vehicle);
			}
		}

		for (int i = 0; i < stops.Length; i++) {
			stops[i].addRouteToList(this);
		}
	}
		


	public void  AppendStop(StopController stop, float timeFromLast, float timeToStart) {
		if (stops.Length == 0) {
			stops = new StopController[] {stop};
			// The time doesn't really make sense if a route only has one stop
			timeBetweenStops = new float[] { 1 };
		}
		else {
			// Put the stop in the array
			StopController[] newStops = new StopController[stops.Length + 1];
			stops.CopyTo(newStops, 0);
			newStops[newStops.Length - 1] = stop;
			stops = newStops;

			// Update The times
			float[] newTimes = new float[timeBetweenStops.Length + 1];
			timeBetweenStops.CopyTo(newTimes, 0);
			newTimes[newTimes.Length - 2] = timeFromLast;
			newTimes[newTimes.Length - 1] = timeToStart;
			timeBetweenStops = newTimes;

			UpdateLineRenderer();
		}
		stop.addRouteToList(this);
	}



	public void AddVehicleToRoute() {
		// Should include a check if Route has more than one stop
	}

	public float GetTimeToNextStop(int onStop) {
		return timeBetweenStops[onStop];
	}

	public string GetRouteName() {
		return routeName;
	}

	public bool IsBusRoute() {
		return isBusRoute;
	}

	public StopController[] GetStopsOnRoute() {
		return stops;
	}

	public bool IsStopOnRoute(string stopName) {
		for (int i = 0; i < stops.Length; i++) {
			if (stops[i].GetStopName() == stopName)
				return true;
		}
		return false;
	}

	private void UpdateLineRenderer() {
		lineRend.startColor = routeColor;
		lineRend.endColor = routeColor;

		// If we have a pre-built route in the scene, we should setup its renderer as well
		if (stops.Length > 1) {
			Vector3[] lineVerticies = new Vector3[stops.Length];
			for (int i = 0; i < stops.Length; i++) {
				lineVerticies[i] = stops[i].transform.position;
			}
			lineRend.positionCount = stops.Length;
			lineRend.SetPositions(lineVerticies);
		}
	}


}
