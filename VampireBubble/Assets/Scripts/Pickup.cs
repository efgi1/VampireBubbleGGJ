using System;
using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupData Data;
    [SerializeField] private AudioClip[] _pickupSounds = Array.Empty<AudioClip>();
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;

    public void Initialize(PickupData data)
    {
        Data = data;
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Data.Sprite;
    }

    public void OnPickup()
    {
        StartCoroutine(PickupHelper());
    }

    IEnumerator PickupHelper()
    {
        var randomIndex = UnityEngine.Random.Range(0, _pickupSounds.Length);
        _audioSource.PlayOneShot(_pickupSounds[randomIndex]);
        while (_audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
