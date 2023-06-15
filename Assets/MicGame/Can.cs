using System;
using System.Collections;
using System.Collections.Generic;
using ToyBoxHHH;
using UnityEngine;

public class Can : MonoBehaviour
{
    public float bombShootingRate = 1f;
    float lastBombShotTime = 0f;

    public float force = 10f;
    public Transform shootPos;

    public Bom bombPrefab;
    public Transform bombParent;
    public List<Bom> bombs = new List<Bom>();

    public Bom SpawnBomb(Vector3 position, Vector3 direction)
    {
        var bomb = Instantiate(bombPrefab, position, Quaternion.LookRotation(direction), bombParent);
        bombs.Add(bomb);
        bomb.cannon = this;

        bomb.AddForce(direction);

        return bomb;
    }

    public void Unspawn(Bom bomb)
    {
        bombs.Remove(bomb);
        Destroy(bomb.gameObject);
    }

    [DebugButton]
    public void ClearBombs()
    {
        foreach (var b in bombs)
        {
            Destroy(b.gameObject);
        }

        bombs.Clear();
    }

    private void Update()
    {
        if (Time.time - lastBombShotTime > bombShootingRate)
        {
            lastBombShotTime = Time.time;
            SpawnBomb(shootPos.position, shootPos.forward * force);
        }
    }
}