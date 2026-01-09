using UnityEngine;

[CreateAssetMenu(fileName = "Mushroom", menuName = "Scriptable Objects/Mushroom")]
public class Mushroom : ScriptableObject
{
    public string mushroomName;
    public GameObject[] spawnableEnemies;
}
