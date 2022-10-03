using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}]", x, y, z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}

[System.Serializable]
public struct SerializableQuaternion
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// w component
    /// </summary>
    public float w;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    /// <param name="rW"></param>
    public SerializableQuaternion(float rX, float rY, float rZ, float rW)
    {
        x = rX;
        y = rY;
        z = rZ;
        w = rW;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
    }

    /// <summary>
    /// Automatic conversion from SerializableQuaternion to Quaternion
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Quaternion(SerializableQuaternion rValue)
    {
        return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
    }

    /// <summary>
    /// Automatic conversion from Quaternion to SerializableQuaternion
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableQuaternion(Quaternion rValue)
    {
        return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
    }
}

[Serializable]
public class Frame
{
    [Serializable]
    public struct RECORD_DATA
    {
        public int spawnFrame;
        public int despawnFrame;
        public string prefabName;
    }

    //transform data
    SerializableVector3 pos, scale;
    SerializableQuaternion rot;

    //RigidBody velocities
    SerializableVector3 RBvelocity, RBAngVelocity;

    //audio data
    //AudioData audio;

    //particles data
    float particleTime;

    //object name
    public string objName;

    //spawn time
    int spawnFrame;

    //weather index
    WeatherController.WEATHER_TYPE weatherType;

    //time of day index
    TimeOfDay.TIMEOFDAY timeOfDay;

    //camera mode
    ThermalController.CameraModes cameraMode;

    //record data
    public RECORD_DATA record_data;

    int cameraZoom;

    bool cameraTracking;

    //Constructor
    public Frame(Vector3 position, Quaternion rotation, Vector3 scale_, string _objName, string _prefabName, int _frameIndex)
    {
        pos = position;
        rot = rotation;
        scale = scale_;
        objName = _objName;
        record_data.prefabName = _prefabName;
        record_data.spawnFrame = _frameIndex;
    }

    //RigidBody set velocity data
    public void SetRBVelocities(Vector3 v, Vector3 aV)
    {
        RBvelocity = v;
        RBAngVelocity = aV;
    }

    //audio set data
    public void SetAudioData(AudioData data)
    {
        //audio = data;
    }

    //particle set data
    public void SetParticleData(float time)
    {
        particleTime = time;
    }
    
    //particle set data
    public void SetWeatherData(WeatherController.WEATHER_TYPE type)
    {
        weatherType = type;
    } 
    
    public void SetTimeOfDayData(TimeOfDay.TIMEOFDAY type)
    {
        timeOfDay = type;
    }
    
    public void SetCameraZoom(int zoom)
    {
        cameraZoom = zoom;
    }
    
    public void SetCameraTracking(bool track)
    {
        cameraTracking = track;
    }

    public void SetCameraMode(ThermalController.CameraModes mode)
    {
        cameraMode = mode;
    }

    //Getters
    public Vector3 GetPosition() { return pos; }
    public Vector3 GetScale() { return scale; }
    public Quaternion GetRotation() { return rot; }

    //RigidBody getter
    public Vector3 GetRBVelocity() { return RBvelocity; }
    public Vector3 GetRBAngularVelocity() { return RBAngVelocity; }

    //Audio getter
    public AudioData GetAudioData() { return null; } //audioData ; }

    //Particle getter
    public float ParticleTime() { return particleTime; }
    public WeatherController.WEATHER_TYPE GetWeatherData() { return weatherType; }
    public TimeOfDay.TIMEOFDAY GetTimeOfDayData() { return timeOfDay; }
    public int GetCameraZoom() { return cameraZoom; }
    public bool GetCameraTracking() { return cameraTracking; }
    public ThermalController.CameraModes GetCameraMode() { return cameraMode; }
}
