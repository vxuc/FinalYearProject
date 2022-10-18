using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainOnLensController : MonoBehaviour
{
    //Camera
    public CameraController renderTextureCamera;
    CameraController.CameraZoom currZoomLevel;
    public float cameraZoom;

    //Weathers
    WeatherController weatherController;
    public GameObject heavyRainOnLens;
    public GameObject drizzlingRainOnLens;
    WeatherController.WEATHER_TYPE currWeather;
    // Start is called before the first frame update
    void Start()
    {
        currZoomLevel = renderTextureCamera.GetCameraZoomLevel();

        weatherController = this.transform.parent.GetComponent<WeatherController>();
        currWeather = weatherController.GetWeatherType();
    }

    // Update is called once per frame
    void Update()
    {
        if (currWeather != weatherController.GetWeatherType())
        {
            currWeather = weatherController.GetWeatherType();
            switch (currWeather)
            {
                case WeatherController.WEATHER_TYPE.WEATHER_RAIN:
                    heavyRainOnLens.SetActive(true);
                    drizzlingRainOnLens.SetActive(false);
                    break;
                case WeatherController.WEATHER_TYPE.WEATHER_DRIZZLE:
                    heavyRainOnLens.SetActive(false);
                    drizzlingRainOnLens.SetActive(true);
                    break;
                default:
                    heavyRainOnLens.SetActive(false);
                    drizzlingRainOnLens.SetActive(false);
                    break;
            }
        }
        if(currZoomLevel != renderTextureCamera.GetCameraZoomLevel())
        {
            currZoomLevel = renderTextureCamera.GetCameraZoomLevel();
            switch (currZoomLevel)
            {
                case CameraController.CameraZoom.x24:
                    cameraZoom = 1 / 24f;
                    break;
                case CameraController.CameraZoom.x4:
                    cameraZoom = 1 / 4f;
                    break;
                default:
                    cameraZoom = 1f;
                    break;
            }
            heavyRainOnLens.GetComponent<MeshRenderer>().material.SetFloat("_cameraZoom", cameraZoom);
            drizzlingRainOnLens.GetComponent<MeshRenderer>().material.SetFloat("_cameraZoom", cameraZoom);
        }



    }
}
