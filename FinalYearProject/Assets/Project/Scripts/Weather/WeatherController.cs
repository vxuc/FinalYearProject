using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherController : MonoBehaviour
{
    [Header ("Clouds")]
    public ParticleSystem cloudSystem;
    public Slider cloudSlider;

    // Update is called once per frame
    void Update()
    {

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
    }
}
