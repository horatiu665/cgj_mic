using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tre : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider[] cols;
    public Rigidbody rb => _rb ? _rb : (_rb = GetComponent<Rigidbody>());

    public static event Action<Vector3> OnTreeBlowup;
    
    private void Awake()
    {
        cols = GetComponentsInChildren<Collider>();
    }

    public void Blow(Vector3 position, float force)
    {
        OnTreeBlowup?.Invoke(position);
        
        rb.isKinematic = false;
        rb.AddForce(
            (Vector3.up * 2 + Random.onUnitSphere)
            * force);

        foreach (var c in cols)
        {
            c.enabled = false;
        }
        
        Destroy(gameObject, 10f);
    }
}