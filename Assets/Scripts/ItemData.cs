// ItemData.cs
using UnityEngine;

public class ItemData : MonoBehaviour
{
    // TO POLE MUSI BYĆ PUBLICZNE (lub [SerializeField]), aby pojawiło się w Inspektorze.
    // Musisz mieć zdefiniowany ItemType (enum z pliku ItemType.cs)
    [SerializeField] public ItemType itemType; 
}