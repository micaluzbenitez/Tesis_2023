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
            Vector3 velocity = displacement / (currentTime - previousTime);

            previousPosition = currentPosition;
            previousTime = currentTime;

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
                    float damageA = CalculateDamage(otherCarLife.rb.velocity.magnitude, rb.velocity.magnitude);
                    float damageB = CalculateDamage(rb.velocity.magnitude, otherCarLife.rb.velocity.magnitude);
                    TakeDamage(damageB);
                    otherCarLife.TakeDamage(damageA);
                }
            }
        }

        private float CalculateDamage(float speedA, float speedB)
        {
            float dmgMultiplier = 5f;
            float damage = dmgMultiplier * speedB / (speedA + speedB);

            return damage;
        }
        public void TakeDamage(float damage)
        {
            currentHealth -= (int)damage;
            Debug.Log(gameObject.name + " life: " + currentHealth);
            Debug.Log(gameObject.name + "velocity: " + rb.velocity);

            if (currentHealth <= 0)
            {

                Destroy(gameObject);
            }
        }
    }
}