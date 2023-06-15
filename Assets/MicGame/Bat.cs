using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private Rigidbody _rb;
    private Rigidbody rb => _rb ? _rb : (_rb = GetComponent<Rigidbody>());

    public Mic mic => Mic.instance;

    public Vector2 range = new Vector2(-85, 85f);

    public void SetRotation(float angle01)
    {
        var targetRot = Quaternion.Euler(0, Mathf.Lerp(range.x, range.y, angle01), 0);
        rb.MoveRotation(targetRot);
    }

    private void Update()
    {
        var l = mic.smoothLoudness;
        SetRotation(l);
    }
}