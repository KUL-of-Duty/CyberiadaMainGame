using Unity.Netcode;
using UnityEngine;

public class HeliMenu : NetworkBehaviour
{
    [SerializeField] GameLevel level;
    Terrain levelTerrain;
    SeedGenerator seedGen;
    bool clicked = false;

    private void Awake()
    {
        seedGen = FindAnyObjectByType<SeedGenerator>();
    }

}