using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] PlayerManager _playerManagerScript;

    void Start()
    {
        healthSlider.maxValue = 100;
        healthSlider.value = 100;
    }

    void Update()
    {
        healthSlider.value = _playerManagerScript.currentHealth;
    }
}
