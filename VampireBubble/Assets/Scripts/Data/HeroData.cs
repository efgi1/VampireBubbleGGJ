using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "Scriptable Objects/HeroData")]
public class HeroData : ScriptableObject
{
     //TODO modifiers based off of specific heros, e.g. starting health/damage, weapons, etc.
     public int MaxHealth = 20;
     // Time before hero can take damage again
     public float DamageDelayTime = 0.3f;
}
