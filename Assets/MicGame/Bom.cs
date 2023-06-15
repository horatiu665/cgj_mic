using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : MonoBehaviour
{
    public Can cannon;
    
    private Rigidbody _rb;
    private Rigidbody rb => _rb ? _rb : (_rb = GetComponent<Rigidbody>());

    public ParticleSystem explosion;
    
    public void AddForce(Vector3 dir)
    {

        rb.velocity = dir;
    }

    private void OnCollisionEnter(Collision other)
    {
        var bat = other.rigidbody.GetComponent<Bat>();
        if (bat)
        {
            ShootBat(other, bat);
        }
        else
        {

            // explode..??
            if (explosion != null)
            {
                var e = Instantiate(explosion, transform.position, Quaternion.identity);
            }

            cannon.Unspawn(this);
        }
    }

    private void ShootBat(Collision other, Bat bat)
    {
        // shoot vaguely towards the castle...!!??!?!
    }
}
