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

    public WEATHER_TYPE weatherType;

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
        Debug.Log("Current Weather: " + weatherType.ToString());
        if (Input.GetKeyDown(KeyCode.R))
        {
            weatherType++;
            if (weatherType >= WEATHER_TYPE.WEATHER_TOTAL)
                weatherType = 0;
        }
    }
}
