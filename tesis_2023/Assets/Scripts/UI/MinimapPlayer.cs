using UnityEngine;

namespace UI
{
    public class MinimapPlayer : MonoBehaviour
    {
        public Transform player; // Referencia al transform del jugador
        public RectTransform minimapRect; // Referencia al RectTransform del minimapa
        public Camera minimapCamera;

        void Update()
        {
            // Convierte la posición del jugador a las coordenadas del minimapa
            Vector3 minimapPos = minimapCamera.WorldToViewportPoint(player.position);
            minimapPos.x *= minimapRect.sizeDelta.x;
            minimapPos.y *= minimapRect.sizeDelta.y;

            // Asegura que el punto del minimapa esté dentro de los límites del minimapa
            minimapPos.x = Mathf.Clamp(minimapPos.x, 0, minimapRect.sizeDelta.x);
            minimapPos.y = Mathf.Clamp(minimapPos.y, 0, minimapRect.sizeDelta.y);

            // Asigna la posición al punto del minimapa
            transform.localPosition = minimapPos - (Vector3)minimapRect.sizeDelta * 0.5f;
        }
    }
}