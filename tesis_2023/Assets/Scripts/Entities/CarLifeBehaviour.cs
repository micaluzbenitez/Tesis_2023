using UnityEngine;

namespace Entities
{
    public class CarLifeBehaviour : MonoBehaviour
    {
        [SerializeField] private int maxHealth;
        private int currentHealth;

        private Rigidbody rb;

        private Vector3 previousPosition;
        private float previousTime;
        Vector3 velocity;
        Vector3 previousVelocity;
        private void Start()
        {
            currentHealth = maxHealth;
            rb = GetComponent<Rigidbody>();
            InitVelocityData();
        }
        private void Update()
        {
            UpdateVelocityData();
        }

        private void TakeDamage(float damage)
        {
            currentHealth -= (int)damage;
            Debug.Log(gameObject.name + " life: " + currentHealth);

            if (currentHealth <= 0)
            {
                Debug.Log(gameObject.name + " is dead");
            }
        }

        private void InitVelocityData()
        {
            previousPosition = transform.position;
            previousTime = 0f;
        }
        private void UpdateVelocityData()
        {
            Vector3 currentPosition = transform.position;
            float currentTime = Time.time;
            Vector3 displacement = currentPosition - previousPosition;
            velocity = displacement / (currentTime - previousTime);
            previousPosition = currentPosition;
            previousTime = currentTime;

            if (velocity != Vector3.zero)
            {
                previousVelocity = velocity;
            }
        }
        private void ToDamageOpponent(Collision collision)
        {
            CarLifeBehaviour otherCarLife = collision.gameObject.GetComponent<CarLifeBehaviour>();

            if (otherCarLife != null)
            {
                otherCarLife.TakeDamage(previousVelocity.magnitude / 2f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Car"))
            {
                Vector3 collisionNormal = collision.contacts[0].normal;
                float dotProduct = Vector3.Dot(transform.forward, collisionNormal);
                float allowedAngle = 0.8f;


                if (dotProduct < -allowedAngle || dotProduct > allowedAngle)
                {
                    Debug.Log("de frente");
                    ToDamageOpponent(collision);

                }

            }
        }
    }
}