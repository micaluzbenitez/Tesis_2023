using Managers;
using UnityEngine;

namespace UI
{
    public class UICredits : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField] private string mainMenuSceneName = "";

        public void LoadMainMenuScene()
        {
            LoaderManager.Get().LoadScene(mainMenuSceneName);
        }
    }
}