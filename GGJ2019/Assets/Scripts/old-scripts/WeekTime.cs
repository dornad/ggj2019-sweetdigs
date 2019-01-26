using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekTime : MonoBehaviour {

	public static float timeSinceStart = 0;

	public static int dayOfWeek = 0;

	public static int hour = 0;

	public static int minute = 0;
	
	// Update is called once per frame
	void Update () {

		timeSinceStart += Time.deltaTime;

		minute = (int)(timeSinceStart % 60);

		hour = (int)((timeSinceStart % 1440) / 60);

		dayOfWeek = (int)((timeSinceStart % 10080) / 1440);

	}

	public static string GetWeekTime() {
		string day = "";
		if (dayOfWeek == 0)
			day = "Sunday";
		else if (dayOfWeek == 1)
			day = "Monday";
		else if (dayOfWeek == 2)
			day = "Tuesday";
		else if (dayOfWeek == 3)
			day = "Wednesday";
		else if (dayOfWeek == 4)
			day = "Thursday";
		else if (dayOfWeek == 5)
			day = "Friday";
		else if (dayOfWeek == 6)
			day = "Saturday";

		string hourAdjust = hour < 10 ? "0" + hour : hour.ToString();
		string minuteAdjust = minute < 10 ? "0" + minute : minute.ToString();

		return string.Format("{0}, {1}:{2}", day, hourAdjust, minuteAdjust);
	}
}
