using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HideInverseKinematicsUI : MonoBehaviour
{
    private void Awake()
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mr in meshRenderers)
        {
            mr.enabled = false;
        }
    }
}
