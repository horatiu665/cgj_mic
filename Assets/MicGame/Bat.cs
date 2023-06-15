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

    public Transform batTipForSpeedMeasurement;
    private float curSpeed;
    private float curSpeedSmooth;
    private Vector3 prevPos;

    public float turnOffColliderForXSeconds = 1f;
    public Collider batCollider;
    private float colliderOffTime;

    public AnimationCurve batSpeedToBombForce = AnimationCurve.Linear(0, 0, 1, 1);
    public float maxBatSpeed = 10f;
    public float maxBombForce = 10f;

    public ParticleSystem shootParticles;
    
    [Header("Shoot bomb")]
    public Transform shootDirectionLeft;

    public Transform shootDirectionRight;


    public void SetRotation(float angle01)
    {
        var targetAngle = Mathf.Lerp(range.x, range.y, angle01);

        var targetRot = Quaternion.Euler(0, targetAngle, 0);
        rb.MoveRotation(targetRot);
    }

    private void Update()
    {
        var l = mic.smoothLoudness;
        SetRotation(l);

        curSpeed = (batTipForSpeedMeasurement.position - prevPos).magnitude / Time.deltaTime;
        curSpeedSmooth = Mathf.Lerp(curSpeedSmooth, curSpeed, 0.1f);
        prevPos = batTipForSpeedMeasurement.position;
    }

    private void FixedUpdate()
    {
        batCollider.enabled = Time.time - colliderOffTime > turnOffColliderForXSeconds;
    }

    public void ShootBomb(Bom bom, Collision other)
    {
        colliderOffTime = Time.time;

        var collisionPoint = other.contacts[0].point;
        var collisionVelocity = other.relativeVelocity;

        if (shootParticles != null)
        {
            var s = Instantiate(shootParticles, collisionPoint, Quaternion.identity);
        }
        
        Debug.Log("Hit! bat speed:" + curSpeedSmooth);

        // 01 point between the two directions
        var distLeft = (collisionPoint - shootDirectionLeft.position);
        var distRight = (collisionPoint - shootDirectionRight.position);

        // project the distances onto the left->right vector.
        var leftRight = shootDirectionRight.position - shootDirectionLeft.position;
        distLeft = Vector3.Project(distLeft, leftRight);
        distRight = Vector3.Project(distRight, leftRight);

        // proportion of left:
        var propLeft = distLeft.magnitude / leftRight.magnitude;
        var propRight = distRight.magnitude / leftRight.magnitude;

        // direction
        var shootDirLeft = shootDirectionLeft.forward;
        var shootDirRight = shootDirectionRight.forward;
        var finalShootDir = Vector3.Lerp(shootDirLeft, shootDirRight, propLeft);

        Debug.DrawRay(collisionPoint, finalShootDir * 10f, Color.red, 1f);

        var shootForce =
            batSpeedToBombForce.Evaluate(curSpeedSmooth / maxBatSpeed)
            * maxBombForce;
        bom.Shoot(finalShootDir * shootForce);
        bom.rb.useGravity = true;
    }
}