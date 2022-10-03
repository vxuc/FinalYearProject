using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Record : MonoBehaviour
{
    public enum DATA_TYPE
    {
        DATA_TRANSFORM,
        DATA_WEATHER,
        DATA_TIMEOFDAY,
        DATA_CAMERA_ZOOM,
        DATA_CAMERA_MODE,
        DATA_CAMERA_TRACKING,
        DATA_TOTAL
    }


    [Header("ReplayManager")]
    [Tooltip("Drag and drop the Replay Manager of the scene here, not necessary if you put below the ReplayManager name.")]
    public ReplayManager replay;

    [Header("Replay Manager name in the scene")]
    [Tooltip("Insted of drag and dropping you can place the name of the ReplayManager here.")]
    //Replaymanager name in Scene
    [SerializeField] string replayManagerName = "ReplayManager";

    //Scripts of the GO that dont have to be disabled during replay
    [Header("Scripts to NOT disable")]
    [Tooltip("Drag and drop the scripts that dont have to be disabled during replay.")]
    [SerializeField] MonoBehaviour[] scripts = null;

    //Scripts of the GO that dont have to be disabled during replay
    [Header("Type of Record Data")]
    [Tooltip("Drag and drop the scripts that dont have to be disabled during replay.")]
    public DATA_TYPE dataType = DATA_TYPE.DATA_TRANSFORM;

    //List of recorded Frames 
    public List<Frame> frames = new List<Frame>();

    //RB recording
    private Rigidbody rigidBody;

    //animator recording
    private Animator animator;
    private bool startedRecording = false;
    private int animFramesRecorded = 0;

    //AudioSource recording
    private AudioSource audioSource;
    private bool audioPlay = false;
    private bool audioStarted = false;

    //Particle system recording
    private ParticleSystem particle;

    //Useful to know if it was instantiated during the recording
    private int numberFirstFrame;
    private bool instantiated = false;

    //Record Deleted while recording
    // if not deleted it will remain -1, if deleted it will take the frame where it was deleted
    private int recordDeletedFrame = -1;
    //deleted go 
    private GameObject deletedGO;

    //weather type
    private WeatherController.WEATHER_TYPE weatherType;

    [SerializeField] string prefabName;

    void Start()
    {
        //make sure replay is not NULL
        if (replay == null)
            replay = GameObject.Find(replayManagerName).GetComponent<ReplayManager>();

        //Get components
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        particle = GetComponent<ParticleSystem>();

        if (replay != null)
        {
            replay.AddRecord(this);

            //if (replay.ReplayMode())
            //{
            //    if (frames.Count > 0)
            //}
            //else
            //{
            //    if (frames.Count > 0)
            //        numberFirstFrame = replay.GetReplayLength();
            //}
            //if (frames.Count > 0) 
            //    numberFirstFrame = frames[0].record_data.spawnFrame;
            //first frame initialization, useful to know the frame where an instantiated go was spawned
            //

            //look if it is an instantiated go
            if (numberFirstFrame != 0) instantiated = true;
        }
        else
            Debug.LogWarning("ReplayManager not found, make sure there is a replayManger in the scene. Make sure to assign it by drag and drop or by puting the correct replayManagerName");
    }

    public void SetNumberFirstFrame(int i)
    {
        numberFirstFrame = i;
    }

    public string GetReplayManagerName()
    {
        return replayManagerName;
    }

    public void RecordFrame()
    {
        //record transforms
        Frame frame = new Frame(transform.position, transform.rotation, transform.localScale, name + DataManager.Instance.GetNewID().ToString(), prefabName, ReplayManager.Instance.GetRunningTime());
        frame.record_data.spawnFrame = ReplayManager.Instance.GetRunningTime();

        //record animations
        RecordAnimation();

        //record rigidBody velocities
        RecordRigidBody(frame);

        //record audio data
        RecordAudio(frame);

        //record particle data
        RecordParticle(frame);

        //record weather data
        RecordWeather(frame);

        //record time of day data
        RecordTimeOfDay(frame);

        //record camera zoom
        RecordCameraZoom(frame);

        //record camera tracking
        RecordCameraTracking(frame);

        //record camera mode
        RecordCameraMode(frame);

        //Add new frame to the list
        AddFrame(frame);
    }

    //Add frame, if list has maxLength remove first element
    void AddFrame(Frame frame)
    {
        if (GetLength() >= replay.GetMaxLength())
        {
            frames.RemoveAt(0);
        }

        frames.Add(frame);
        numberFirstFrame = frames[0].record_data.spawnFrame;
    }

    //Record RB velocities
    void RecordRigidBody(Frame frame)
    {
        if (rigidBody != null)
            frame.SetRBVelocities(rigidBody.velocity, rigidBody.angularVelocity);
    }

    //Record Animation
    void RecordAnimation()
    {
        if (animator != null && startedRecording == false)
        {
            animator.StartRecording(replay.GetAnimatorRecordLength());
            startedRecording = true;
        }
    }

    //Record Audio
    void RecordAudio(Frame frame)
    {
        if (audioSource != null)
        {
            if (audioSource.isPlaying && audioStarted == false)
            {
                audioStarted = true;
                audioPlay = true;
            }
            else if (audioStarted && audioPlay)
            {
                audioPlay = false;
            }
            else if (audioSource.isPlaying == false && audioStarted)
            {
                audioStarted = false;
            }

            frame.SetAudioData(new AudioData(audioPlay, audioSource.pitch, audioSource.volume, audioSource.panStereo, audioSource.spatialBlend, audioSource.reverbZoneMix));
        }
    }

    //Record Particle
    void RecordParticle(Frame frame)
    {
        if (particle != null)
        {
            if (particle.isEmitting)
                frame.SetParticleData(particle.time);
            else
                frame.SetParticleData(0f);
        }
    }

    //Record Weather
    void RecordWeather(Frame frame)
    {
        frame.SetWeatherData(WeatherController.Instance.weatherType);
    }
    
    void RecordTimeOfDay(Frame frame)
    {
        frame.SetTimeOfDayData(TimeOfDay.Instance.currTimeOfDay);
    }
    
    //Record Camera Zoom
    void RecordCameraZoom(Frame frame)
    {
        frame.SetCameraZoom(Camera.main.fieldOfView);
    }

    //Record Camera Tracking
    void RecordCameraTracking(Frame frame)
    {
        frame.SetCameraTracking(FindObjectOfType<InformationController>().GetCameraController().IsTracking());
    }

    //Record Camera Mode
    void RecordCameraMode(Frame frame)
    {
        frame.SetCameraMode(FindObjectOfType<ThermalController>().GetCameraMode());
    }

    //Prepare to record again
    public void ClearFrameList()
    {
        frames.Clear();
        animFramesRecorded = 0;
        numberFirstFrame = 0;
        instantiated = false;
        deletedGO = null;
        recordDeletedFrame = -1;
    }

    //used for the animator recording
    public void SetStartRecording(bool b)
    {
        startedRecording = b;
    }

    //when enter replay mode set to TRUE and when exit set to FALSE
    public void SetKinematic(bool b)
    {
        if (rigidBody != null)
            rigidBody.isKinematic = b;
    }

    //rearrange instantiation and deletion frames
    public void UpdateFramesNum()
    {
        if (replay.GetReplayLength() == replay.GetMaxLength())
        {
            //instantiated record
            if (numberFirstFrame > 0)
                numberFirstFrame--;

            //deleted record
            if (recordDeletedFrame != -1 && recordDeletedFrame > 0)
            {
                recordDeletedFrame--;

                //delete frames that are out of the replay
                if (instantiated == false || (instantiated == true && numberFirstFrame == 0))
                    if (frames.Count > 0)
                        frames.RemoveAt(0);
            }
        }
    }

    public void ManageScripts(bool enable)
    {
        MonoBehaviour[] allScripts = gameObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in allScripts)
        {
            if (script == null)
                continue;

            if (script != this && CheckScriptsList(script) == false)
                script.enabled = enable;
        }
    }

    bool CheckScriptsList(MonoBehaviour s)
    {
        bool ret = false;

        foreach (MonoBehaviour script in scripts)
        {
            if (script == s)
                ret = true; break;
        }

        return ret;
    }

    //SETTERS
    public void SetDeletedGameObject(GameObject go) { deletedGO = go; }
    public void SetFirstFrameIndex() { numberFirstFrame = frames[0].record_data.spawnFrame; }
    public void SetRecordDeletedFrame(int frame) { recordDeletedFrame = frame; frames[0].record_data.despawnFrame = frame; }

    public void IncreaseRecordedAnimatorFrames() { animFramesRecorded++; }

    // GETTERS
    public int GetLength() { return frames.Count; }
    public Frame GetFrameAtIndex(int index)
    {
        if (index < 0) return null;

        return index >= frames.Count ? null : frames[index];
    }

    //instantiation and deletion
    public int GetFirstFrameIndex() { return numberFirstFrame; }
    public bool IsInstantiated() { return instantiated; }
    public int GetRecordDeletedFrame() { return recordDeletedFrame; }

    //records Go and deleted GO
    public GameObject GetGameObject() { return gameObject; }
    public GameObject GetDeletedGO() { return deletedGO; }

    // other recorded components
    public Rigidbody GetRigidbody() { return rigidBody; }
    public Animator GetAnimator() { return null; }
    public int GetAnimFramesRecorded() { return animFramesRecorded; }
    public AudioSource GetAudioSource() { return null; }
    public ParticleSystem GetParticle() { return null; }
    public WeatherController.WEATHER_TYPE GetWeather() { return weatherType; }
}
