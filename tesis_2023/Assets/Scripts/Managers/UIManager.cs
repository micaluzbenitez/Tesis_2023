using UnityEngine;
using TMPro;
using Entities.Player;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private CarController carController;
    [SerializeField] private Slider healthBar;

    private void Start()
    {
        carController.OnSpeedChange += SetSpeedText;
        carController.OnRecieveCurrentHealth += SetHealthBar;
    }
    private void OnDestroy()
    {
        carController.OnSpeedChange -= SetSpeedText;
    }
    private void SetSpeedText(float speed)
    {
        speedText.text = Math.Round(speed).ToString() + " KM/H";
    }
    private void SetHealthBar(int health)
    {
        float result = (float)health / 100f;
        healthBar.value = result;
    }
}
