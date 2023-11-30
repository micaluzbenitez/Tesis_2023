using Managers;
using UnityEngine;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField] private string gameSceneName = "";
        [SerializeField] private string creditsSceneName = "";

        public void LoadGameScene()
        {
            LoaderManager.Get().LoadScene(gameSceneName);
        }

        public void LoadCreditsScene()
        {
            LoaderManager.Get().LoadScene(creditsSceneName);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

    }
}