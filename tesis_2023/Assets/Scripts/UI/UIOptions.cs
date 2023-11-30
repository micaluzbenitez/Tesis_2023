using Managers;
using UnityEngine;

namespace UI
{
    public class UIOptions : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField] private string mainMenuSceneName = "";

        public void LoadMainMenuScene()
        {
            LoaderManager.Get().LoadScene(mainMenuSceneName);
        }
    }
}