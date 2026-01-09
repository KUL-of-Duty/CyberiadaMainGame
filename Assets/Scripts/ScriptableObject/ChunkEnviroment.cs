using UnityEngine;

[CreateAssetMenu(fileName = "Chunk Enviroment", menuName = "Chunks")]
public class ChunkEnviroment : ScriptableObject
{
    public string chunkType;
    public GameObject[] chunkTrees;
    public GameObject[] chunkOtherObjects;
    public int maxMushrooms;
    public int minMushrooms;
    public int multipleMushroomsChance;
    public GameObject[] mushroomsPossible;
    public enum chunkDificulty
    {
        superEasy = 0,
        easy = 1,
        medium = 2,
        hard = 3,
        impossible = 4
    }
}
