using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : MonoBehaviour
{
    public Can cannon;
    
    private Rigidbody _rb;
    public Rigidbody rb => _rb ? _rb : (_rb = GetComponent<Rigidbody>());

    public ParticleSystem explosion;
    
    public void Shoot(Vector3 dir)
    {
        rb.velocity = dir;
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.rigidbody == null)
        {
            Explode();
            return;
        }
        
        var bat = other.rigidbody.GetComponent<Bat>();
        if (bat != null)
        {
            ShootBat(other, bat);
        }
        else
        {
            Explode();
        }
    }

    private void Explode()
    {
        // explode..??
        if (explosion != null)
        {
            var e = Instantiate(explosion, transform.position, Quaternion.identity);
        }

        cannon.Unspawn(this);
    }
    
    private void ShootBat(Collision other, Bat bat)
    {
        // shoot vaguely towards the castle...!!??!?!
        bat.ShootBomb(this, other);


    }
}
