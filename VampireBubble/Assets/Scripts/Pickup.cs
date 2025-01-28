using System;
using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupData Data;
    [SerializeField] private AudioClip[] _pickupSounds = Array.Empty<AudioClip>();
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    [SerializeField] private GameObject _effect;

    public void Initialize(PickupData data)
    {
        Data = data;
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer.sprite = Data.Sprite;
        _effect.SetActive(false);
    }

    public void OnPickup()
    {
        StartCoroutine(PickupHelper());
    }

    IEnumerator PickupHelper()
    {
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        if (Data.Type == PickupType.Bomb)
        {
            _effect.SetActive(true);
        }
        var randomIndex = UnityEngine.Random.Range(0, _pickupSounds.Length);
        _audioSource.PlayOneShot(_pickupSounds[randomIndex]);
        while (_audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
