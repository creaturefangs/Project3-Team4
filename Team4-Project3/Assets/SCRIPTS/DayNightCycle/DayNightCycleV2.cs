using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayNightCycleV2 : MonoBehaviour
{    
    [Header("skybox")]

    [SerializeField]
    private Material skyBoxMaterial;

    [Header("Day/Night Cycle")]

    [SerializeField]
    private float timeMultiplier;

    [SerializeField]
    private float startHour;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private float sunriseHour;

    [SerializeField]
    private float sunsetHour;

    [SerializeField]
    private Color dayAmbientLight;

    [SerializeField]
    private Color nightAmbientLight;

    [SerializeField]
    private AnimationCurve lightChangeCurve;

    [SerializeField]
    private float maxSunLightIntensity;

    [SerializeField]
    private Light moonLight;

    [SerializeField]
    private float maxMoonLightIntensity;

    private DateTime currentTime;

    private TimeSpan sunriseTime;

    private TimeSpan sunsetTime;

    [Header("Day Sky Color RGB")]
    
    public static float sunSkyRed;
    public static float sunSkyGreen;
    public static float sunSkyBlue;

    [Header("Night Sky Color RGB")]

    public static float moonSkyRed;
    public static float moonSkyGreen;
    public static float moonSkyBlue;
    

    [Header("Skybox Color Changer")]

    public Color dayTime = Color.blue;//new Color(sunSkyRed, sunSkyGreen, sunSkyBlue);
    public Color nightTime = Color.black;
    public float duration = 1.0F;

    public bool sunriseColorBool;

    public bool sunsetColorBool;

    [Header("Is it day or night")]

    public bool day;
    public bool night;  

    [Header("Audio Manager References")]

    public AudioClip dayTrack;
    public AudioClip nightTrack;

    // Start is called before the first frame update
    void Start()
    {   
        RenderSettings.skybox = skyBoxMaterial;

        if (startHour > 7 && startHour < 20) //Day
        {
            day = true;
            dayTime = Color.blue;//new Color(sunSkyRed, sunSkyGreen, sunSkyBlue);
        }
        if (startHour < 7 && startHour > 20) //Night
        {
            day = false;
            nightTime = Color.black; //(moonSkyRed, moonSkyGreen, moonSkyBlue);
        }

        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
        timeText = GameObject.Find("Clock").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        ChangeSkyColor();
        RotateSun();
        UpdateLightSettings();
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if (timeText != null)
        {
        timeText.text = currentTime.ToString("hh:mm tt");
        }
    }

    private void ChangeSkyColor()
    {
        if (timeText.text == "07:05") // Sunrise
        {
            day = true;
            night = false;

            AudioManager.instance.activateTimeZero();
            AudioManager.instance.SwapTrack(dayTrack);

            //skyBoxMaterial.shader._SUN;

            dayTime = Color.blue;
            //float lerp = Mathf.PingPong(Time.time, duration) / duration;
            //RenderSettings.skybox.SetColor("_SkyColor", Color.Lerp(nightTime, dayTime, lerp));
        }
        else if (timeText.text == "20:05") // SunSet
        {
            night = true;
            day = false;

            AudioManager.instance.activateTimeOne();
            AudioManager.instance.SwapTrack(nightTrack);

            nightTime = Color.black;
            //float lerp = Mathf.PingPong(Time.time, duration) / duration;
            //RenderSettings.skybox.SetColor("_SkyColor", Color.Lerp(dayTime, nightTime, lerp));
        }
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
}
