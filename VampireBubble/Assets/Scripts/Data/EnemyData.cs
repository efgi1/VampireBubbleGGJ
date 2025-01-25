using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float Health;
    public float Speed;
    public float Damage;
    public bool Flying;
    public Sprite Sprite;
}
