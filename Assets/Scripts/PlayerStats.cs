using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float WalkSpeed = 2f;
    public float RunSpeed = 4f;
    public float CroachSpeed = 1f;
    public float JumpForce =0.005f;
    public float GroundCheckDistance = 1.1f;
    public float sensitivity = 1f;
    public float HealthPoints = 100f;
    public float StaminaPoints = 100f;

}
