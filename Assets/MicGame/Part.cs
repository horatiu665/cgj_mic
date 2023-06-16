using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Part : MonoBehaviour
{
    public Material unpainted;
    public Material painted;

    public List<Part> subParts = new List<Part>();
    public bool isPainted => _isPainted;
    private bool _isPainted;

    public Part parentPart => transform.parent.GetComponentInParent<Part>();
    public bool isSubpart => parentPart != null;

    public static event Action<bool> PartUpgraded; // bool = is it first time it's upgraded?

    private void OnEnable()
    {
        subParts = GetComponentsInChildren<Part>()
            .Where(c => c != this).ToList();
    }

    public void SetPainted(bool p)
    {
        if (!isSubpart)
        {
            if (p && !_isPainted)
            {
                PartUpgraded?.Invoke(true);
            } else if (p && _isPainted)
            {
                PartUpgraded?.Invoke(false);
            }
        }

        this._isPainted = p;
        var mr = GetComponentInChildren<MeshRenderer>();
        mr.sharedMaterial = p ? painted : unpainted;

        if (p)
        {
            // play anim...
            var anim = GetComponentInChildren<Animator>();
            anim.SetTrigger("Hit");
        }

        foreach (var subP in subParts)
        {
            subP.SetPainted(p);
        }
    }
}