using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
     [SerializeField] private PlayerController _player;
     private Slider _healthSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
          _healthSlider = GetComponent<Slider>();
          _healthSlider.maxValue = _player._heroDataSO.MaxHealth;
          _healthSlider.value = _healthSlider.maxValue;

          _player.HealthChangeEvent += OnHealthChanged;
    }

     private void OnDestroy()
     {
          _player.HealthChangeEvent -= OnHealthChanged;
     }

     private void OnHealthChanged(int newHealth)
     {
          _healthSlider.value = newHealth;
     }
}
