using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class GameLevel : ScriptableObject
{
    public Terrain terrain;
    public GameObject[] enviromentObjects;
}
