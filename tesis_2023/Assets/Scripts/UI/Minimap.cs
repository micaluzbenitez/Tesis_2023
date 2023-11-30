using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class Minimap : MonoBehaviour
    {
        [Header("Minimap")]
        [SerializeField] private Camera minimapCamera;
        [SerializeField] private RectTransform minimapRect; // Referencia al RectTransform del minimapa

        [Header("Prefabs")]
        [SerializeField] private RectTransform playerMark;
        [SerializeField] private RectTransform opponentMark;

        [Header("Entities")]
        [SerializeField] private Transform player;

        private RectTransform playerRect;
        private List<Transform> opponents = new List<Transform>();
        private List<RectTransform> opponentsRect = new List<RectTransform>();

        private void Start()
        {
            playerRect = Instantiate(playerMark, transform);
        }

        private void Update()
        {
            MarkEntity(playerRect, player);

            for (int i = 0; i < opponents.Count; i++)
            {
                MarkEntity(opponentsRect[i], opponents[i]);
            }
        }

        private void MarkEntity(RectTransform rect, Transform entity)
        {
            // Convierte la posición del jugador a las coordenadas del minimapa
            Vector3 minimapPos = minimapCamera.WorldToViewportPoint(entity.position);
            minimapPos.x *= minimapRect.sizeDelta.x;
            minimapPos.y *= minimapRect.sizeDelta.y;

            // Asegura que el punto del minimapa esté dentro de los límites del minimapa
            minimapPos.x = Mathf.Clamp(minimapPos.x, 0, minimapRect.sizeDelta.x);
            minimapPos.y = Mathf.Clamp(minimapPos.y, 0, minimapRect.sizeDelta.y);

            // Asigna la posición al punto del minimapa
            rect.transform.localPosition = minimapPos - (Vector3)minimapRect.sizeDelta * 0.5f;
        }

        public void InitOpponentMark(Transform opponent)
        {
            RectTransform opponentRect = Instantiate(opponentMark, transform);
            
            opponentsRect.Add(opponentRect);
            opponents.Add(opponent);
        }
    }
}