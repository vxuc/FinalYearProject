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
        WEATHER_RAIN,
        WEATHER_DRIZZLE,
        WEATHER_CLOUDY,
        WEATHER_TOTAL
    }

    public WEATHER_TYPE weatherType;

    public static WeatherController Instance;
    [SerializeField] TextMeshProUGUI text;

    [Header("Clouds")]
    public ParticleSystem cloudSystem;
    public Slider cloudSlider;
    int minCloudParticles;
    float minCloudSize;
    int additionalCloud;

    [SerializeField] float cloudMaxDistance = 1600000;

    bool valueChange = false;
    public CloudsController cloudsController;

    [Header("Rains")]
    public ParticleSystem rainSystem;



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
        minCloudParticles = 0;
        minCloudSize = 100;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Weather: " + weatherType.ToString();
        if (Input.GetKeyDown(KeyCode.R))
        {
            weatherType++;
            if (weatherType >= WEATHER_TYPE.WEATHER_TOTAL)
                weatherType = 0;
            if(cloudSystem)
                cloudSystem.Clear();

            UpdateCloudV2();
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

        if (cloudSlider && cloudSystem)
        {
            UpdateCloud();
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
                break;
            case WEATHER_TYPE.WEATHER_CLOUDY:
                rainSystem.Clear();
                main.simulationSpeed = 0;
                break;
            case WEATHER_TYPE.WEATHER_DRIZZLE:
                main.simulationSpeed = 10;
                break;
            default:
                rainSystem.Clear();
                main.simulationSpeed = 0;
                break;

        }
    }

    void UpdateCloud()
    {
        var main = cloudSystem.main;

        switch (weatherType)
        {
            case WEATHER_TYPE.WEATHER_RAIN:
                minCloudParticles = 100;
                minCloudSize = 60000;
                break;
            case WEATHER_TYPE.WEATHER_CLOUDY:
                minCloudParticles = 40;
                minCloudSize = 30000;
                break;
            case WEATHER_TYPE.WEATHER_DRIZZLE:
                minCloudParticles = 40;
                minCloudSize = 25000;
                break;
            default:
                minCloudParticles = 0;
                minCloudSize = 10000;
                break;

        }

        cloudSlider.onValueChanged.AddListener
        (delegate
        {
            cloudSystem.Clear();
        }
        );

        main.maxParticles = 40 * Mathf.RoundToInt(cloudSlider.value) + minCloudParticles;
        main.startSize = 10000 * Mathf.RoundToInt(cloudSlider.value) + minCloudSize;
    }

    void UpdateCloudV2()
    {
        switch (weatherType)
        {
            case WEATHER_TYPE.WEATHER_RAIN:
                additionalCloud = 20;
                cloudSlider.value = 4;
                break;
            case WEATHER_TYPE.WEATHER_CLOUDY:
                additionalCloud = 15;
                cloudSlider.value = 2;
                break;
            case WEATHER_TYPE.WEATHER_DRIZZLE:
                additionalCloud = 10;
                cloudSlider.value = 1;
                break;
            default:
                cloudSlider.value = 0;
                cloudsController.ClearCloud();
                break;

        }
    }
}
