using UnityEngine;
using TMPro;
using Entities.Player;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI respawnText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject victoryPanel;

    public void SetSpeedText(float speed)
    {
        speedText.text = Math.Round(speed).ToString() + " KM/H";
    }
    public void SetHealthBar(int health)
    {
        float result = (float)health / 100f;
        healthBar.value = result;
    }

    public void SetScoreText(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void EnableRespawnText()
    {
        respawnText.gameObject.SetActive(true);
    }
    public void DisableRespawnText()
    {
        respawnText.gameObject.SetActive(false);
    }
    public void EnableVictoryPanel()
    {
        victoryPanel.gameObject.SetActive(true);
    }
}
