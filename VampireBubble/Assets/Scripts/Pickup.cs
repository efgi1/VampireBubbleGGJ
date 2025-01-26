using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupData Data;

    public void Initialize(PickupData data)
    {
        Data = data;
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Player"))
        {
            Other.GetComponent<PlayerController>().ApplyPickup(Data);
        }
    }
}
