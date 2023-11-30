using UnityEngine;
using TMPro;
using Entities.Player;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI respawnText;
    [SerializeField] private Text gameplayScoreText;
    [SerializeField] private TextMeshProUGUI victoryScoreText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider turboSlider;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject miniMap;

    public void SetSpeedText(float speed)
    {
        speedText.text = Math.Round(speed).ToString() + " KM/H";
    }
    public void SetHealthBar(int health)
    {
        float result = (float)health / 100f;
        healthBar.value = result;
    }
    public void SetTurboSlider(float value)
    {

        turboSlider.value = value / 100f;
    }

    public void SetGameplayScoreText(int score)
    {
        gameplayScoreText.text = "Score: " + score.ToString();
    }
    public void SetVictoryScoreText(int score)
    {
        victoryScoreText.text = score.ToString();
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
    public void EnableDefeatPanel()
    {
        defeatPanel.gameObject.SetActive(true);
    }
    public void DisableGameplayUI()
    {
        healthBar.gameObject.SetActive(false);
        gameplayScoreText.gameObject.SetActive(false);
        speedText.gameObject.SetActive(false);
        miniMap.SetActive(false);
        turboSlider.gameObject.SetActive(false);
    }
}
