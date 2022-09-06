using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalController : MonoBehaviour
{
    int mode = 0;

    GameObject[] gameObjectsWithHeat;

    public GameObject infraredEnvironmentWhite, infraredEnvironmentBlack;

    [Header("Shader")]
    [SerializeField] Shader defaultShader;
    [SerializeField] Shader whiteShader;
    [SerializeField] Shader blackShader;

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

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            //Should be 2 later for black IR
            if (mode < 2)
                ++mode;
            else
                mode = 0;
        }
        ToggleInfrared(mode);
    }
    public void ToggleInfrared(int mode)
    {
       gameObjectsWithHeat = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));

        //Environment
        switch (mode)
        {
            case 1:
                infraredEnvironmentWhite.SetActive(true);
                infraredEnvironmentBlack.SetActive(false);
                break;
            case 2:
                infraredEnvironmentWhite.SetActive(false);
                infraredEnvironmentBlack.SetActive(true);
                break;
            default:
                infraredEnvironmentWhite.SetActive(false);
                infraredEnvironmentBlack.SetActive(false);
                break;
        }

        //Shader
        foreach (GameObject gameObject in gameObjectsWithHeat)
        {
            if (gameObject.GetComponent<Renderer>())
            {
                Renderer renderer = gameObject.GetComponent<Renderer>();
                switch (mode)
                {
                    case 1:
                        ActivateInfraredWhite(renderer);
                        break;
                    case 2:
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
}
