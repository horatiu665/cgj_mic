using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Mic : MonoBehaviour
{
    // singl
    private static Mic _instance;
    public static Mic instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Mic>();
            }

            return _instance;
        }
    }
    // lazy audiosource audio
    private AudioSource _audioSource;
    private AudioSource audios
    {
        get
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            return _audioSource;
        }
    }

    public int micIndex = 0;

    private string micStarted;
    private bool isMicOn = false;

    private AudioClip micClip;

    [Header("Smoothness controls")]
    public float smooth = 0.5f;

    [FormerlySerializedAs("loudnessMultiplier")]
    public float globalLoudnessMultiplier = 5;

    public float smoothLoudness = 0f;

    public void SimpleStartMic(int index)
    {
        PrintMicDevices();

        micStarted = Microphone.devices[index];
        micClip = Microphone.Start(micStarted,
            true, 5, AudioSettings.outputSampleRate);
        isMicOn = true;
        Debug.Log("SimpleStartedMic: " + micStarted);
    }

    private static void PrintMicDevices()
    {
        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            Debug.Log("Device " + i + " - " + Microphone.devices[i]);
        }
    }

    public void StartMic(int index)
    {
        try
        {
            micStarted = Microphone.devices[index];
            micClip = Microphone.Start(
                micStarted,
                true, 30, AudioSettings.outputSampleRate);
            isMicOn = true;

            Debug.Log("Started mic " + micStarted);
        }
        catch (System.Exception e)
        {
            isMicOn = false;
            Debug.Log("Couldn't start mic because error. Try again maybe!");
            Debug.Log(e);
        }
    }

    public void StopMic()
    {
        if (isMicOn)
        {
            Microphone.End(micStarted);
            isMicOn = false;
        }
    }


    private IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("Microphone found");
        }
        else
        {
            Debug.Log("Microphone not found");
        }
        
        SimpleStartMic(micIndex);
    }

    private void Update()
    {
        smoothLoudness = 
            Mathf.Clamp01(
            Mathf.Lerp(smoothLoudness, GetLoudnessFromMic(), smooth)
        );
    }

    public int sampleWindow = 64;

    public float GetLoudnessFromMic()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(micStarted), micClip);
    }

    public float GetLoudnessFromAudioClip(int clipPos, AudioClip clip)
    {
        int startPosition = clipPos - sampleWindow;
        if (startPosition < 0)
            return 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            float wave = Mathf.Abs(waveData[i]);
            totalLoudness += wave;
        }

        return globalLoudnessMultiplier * totalLoudness / sampleWindow;
    }
}