using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherController : MonoBehaviour
{    
    public enum WEATHER_TYPE
    { 
        WEATHER_CLEAR,
        WEATHER_CLOUDY,
        WEATHER_DRIZZLE,
        WEATHER_RAIN,
        WEATHER_TOTAL
    }

    public WEATHER_TYPE weatherType;

    [Header("Weather")]
    public static WeatherController Instance;
    [SerializeField] TextMeshProUGUI weatherText;

    [Header("Clouds")]
    public Slider cloudSlider;
    int additionalCloud;

    [SerializeField] float cloudMaxDistance;
    [SerializeField] TextMeshProUGUI cloudText;
    bool valueChange = false;
    public CloudsController cloudsController;

    [Header("ExtraCloudsWithButton")]
    public Slider cloudAreaMaxDistanceSlider;
    public Slider cloudAreaIntensitySlider;
    public bool[] cloudAreaButton;

    [Header("Rains")]
    public ParticleSystem rainSystem;
    public GameObject rainOnLens;



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        weatherText.text = "Weather: " + weatherType.ToString();
        if (Input.GetKeyDown(KeyCode.R))
        {
            weatherType++;
            if (weatherType >= WEATHER_TYPE.WEATHER_TOTAL)
                weatherType = 0;
            UpdateCloud();
        }
        
        if(cloudSlider)
        {
            cloudSlider.onValueChanged.AddListener
            (delegate
            {
                valueChange = true;
            }
            );
        }

        if (valueChange)
        {
            cloudsController.ClearCloud();
            cloudText.text = cloudSlider.value.ToString() + " Cloud Intenstity";
            if (cloudSlider.value != 0)
            {
                if (weatherType == WEATHER_TYPE.WEATHER_CLEAR)
                    weatherType = WEATHER_TYPE.WEATHER_CLOUDY;
                cloudsController.AddCloud((int)cloudSlider.value, cloudMaxDistance);
                if (additionalCloud != 0)
                {
                    cloudsController.AddAdditionalCloud(additionalCloud, cloudMaxDistance, (int)cloudSlider.value);
                }
            }
            else
                weatherType = WEATHER_TYPE.WEATHER_CLEAR;

            valueChange = false;
        }

        UpdateRain();
    }

    void UpdateRain()
    {
        var main = rainSystem.main;
        switch (weatherType)
        {
            case WEATHER_TYPE.WEATHER_RAIN:
                main.simulationSpeed = 100;
                rainOnLens.SetActive(true);
                break;
            case WEATHER_TYPE.WEATHER_CLOUDY:
                rainSystem.Clear();
                main.simulationSpeed = 0;
                rainOnLens.SetActive(false);
                break;
            case WEATHER_TYPE.WEATHER_DRIZZLE:
                main.simulationSpeed = 10;
                rainOnLens.SetActive(true);
                break;
            default:
                rainSystem.Clear();
                main.simulationSpeed = 0;
                rainOnLens.SetActive(false);
                break;

        }
    }

    void UpdateCloud()
    {
        switch (weatherType)
        {
            case WEATHER_TYPE.WEATHER_RAIN:
                additionalCloud = 20;
                cloudSlider.value = 4;
                break;
            case WEATHER_TYPE.WEATHER_CLOUDY:
                additionalCloud = 15;
                cloudSlider.value = 1;
                break;
            case WEATHER_TYPE.WEATHER_DRIZZLE:
                additionalCloud = 10;
                cloudSlider.value = 2;
                break;
            default:
                cloudSlider.value = 0;
                cloudsController.ClearCloud();
                break;

        }
    }

    public void UpdateCloudByButton(int buttonNo)
    {
        cloudsController.AddCloudWithButton(50, cloudMaxDistance, buttonNo);
    }

    public void SetWeather(int weather)
    {
        this.weatherType = (WEATHER_TYPE)weather;
        UpdateCloud();
    }

    public WEATHER_TYPE GetWeatherType()
    {
        return weatherType;
    }
}
