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

        private bool pauseGame = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!pauseGame) Pause();
                else Resume();
            }
        }

        public void LoadGameScene()
        {
            pauseGame = false;
            Time.timeScale = 1;
            LoaderManager.Get().LoadScene(gameSceneName);
        }

        public void LoadMainMenuScene()
        {
            pauseGame = false;
            Time.timeScale = 1;
            LoaderManager.Get().LoadScene(mainMenuSceneName);
        }

        public void Pause()
        {
            pauseGame = true;
            optionsPanel.SetActive(true);
            Time.timeScale = 0;
        }

        public void Resume()
        {
            pauseGame = false;
            optionsPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}