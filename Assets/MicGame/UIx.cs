using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIx : MonoBehaviour
{
    public TMP_InputField micLoudInput;
    public RectTransform micInputVisualizer;
    
    private void OnEnable()
    {
        micLoudInput.onValueChanged.AddListener(OnMicLoudInputChanged);
        micLoudInput.text = Mic.instance.globalLoudnessMultiplier.ToString();
    }

    private void OnDisable()
    {
        micLoudInput.onValueChanged.RemoveListener(OnMicLoudInputChanged);
    }

    private void Update()
    {
        micInputVisualizer.transform.localScale =
            new Vector3(
                Mathf.Lerp(0.05f, 1f, Mic.instance.smoothLoudness)
                , 1, 1);
    }

    private void OnMicLoudInputChanged(string arg0)
    {
        if (float.TryParse(arg0, out var f))
        {
            Mic.instance.globalLoudnessMultiplier = f;
        }
    }
}
