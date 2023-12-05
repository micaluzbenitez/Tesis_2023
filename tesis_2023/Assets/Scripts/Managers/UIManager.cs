using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI respawnText;
    [SerializeField] private Text gameplayScoreText;
    [SerializeField] private TextMeshProUGUI victoryScoreText;
    [SerializeField] private TextMeshProUGUI defeatScoreText;
    [SerializeField] private TextMeshProUGUI victoryHighscoreText;
    [SerializeField] private TextMeshProUGUI defeatHighscoreText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider turboSlider;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject miniMap;
    [SerializeField] private UIGame uiGame;

    private void SetHighscore(int score)
    {
        if (score > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", score);
            PlayerPrefs.Save();
        }
    }

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
        SetHighscore(score);
        victoryScoreText.text = score.ToString();
        victoryHighscoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
    }

    public void SetDefeatScoreText(int score)
    {
        SetHighscore(score);
        defeatScoreText.text = score.ToString();
        defeatHighscoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
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
        ShowCursor();
        Time.timeScale = 0;
        uiGame.EndGame = true;
        victoryPanel.gameObject.SetActive(true);
    }

    public void EnableDefeatPanel()
    {
        ShowCursor();
        Time.timeScale = 0;
        uiGame.EndGame = true;
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

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
