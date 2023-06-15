using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIx : MonoBehaviour
{
    public TMP_InputField micLoudInput;

    private void OnEnable()
    {
        micLoudInput.onValueChanged.AddListener(OnMicLoudInputChanged);
        micLoudInput.text = Mic.instance.globalLoudnessMultiplier.ToString();
    }

    private void OnDisable()
    {
        micLoudInput.onValueChanged.RemoveListener(OnMicLoudInputChanged);
    }

    private void OnMicLoudInputChanged(string arg0)
    {
        if (float.TryParse(arg0, out var f))
        {
            Mic.instance.globalLoudnessMultiplier = f;
        }
    }
}
