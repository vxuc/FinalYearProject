using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class TimeOfDay : MonoBehaviour
{
    public enum TIMEOFDAY
    {
        TIME_DAWN,
        TIME_MORNING,
        TIME_EVENING,
        TIME_DUSK,
        TIME_NIGHT,
        TIME_TOTAL
    }

    public static TimeOfDay Instance;
    public TIMEOFDAY currTimeOfDay;

    [Header("Ground Tint")]
    public Color DawnGroundTint;
    public Color MorningGroundTint;
    public Color EveningGroundTint;
    public Color DuskGroundTint;
    public Color NightGroundTint;

    [Header("Horizon Tint")]
    public Color DawnHorizonTint;
    public Color MorningHorizonTint;
    public Color EveningHorizonTint;
    public Color DuskHorizonTint;
    public Color NightHorizonTint;

    [SerializeField] List<Color> groundTints;
    [SerializeField] List<Color> horizonTints;

    [SerializeField] TextMeshProUGUI text;

    Volume volume;

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

        volume = GetComponent<Volume>();

        groundTints.Add(DawnGroundTint);
        groundTints.Add(MorningGroundTint);
        groundTints.Add(EveningGroundTint);
        groundTints.Add(DuskGroundTint);
        groundTints.Add(NightGroundTint);

        horizonTints.Add(DawnHorizonTint);
        horizonTints.Add(MorningHorizonTint);
        horizonTints.Add(EveningHorizonTint);
        horizonTints.Add(DuskHorizonTint);
        horizonTints.Add(NightHorizonTint);

        text.text = "Time: " + currTimeOfDay.ToString();
    }


    public void UpdateTimeOfDay()
    {
        VolumeProfile profile = volume.sharedProfile;
        if (!profile.TryGet<PhysicallyBasedSky>(out var sky))
        {
            sky = profile.Add<PhysicallyBasedSky>(false);
        }

        sky.groundTint.Override(groundTints[(int)currTimeOfDay]);
        sky.horizonTint.Override(horizonTints[(int)currTimeOfDay]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            currTimeOfDay++;
            if (currTimeOfDay >= TIMEOFDAY.TIME_TOTAL)
                currTimeOfDay = 0;
        }

        volume.enabled = true;
        UpdateTimeOfDay();
        text.text = "Time: " + currTimeOfDay.ToString();
    }
    public void SetTimeOfDay(int time)
    {
        this.currTimeOfDay = (TIMEOFDAY)time;
    }
}
