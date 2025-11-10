using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class SceeneInitalizator : NetworkBehaviour
{
    [SerializeField] GameObject terrain;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        StartCoroutine(InitializeSceneGeneration());
    }

    private IEnumerator InitializeSceneGeneration()
    {
        PoissonDiscSampling treeGen = terrain.GetComponent<PoissonDiscSampling>();
        TerrainGenerator worldGen = terrain.GetComponent<TerrainGenerator>();
        SeedGenerator seedGen = terrain.GetComponent<SeedGenerator>();
        yield return StartCoroutine(seedGen.InitalizeSeedGeneration());
        yield return StartCoroutine(worldGen.InitalizeTerrainGenerator());
        yield return StartCoroutine(treeGen.InitializePoisson());
    }
}
