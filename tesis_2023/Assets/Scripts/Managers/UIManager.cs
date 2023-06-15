using UnityEngine;
using TMPro;
using Entities.Player;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private CarController carController;

    private void Start()
    {
        carController.OnSpeedChange += SetSpeedText;
    }
    private void OnDestroy()
    {
        carController.OnSpeedChange -= SetSpeedText;
    }
    private void SetSpeedText(float speed)
    {
        speedText.text = Math.Round(speed).ToString() + " KM/H";
    }
}
