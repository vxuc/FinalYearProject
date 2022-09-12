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
        WEATHER_RAINY,
        WEATHER_DRIZZLING,
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
        }

        if (cloudSlider && cloudSystem)
        {
            var main = cloudSystem.main;

            switch (weatherType)
            {
                case WEATHER_TYPE.WEATHER_RAINY:
                    minCloudParticles = 100;
                    minCloudSize = 60000;
                    break;
                case WEATHER_TYPE.WEATHER_CLOUDY:
                    minCloudParticles = 40;
                    minCloudSize = 30000;
                    break;
                case WEATHER_TYPE.WEATHER_DRIZZLING:
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


        if(rainSystem)
        {
            var main = rainSystem.main;
            switch (weatherType)
            {
                case WEATHER_TYPE.WEATHER_RAINY:
                    main.simulationSpeed = 100;
                    break;
                case WEATHER_TYPE.WEATHER_CLOUDY:
                    main.simulationSpeed = 5;
                    break;
                case WEATHER_TYPE.WEATHER_DRIZZLING:
                    main.simulationSpeed = 10;
                    break;
                default:
                    rainSystem.Clear();
                    main.simulationSpeed = 0;
                    break;

            }
        }
    }
}
