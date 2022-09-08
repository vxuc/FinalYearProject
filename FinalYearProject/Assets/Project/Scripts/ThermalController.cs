using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalController : MonoBehaviour
{
    public enum CameraModes
    {
        Color,
        ThermalWhite,
        ThermalBlack
    };

    GameObject[] gameObjectsWithHeat;

    public GameObject infraredEnvironmentWhite, infraredEnvironmentBlack;

    [Header("Shader")]
    [SerializeField] Shader defaultShader;
    [SerializeField] Shader whiteShader;
    [SerializeField] Shader blackShader;

    CameraModes cameraModes;
    // Start is called before the first frame update
    void Start()
    {
        gameObjectsWithHeat = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject gameObject in gameObjectsWithHeat)
        {
            if(gameObject.GetComponent<Renderer>())
                gameObject.GetComponent<Renderer>().material.shader = defaultShader;
        }
    }

    public void ChangeCameraMode()
    {
        int noOfCameraModes = System.Enum.GetValues(typeof(CameraModes)).Length - 1;
        int currMode = (int)cameraModes;
        ++currMode;

        if (currMode > noOfCameraModes)
            currMode = 0;

        cameraModes = (CameraModes)currMode;
        Debug.Log(cameraModes.ToString());
        //Environment
        switch (cameraModes)
        {
            case CameraModes.ThermalWhite:
                infraredEnvironmentWhite.SetActive(true);
                infraredEnvironmentBlack.SetActive(false);
                break;
            case CameraModes.ThermalBlack:
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
            if (gameObject.GetComponent<Renderer>())
            {
                Renderer renderer = gameObject.GetComponent<Renderer>();
                switch (cameraModes)
                {
                    case CameraModes.ThermalWhite:
                        ActivateInfraredWhite(renderer);
                        break;
                    case CameraModes.ThermalBlack:
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
}
