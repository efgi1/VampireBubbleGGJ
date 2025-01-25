using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public DamageType Type;
    public float Damage;
    public float FireRatePerSecond;
    public Vector2 Velocity;
    public Vector2 Acceleration;
}
