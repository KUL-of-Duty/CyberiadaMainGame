using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/Weapons")]
public class Weapons : ScriptableObject //okreslenie sciezki
//do powtorzenia, ponoc latwiej sie działa
{
    
    public string weaponName;
    public GameObject weaponPrefab; 
}
