using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalController : MonoBehaviour
{
    public enum CameraModes
    {
        COLOR,
        THERMAL_WHITE,
        THERMAL_BLACK,
        TOTAL_MODES
    };

    GameObject[] gameObjectsWithHeat;

    [Header("InfraredEnvironment")]
    public GameObject infraredEnvironmentWhite, infraredEnvironmentBlack;

    [Header("Shader")]
    public Shader defaultShader, whiteShader, blackShader;


    CameraModes cameraModes;
    // Start is called before the first frame update
    void Start()
    {
        gameObjectsWithHeat = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject gameObject in gameObjectsWithHeat)
        {
            if(gameObject.GetComponent<Renderer>() && gameObject.layer < 20)
                gameObject.GetComponent<Renderer>().material.shader = defaultShader;
        }
    }

    public void ChangeCameraMode(bool init = true)
    {
        if (init)
        {
            ++cameraModes;

            if (cameraModes >= CameraModes.TOTAL_MODES)
                cameraModes = 0;

            Debug.Log(cameraModes.ToString());
        }
        
        //Environment
        switch (cameraModes)
        {
            case CameraModes.THERMAL_WHITE:
                infraredEnvironmentWhite.SetActive(true);
                infraredEnvironmentBlack.SetActive(false);
                break;
            case CameraModes.THERMAL_BLACK:
                infraredEnvironmentWhite.SetActive(false);
                infraredEnvironmentBlack.SetActive(true);
                break;
            default:
                infraredEnvironmentWhite.SetActive(false);
                infraredEnvironmentBlack.SetActive(false);
                break;
        }

        gameObjectsWithHeat = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        //Shader
        foreach (GameObject gameObject in gameObjectsWithHeat)
        {
            if (gameObject.GetComponent<Renderer>() && gameObject.layer < 20)
            {
                Renderer renderer = gameObject.GetComponent<Renderer>();
                switch (cameraModes)
                {
                    case CameraModes.THERMAL_WHITE:
                        ActivateInfraredWhite(renderer);
                        break;
                    case CameraModes.THERMAL_BLACK:
                        ActivateInfraredBlack(renderer);
                        break;
                    default:
                        ActivateDefault(renderer);
                        break;
                }
            }
        }
    }

    private void ActivateDefault(Renderer renderer)
    {
        if(renderer.material.shader != defaultShader)
        {
            renderer.material.shader = defaultShader;
        }
    }

    private void ActivateInfraredWhite(Renderer renderer)
    {
        if(renderer.material.shader != whiteShader)
        {
            renderer.material.shader = whiteShader;

            if(renderer.gameObject.GetComponent<HeatController>())
            {
                SetInfraredHeatValue(renderer, renderer.gameObject.GetComponent<HeatController>().GetHeatValue());
            }
            else
                 SetInfraredHeatValue(renderer, 0);
        }   
    }

    private void ActivateInfraredBlack(Renderer renderer)
    {

        if (renderer.material.shader != blackShader)
        {
            renderer.material.shader = blackShader;

            if (renderer.gameObject.GetComponent<HeatController>())
            {
                SetInfraredHeatValue(renderer, renderer.gameObject.GetComponent<HeatController>().GetHeatValue());
            }
            else
                SetInfraredHeatValue(renderer, 0);
        }
    }
    void SetInfraredHeatValue(Renderer renderer,float value)
    {
        renderer.material.SetFloat("_Temperature", value);
    }

    public CameraModes GetCameraMode()
    { 
        return cameraModes;
    }

    public void SetCameraMode(CameraModes mode)
    {
        cameraModes = mode;
        ChangeCameraMode(false);
    }
}
