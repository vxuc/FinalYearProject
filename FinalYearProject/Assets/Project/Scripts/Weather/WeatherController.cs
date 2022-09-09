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
        WEATHER_CLOUDY,
        WEATHER_TOTAL
    }

    public WEATHER_TYPE weatherType;

    public static WeatherController Instance;
    [SerializeField] TextMeshProUGUI text;

    [Header("Clouds")]
    public ParticleSystem cloudSystem;
    public Slider cloudSlider;
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
        }

        if (cloudSlider && cloudSystem)
        {
            cloudSlider.onValueChanged.AddListener
            (delegate
                {
                    cloudSystem.Clear();
                    var main = cloudSystem.main;
                    main.maxParticles = 40 * Mathf.RoundToInt(cloudSlider.value);
                    main.startSize = 10000 * Mathf.RoundToInt(cloudSlider.value);
                }
            );
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
