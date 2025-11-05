using UnityEngine;

[CreateAssetMenu(menuName="Items/Gun")]
public class ItemSO : ScriptableObject 
{
    public string itemName;
    public GameObject model;
    public int ammo;
}
