using System;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Mushroom", menuName = "Scriptable Objects/Mushroom")]
public class Mushroom : ScriptableObject
{
    public MushroomType mushroomType;
    public string mushroomName;
    public GameObject[] spawnableEnemies;

    public string getMushroomName()
    {
        return mushroomName;
    }
}
