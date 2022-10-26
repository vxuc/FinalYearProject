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
    public Shader defaultShader,textureShader, whiteShader, blackShader;

    [Header("Cloud")]
    public Shader cloudDefaultShader,cloudShaderWhite, cloudShaderBlack;
    public Material cloudDefaultMaterial, cloudWhiteMaterial, cloudBlackMaterial;

    CameraModes cameraModes;
    // Start is called before the first frame update
    void Start()
    {
        //gameObjectsWithHeat = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));

        //foreach (GameObject gameObject in gameObjectsWithHeat)
        //{
        //    if(gameObject.GetComponent<Renderer>())
        //    {
        //        if (gameObject.layer < 20)
        //            gameObject.GetComponent<Renderer>().material.shader = defaultShader;
        //        else if (gameObject.layer < 20)
        //            gameObject.GetComponent<Renderer>().material.shader = defaultShader;
        //        else if (gameObject.layer == LayerMask.NameToLayer("Cloud")) // Cloud
        //            gameObject.GetComponent<Renderer>().material.shader = cloudDefaultShader;
        //    }
        //    if(gameObject.layer == LayerMask.NameToLayer("Particles"))
        //    {
        //        gameObject.SetActive(true);
        //    }
        //}
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
            if (gameObject.GetComponent<Renderer>())
            {
                if (gameObject.layer < 20)
                {
                    Renderer renderer = gameObject.GetComponent<Renderer>();
                    switch (cameraModes)
                    {
                        case CameraModes.THERMAL_WHITE:
                            SetShader(renderer, whiteShader);
                            break;
                        case CameraModes.THERMAL_BLACK:
                            SetShader(renderer, blackShader);
                            break;
                        default:
                            if (gameObject.layer == LayerMask.NameToLayer("ObjectsWithTexture"))
                                SetShader(renderer, textureShader);
                            else
                                SetShader(renderer, defaultShader);
                            break;
                    }
                }
                else if (gameObject.layer == LayerMask.NameToLayer("Cloud")) // Cloud
                {
                    Renderer renderer = gameObject.GetComponent<Renderer>();
                    switch (cameraModes)
                    {
                        case CameraModes.THERMAL_WHITE:
                            SetMaterial(renderer, cloudWhiteMaterial);
                            break;
                        case CameraModes.THERMAL_BLACK:
                            SetMaterial(renderer, cloudBlackMaterial);
                            break;
                        default:
                            SetMaterial(renderer, cloudDefaultMaterial);
                            break;
                    }
                }
                else if (gameObject.layer == LayerMask.NameToLayer("Particles"))
                {
                    switch (cameraModes)
                    {
                        default:
                            gameObject.SetActive(true);
                            break;
                        case CameraModes.THERMAL_WHITE:
                        case CameraModes.THERMAL_BLACK:
                            gameObject.SetActive(false);
                            break;
                    }
                }
            }
        }
    }

    private void SetShader(Renderer renderer,Shader shader)
    {
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            renderer.materials[i].shader = shader;
            if (renderer.gameObject.GetComponent<HeatController>())
            {
                SetInfraredHeatValue(renderer, renderer.gameObject.GetComponent<HeatController>().GetHeatValue());
            }
            else
                SetInfraredHeatValue(renderer, 0);
        }
    }
    private void SetMaterial(Renderer renderer,Material material)
    {
        renderer.material = material;
        if (renderer.gameObject.GetComponent<HeatController>())
        {
            SetInfraredHeatValue(renderer, renderer.gameObject.GetComponent<HeatController>().GetHeatValue());
        }
        else
            SetInfraredHeatValue(renderer, 0);
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
