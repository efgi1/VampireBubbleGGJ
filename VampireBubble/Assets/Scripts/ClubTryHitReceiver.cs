using UnityEngine;

public class ClubTryHitReceiver : MonoBehaviour
{
    [SerializeField] private AreaOfEffectWeapon _weapon;

    public void TryHit()
    {
        _weapon.TryHit();
    }
}
