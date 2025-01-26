using UnityEngine;

[CreateAssetMenu(fileName = "PickupData", menuName = "Scriptable Objects/PickupData")]
public class PickupData : ScriptableObject
{
    public PickupType Type;
    public float Value;
    [Range(0,1f)] public float Chance;
}

public enum PickupType
{
    NailUpgrade,
    RoseUpgrade,
    ClubUpgrade,
    Bomb,
    Shield,
    Health
}
