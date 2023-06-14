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


            previousPosition = transform.position;
            previousTime = Time.time;
        }
        private void Update()
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

            if (gameObject.name == "Player")
            {
                Debug.Log((int)velocity.magnitude);

            }

        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Car"))
            {

                CarLifeBehaviour otherCarLife = collision.gameObject.GetComponent<CarLifeBehaviour>();
                if (otherCarLife != null)
                {

                    otherCarLife.TakeDamage(previousVelocity.magnitude / 2f);
                }
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= (int)damage;
            Debug.Log(gameObject.name + " life: " + currentHealth);

            if (currentHealth <= 0)
            {

                Destroy(gameObject);
            }
        }
    }
}