using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public Sprite Sprite;
    public bool Projectile;
    public bool NearestEnemy;
    public float BoxHeight;
    public float BoxWidth;
    public float Damage;
    public float AttackCooldown;
    public float Duration; // if 0, only one hit
    public float DebugDisplayTime;
}
