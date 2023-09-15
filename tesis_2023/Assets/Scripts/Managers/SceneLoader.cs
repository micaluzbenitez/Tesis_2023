using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

}
