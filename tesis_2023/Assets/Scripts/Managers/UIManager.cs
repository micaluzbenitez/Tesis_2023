using UnityEngine;
using TMPro;
using Entities.Player;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;

    [SerializeField] private Slider healthBar;


    public void SetSpeedText(float speed)
    {
        speedText.text = Math.Round(speed).ToString() + " KM/H";
    }
    public void SetHealthBar(int health)
    {
        float result = (float)health / 100f;
        healthBar.value = result;
    }
}
