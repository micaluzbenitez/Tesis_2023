using UnityEngine.SceneManagement;
using Toolbox;

namespace Managers
{
    public class LoaderManager : MonoBehaviourSingleton<LoaderManager>
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}