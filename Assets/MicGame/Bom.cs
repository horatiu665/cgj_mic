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

    public float force = 45f;

    public static event Action<Vector3> BombSplat;
    
    public void Shoot(Vector3 dir)
    {
        rb.velocity = dir;
    }

    private void OnCollisionEnter(Collision other)
    {
        var part = other.collider.GetComponentInParent<Part>();
        if (part != null)
        {
            int montecarlo = 100;
            while (part.isSubpart && montecarlo-- > 0)
            {
                part = part.parentPart;
            }
            part.SetPainted(true);
            // we hit a part!
            Explode();
            return;
        }

        if (other.rigidbody != null)
        {
            var tre = other.rigidbody.GetComponent<Tre>();
            if (tre != null)
            {
                tre.Blow(other.contacts[0].point, force);
                // we hit a tree!
                Explode();
                return;
            }
            
            var bat = other.rigidbody.GetComponent<Bat>();
            if (bat != null)
            {
                // we hit the bat!
                ShootBat(other, bat);
            }
        }
        else
        {
            // hit random stuff.
            BombSplat?.Invoke(transform.position);
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