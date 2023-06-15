using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtBat : MonoBehaviour
{
    public Transform target;
    
    void Update()
    {
        transform.forward = target.position - transform.position;
    }
}
