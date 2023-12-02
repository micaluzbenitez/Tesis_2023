using Managers;
using UnityEngine;

namespace UI
{
    public class UIGame : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField] private string gameSceneName = "";
        [SerializeField] private string mainMenuSceneName = "";

        [Header("Panels")]
        [SerializeField] private GameObject controlsPanel;
        [SerializeField] private GameObject optionsPanel;

        [Header("Control panel buttons")]
        [SerializeField] private GameObject startButton;
        [SerializeField] private GameObject backButton;

        private bool pauseGame = false;

        private void Start()
        {
            controlsPanel.SetActive(true);
            pauseGame = true;
            ShowCursor();
            Time.timeScale = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!pauseGame) Pause();
                else Resume();
            }
        }

        private void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void LoadGameScene()
        {
            ShowCursor();
            pauseGame = false;
            Time.timeScale = 1;
            LoaderManager.Get().LoadScene(gameSceneName);
        }

        public void LoadMainMenuScene()
        {
            ShowCursor();
            pauseGame = false;
            Time.timeScale = 1;
            LoaderManager.Get().LoadScene(mainMenuSceneName);
        }

        public void Pause()
        {
            ShowCursor();
            pauseGame = true;
            optionsPanel.SetActive(true);
            Time.timeScale = 0;
        }

        public void Resume()
        {
            HideCursor();
            pauseGame = false;
            optionsPanel.SetActive(false);
            Time.timeScale = 1;
        }

        public void StartGame()
        {
            Resume();
            controlsPanel.SetActive(false);
            startButton.SetActive(false);
            backButton.SetActive(true);
        }

        public void OpenControlsPopup()
        {
            controlsPanel.SetActive(true);
        }

        public void CloseControlsPopup()
        {
            controlsPanel.SetActive(false);
        }
    }
}