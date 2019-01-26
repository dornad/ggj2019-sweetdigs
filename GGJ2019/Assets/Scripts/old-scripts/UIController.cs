using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour {


	// Since we use wsad to move camera, we don't want to move when entering info;
	public static bool EnteringInfo;


	public TransitSystem transitSim;


	public Text timescaleText;

	public Text timePassedText;

	public Slider timescaleSlider;



	public InputField newStopName;
	public InputField newStopX;
	public InputField newStopY;
	public InputField newStopRPS;

	public InputField newRouteName;
	public Dropdown newRouteColor;
	public Toggle newRouteIsBus;

	public InputField newVehicleName;
	public InputField newVehicleMax;
	public InputField newVehicleStartTime;
	public InputField newVehicleEndTime;
	public Dropdown newVehicleOnRoute;
	public Toggle newVehicleIsBus;


	public Dropdown appendRouteNames;
	public Dropdown appendStopNames;
	public InputField appendStopLastTime;
	public InputField appendStopFirstTime;


	public Text infoText;
	private VehicleController selectedVehicle;
	private StopController selectedStop;

	// Use this for initialization
	void Start () {
		UpdateUILists();
	}
	
	// Update is called once per frame
	void Update () {
		timescaleText.text = "Timescale: " + Time.timeScale;
		timePassedText.text = "Time Passed: " + ((int)WeekTime.timeSinceStart).ToString() + "\n " + WeekTime.GetWeekTime();

		Time.timeScale = timescaleSlider.value;

		EnteringInfo = newStopName.isFocused || newStopX.isFocused || newStopY.isFocused;
		EnteringInfo = EnteringInfo || newRouteName.isFocused;
		EnteringInfo = EnteringInfo || newVehicleName.isFocused;

		RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
		if (Input.GetMouseButtonDown(0)) {
			selectedStop = null;
			selectedVehicle = null;
			if (hit.collider != null) {
				selectedStop = hit.collider.gameObject.GetComponent<StopController>();
				selectedVehicle = hit.collider.gameObject.GetComponent<VehicleController>();
			}
		}
		DisplaySelectedVehicleStop();
	}


	public void CreateStop() {
		// Only create the stop if all the fields are entered and has a unique name
		if (newStopX.text != "" && newStopY.text != "" && newStopName.text != "" && newStopRPS.text != "" && Array.IndexOf(transitSim.GetStopNames(), newStopName.text ) == -1) {
			// We can just do a straight parse because of the properties on the input fields limits the input to decimal values
			float x = float.Parse(newStopX.text);
			float y = float.Parse(newStopY.text);
			float rps = float.Parse(newStopRPS.text);
			string name = newStopName.text;
			transitSim.CreateStop(x, y, name, rps);
			// Clear the input fields
			newStopX.text = "";
			newStopName.text = "";
			newStopY.text = "";
			UpdateUILists();
		}
	}

	public void CreateRoute() {
		if (newRouteName.text != "" && Array.IndexOf(transitSim.GetRouteNames(), newRouteName.text ) == -1) {
			string name = newRouteName.text;
			Color col = Color.white;
			if (newRouteColor.value == 0) {
				col = Color.red;
			}
			else if (newRouteColor.value == 1) {
				col = Color.blue;
			}
			else if (newRouteColor.value == 2) {
				col = Color.green;
			}
			transitSim.CreateRoute(name, col, newRouteIsBus.isOn);
			newRouteName.text = "";
			newRouteColor.value = 0;
			UpdateUILists();
		}
	}

	public void CreateVehicle() {
		if (newVehicleName.text != "" && newVehicleMax.text != "" && newVehicleStartTime.text != "" && newVehicleEndTime.text != "" && Array.IndexOf(transitSim.GetVehicleNames(), newVehicleName.text ) == -1) {
			string name = newVehicleName.text;
			int max = int.Parse(newVehicleMax.text);
			int startTime = int.Parse(newVehicleStartTime.text);
			int endTime = int.Parse(newVehicleEndTime.text);
			string onRoute = newVehicleOnRoute.captionText.text;

			transitSim.CreateVehicle(name, max, startTime, endTime, onRoute, newVehicleIsBus.isOn);
			newVehicleName.text = "";
			newVehicleMax.text = "";
			newRouteColor.value = 0;
			UpdateUILists();
		}
	}


	public void AppendRoute() {
		if (appendStopFirstTime.text != "" && appendStopLastTime.text != "") {
			float timeToLastStop = float.Parse(appendStopLastTime.text);
			float timeToFirstStop = float.Parse(appendStopFirstTime.text);
			transitSim.AppendStopToRoute(appendStopNames.captionText.text, appendRouteNames.captionText.text, timeToLastStop, timeToFirstStop);
			appendStopFirstTime.text = "";
			appendStopLastTime.text = "";
		}
	}


	// Update the dropdown lists based on the transit's hashtables
	public void UpdateUILists() {
		string[] stops = transitSim.GetStopNames();
		string[] routes = transitSim.GetRouteNames();

		Array.Sort(stops);
		Array.Sort(routes);

		appendStopNames.ClearOptions();
		appendStopNames.AddOptions(new List<String>(stops));

		appendRouteNames.ClearOptions();
		appendRouteNames.AddOptions(new List<String>(routes));

		bool vehicleRoutes = newVehicleIsBus.isOn;
		string[] vehicleBusTrainRoutes = transitSim.GetBusTrainRouteNames(vehicleRoutes);
		newVehicleOnRoute.ClearOptions();
		newVehicleOnRoute.AddOptions(new List<String>(vehicleBusTrainRoutes));
	}

	private void DisplaySelectedVehicleStop() {
		if (selectedVehicle != null) {
			string infoString = "Vehicle Name:  {0}\n" +
			                    "On Route:  {1}\n" +
			                    "Max Riders:  {2}\n" +
			                    "Num Riders:  {3}\n" +
			                    "Destinations:  {4}\n";

			infoString = String.Format(infoString, selectedVehicle.GetVehicleName(), selectedVehicle.GetOnRoute().GetRouteName(), selectedVehicle.GetMaxRiders(), selectedVehicle.GetNumRiders(), selectedVehicle.GetRiderInfo());
			infoText.text = infoString;
		}
		else if (selectedStop != null) {
			string infoString = "Stop Name:  {0}\n" +
				"Num Riders Waiting:  {1}\n" +
				"Destinations:  {2}";

			infoString = String.Format(infoString, selectedStop.GetStopName(), selectedStop.GetTotalRiders(), selectedStop.GetRiderInfo());
			infoText.text = infoString;
		}
		else {
			infoText.text = "";
		}
	}
}


