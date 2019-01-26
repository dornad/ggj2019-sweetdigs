using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitSystem : MonoBehaviour {

	public GameObject stopPrefab;
	public GameObject routePrefab;
	public GameObject busPrefab;
	public GameObject trainPrefab;

	private Hashtable stops;
	private Hashtable routes;
	private Hashtable vehicles;

	// Making this Awake() because it gets called before Start() and we want to update the UI Dropdowns in Start
	void Awake() {
		stops = new Hashtable();
		routes = new Hashtable();
		vehicles = new Hashtable();

		// Add in any routes or stops to the hashtables that are already in the scene
		GameObject[] stopObjs = GameObject.FindGameObjectsWithTag("Stop");
		for (int i = 0; i < stopObjs.Length; i++) {
			StopController stop = stopObjs[i].GetComponent<StopController>();
			stops.Add(stop.GetStopName(), stop);
		}
		GameObject[] routeObjs = GameObject.FindGameObjectsWithTag("Route");
		for (int i = 0; i < routeObjs.Length; i++) {
			RouteController route = routeObjs[i].GetComponent<RouteController>();
			routes.Add(route.GetRouteName(), route);
		}
		GameObject[] vehicleObj = GameObject.FindGameObjectsWithTag("Vehicle");
		for (int i = 0; i < vehicleObj.Length; i++) {
			VehicleController vehicle = vehicleObj[i].GetComponent<VehicleController>();
			vehicles.Add(vehicle.GetVehicleName(), vehicle);
		}
	}
	

	void FixedUpdate () {


	}


	public void CreateStop(float x, float y, string name, float rps) {
		GameObject stopObj = Instantiate(stopPrefab, new Vector3(), Quaternion.identity);
		StopController stop = stopObj.GetComponent<StopController>();
		stop.Init(x, y, name, rps);
		stops.Add(name, stop);
	}

	public void CreateRoute(string name, Color col, bool isBusRoute) {
		GameObject routeObj = Instantiate(routePrefab, new Vector3(), Quaternion.identity);
		RouteController route = routeObj.GetComponent<RouteController>();
		route.Init(name, col, isBusRoute);
		routes.Add(name, route);
	}

	public void AppendStopToRoute(string stopName, string routeName, float timeFromLastStop, float timeToFirstStop) {
		StopController stop = (StopController)stops[stopName];
		RouteController route = (RouteController)routes[routeName];
		route.AppendStop(stop, timeFromLastStop, timeToFirstStop);
	}

	public void CreateVehicle(string vehicleName, int maxRiders, int startTime, int endTime, string routeName, bool isBus) {
		GameObject vehicleObj = Instantiate(isBus ? busPrefab : trainPrefab, new Vector3(), Quaternion.identity);
		VehicleController vehicle = vehicleObj.GetComponent<VehicleController>();
		vehicle.Init(vehicleName, maxRiders, startTime, endTime, (RouteController)routes[routeName], isBus);
		vehicles.Add(vehicleName, vehicle);
	}

	public string[] GetStopNames() {
		string[] keys = new string[stops.Keys.Count];
		stops.Keys.CopyTo(keys, 0);
		return keys;
	}

	public string[] GetRouteNames() {
		string[] keys = new string[routes.Keys.Count];
		routes.Keys.CopyTo(keys, 0);
		return keys;
	}

	public string[] GetVehicleNames() {
		string[] keys = new string[vehicles.Keys.Count];
		vehicles.Keys.CopyTo(keys, 0);
		return keys;
	}

	public string[] GetBusTrainRouteNames(bool forBusRoutes) {
		RouteController[] allRoutes = new RouteController[routes.Values.Count];
		routes.Values.CopyTo(allRoutes, 0);
		int rs = 0;
		for (int i = 0; i < allRoutes.Length; i++) {
			if (allRoutes[i].IsBusRoute() == forBusRoutes) {
				rs++;
			}
		}
		string[] rNames = new string[rs];
		for (int i = 0, j = 0; i < allRoutes.Length; i++) {
			if (allRoutes[i].IsBusRoute() == forBusRoutes) {
				rNames[j] = allRoutes[i].GetRouteName();
				j++;
			}
		}
		return rNames;
	}
}
