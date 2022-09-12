using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weather : MonoBehaviour
{
    public static Weather Instance;
    [SerializeField] TextMeshProUGUI text;

    public enum WEATHER_TYPE
    { 
        WEATHER_CLEAR,
        WEATHER_RAINY,
        WEATHER_CLOUDY,
        WEATHER_TOTAL
    }

    public WEATHER_TYPE weatherType = WEATHER_TYPE.WEATHER_CLEAR;

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

        text.text = "Weather: " + weatherType.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            weatherType++;
            if (weatherType >= WEATHER_TYPE.WEATHER_TOTAL)
                weatherType = 0;

        }
        text.text = "Weather: " + weatherType.ToString();
    }
}
