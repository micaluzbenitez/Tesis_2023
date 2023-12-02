using UnityEngine;

namespace Entities.Camera
{
    public class ThirdPersonCam : MonoBehaviour
    {
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}