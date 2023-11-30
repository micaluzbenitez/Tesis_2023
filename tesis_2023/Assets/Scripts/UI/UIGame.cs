using Managers;
using UnityEngine;

namespace UI
{
    public class UIGame : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField] private string gameSceneName = "";
        [SerializeField] private string mainMenuSceneName = "";

        [Header("Pause")]
        [SerializeField] private GameObject optionsPanel = null;

        public void LoadGameScene()
        {
            LoaderManager.Get().LoadScene(gameSceneName);
        }

        public void LoadMainMenuScene()
        {
            Time.timeScale = 1;
            LoaderManager.Get().LoadScene(mainMenuSceneName);
        }

        public void Pause()
        {
            optionsPanel.SetActive(true);
            Time.timeScale = 0;
        }

        public void Resume()
        {
            optionsPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}