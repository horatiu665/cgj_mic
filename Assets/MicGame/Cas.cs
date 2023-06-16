using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using ToyBoxHHH;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cas : MonoBehaviour
{
    private static Cas _instance;
    public static Cas instance => _instance ? _instance : (_instance = FindObjectOfType<Cas>());

    public List<Part> parts = new List<Part>();

    public int totalParts => parts.Count;
    public int partsThatArePainted => parts.Count(p => p.isPainted);

    public TMP_Text scoreText;

    private bool firstWin = false;

    public bool isWin;

    public SmartSound soundPartUpgraded;
    public SmartSound soundPartSad;
    public SmartSound soundWin;
    public SmartSound soundBombSplat;
    
    private void OnEnable()
    {
        Part.PartUpgraded += PartOnPartUpgraded;
        Bom.BombSplat += BomOnBombSplat;
    }

    private void OnDisable()
    {
        Part.PartUpgraded -= PartOnPartUpgraded;
        Bom.BombSplat -= BomOnBombSplat;
    }
    
    private void BomOnBombSplat(Vector3 pos)
    {
        if (soundBombSplat != null)
            soundBombSplat.Play();
    }

    private void PartOnPartUpgraded(bool firstTime)
    {
        if (partsThatArePainted >= totalParts)
        {
            if (soundWin != null)
                soundWin.Play();
        }
        else
        {
            if (firstTime)
            {
                if (soundPartUpgraded != null)
                    soundPartUpgraded.Play();
            }
            else
            {
                if (soundPartSad != null)
                    soundPartSad.Play();
            }
        }
    }

    // [DebugButton]
    public void EDIT_ReverseTheParts()
    {
        parts = GetComponentsInChildren<Part>().ToList();

        foreach (var p in parts)
        {
            var child = p.transform.GetChild(0);
            child.name = p.name + "[parent]";
            child.SetParent(p.transform.parent);
            p.transform.SetParent(child);
        }
    }


    public void SetPainted(bool p)
    {
        foreach (var part in parts)
        {
            part.SetPainted(p);
        }
    }

    private void Start()
    {
        parts = GetComponentsInChildren<Part>()
            .Where(p => !p.isSubpart).ToList();
        SetPainted(false);
    }

    private void Update()
    {
        if (scoreText != null)
        {
            if (partsThatArePainted < totalParts)
            {
                scoreText.text = "Painted " + partsThatArePainted +
                                 "/" + totalParts + " parts";
            }
            else
            {
                scoreText.text = "WIN! You painted the castle!";

                // only win once.
                if (!firstWin)
                {
                    firstWin = true;
                    Win();
                }
            }
        }
    }

    public void Win()
    {
        isWin = true;

        // set castle painted
        foreach (var p in parts)
        {
            p.SetPainted(true);
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    // code for ivan :)
    
    public Terrain terrain;

    public void SaveTerrainHeightmapToPng(Terrain terrain, string path)
    {
        var heightmap = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapResolution,
            terrain.terrainData.heightmapResolution);
        var tex = new Texture2D(terrain.terrainData.heightmapResolution, terrain.terrainData.heightmapResolution);
        for (var x = 0; x < terrain.terrainData.heightmapResolution; x++)
        {
            for (var y = 0; y < terrain.terrainData.heightmapResolution; y++)
            {
                var c = heightmap[x, y];
                tex.SetPixel(x, y, new Color(c, c, c));
            }
        }

        var bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);
    }
}