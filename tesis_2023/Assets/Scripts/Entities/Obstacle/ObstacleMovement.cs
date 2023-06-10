using UnityEngine;

namespace Entities.Obstacle
{
    public class ObstacleMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float distance;

        private Vector3 initialPosition;
        private bool rightMovement = true;

        private void Start()
        {
            initialPosition = transform.position;
        }

        private void Update()
        {
            if (rightMovement)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.position.x >= (initialPosition.x + distance)) rightMovement = false;
            }
            else
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (transform.position.x <= initialPosition.x) rightMovement = true;
            }
        }
    }
}