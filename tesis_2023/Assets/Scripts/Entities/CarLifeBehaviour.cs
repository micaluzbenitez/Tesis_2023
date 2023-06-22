using System;
using UnityEngine;

namespace Entities
{
    public class CarLifeBehaviour : MonoBehaviour
    {
        private int maxHealth;
        private int currentHealth;

        private float previousTime;
        private float speed;
        private float previousSpeed;

        private Vector3 previousPosition;

        public event Action<int> OnTakeDamage;
        public event Action<float> OnSpeedChange;
        private void Start()
        {
            maxHealth = 100;
            currentHealth = maxHealth;
            InitVelocityData();
        }

        private void TakeDamage(float damage)
        {
            currentHealth -= (int)damage;
            OnTakeDamage?.Invoke(currentHealth);

            Debug.Log(gameObject.name + " life: " + currentHealth);

            if (currentHealth <= 0)
                Debug.Log(gameObject.name + " is dead");

        }

        private void InitVelocityData()
        {
            speed = 0f;
            previousPosition = transform.position;
            previousTime = 0f;
        }
        private void FixedUpdate()
        {
            UpdateVelocityData();
        }
        private void UpdateVelocityData()
        {
            Vector3 currentPosition = transform.position;
            float currentTime = Time.time;
            float distance = Vector3.Distance(currentPosition, previousPosition);
            speed = (distance / (currentTime - previousTime)) * 3.6f;
            previousPosition = currentPosition;
            previousTime = currentTime;

            OnSpeedChange?.Invoke(speed);

            if (speed != 0)
                previousSpeed = speed;

        }
        private void ToDamageOpponent(Collision collision)
        {
            CarLifeBehaviour otherCarLife = collision.gameObject.GetComponent<CarLifeBehaviour>();

            if (otherCarLife != null)
            {
                otherCarLife.TakeDamage(previousSpeed / 2f);
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

        public int GetCurrenHealth()
        {
            return currentHealth;
        }
        public float GetSpeed()
        {
            return speed;
        }
    }
}