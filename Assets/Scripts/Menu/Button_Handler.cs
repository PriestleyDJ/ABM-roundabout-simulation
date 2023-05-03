using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Handler : MonoBehaviour
{
    public static float broadLaneSPS;
    public static float hanoverStreetSPS;
    public static float brookHillSPS;
    public static float bolsoverStreetSPS;
    public static float netherthorpeRoadSPS;
    public static float speedValue;
    public static string CSVlocation;

    public void setBroadLaneSpeed(float newValue)
    {
            broadLaneSPS = newValue;
    }
    public void setHanoverSpeed(float newValue)
    {
        broadLaneSPS = newValue;
    }
    public void setBrookHillSpeed(float newValue)
    {
        broadLaneSPS = newValue;
    }
    public void setBolsoverSpeed(float newValue)
    {
        broadLaneSPS = newValue;
    }
    public void setNetherthorpeSpeed(float newValue)
    {
        broadLaneSPS = newValue;
    }
    public void setVehicleSpeedSliderValue(float newValue)
    {
        speedValue = newValue;
    }
    public void setCSVDirectory(string CSVLocation)
    {
        CSVlocation = CSVLocation;
    }
}
