using System;
using UnityEngine;

namespace Entities
{
    public class CarLifeBehaviour : MonoBehaviour
    {
        [Header("HP")]
        [SerializeField] private int maxHealth = 100;

        [Header("Feedbacks")]
        [SerializeField] private ParticleSystem smokeParticles;
        [SerializeField] private ParticleSystem fireParticles;
        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private ParticleSystem destroyParticles;

        private int currentHealth;
        private int score;
        private float previousTime;
        private float speed;
        private float previousSpeed;
        private bool alive = true;

        private Vector3 previousPosition;

        public event Action OnZeroHealth;
        public event Action<int> OnWin;
        public event Action<int> OnTakeDamage;
        public event Action<int> OnIncreaseScore;
        public event Action<float> OnSpeedChange;

        private void Start()
        {
            score = 0;
            currentHealth = maxHealth;
            InitVelocityData();
        }

        private void TakeDamage(float damage)
        {
            currentHealth -= (int)damage;
            OnTakeDamage?.Invoke(currentHealth);

            Debug.Log(gameObject.name + " life: " + currentHealth);

            if ((currentHealth / maxHealth) <= 0.25f)
            {
                smokeParticles.Stop();
                fireParticles.Play();
            }
            else if ((currentHealth / maxHealth) <= 0.5f)
            {
                smokeParticles.Play();
            }

            if (currentHealth <= 0 && alive)
            {
                fireParticles.Stop();
                explosionParticles.Play();
                destroyParticles.Play();
                OnZeroHealth?.Invoke();
                this.enabled = false;
                alive = false;
            }
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
            previousSpeed = speed;
        }

        private void ToDamageOpponent(Collision collision)
        {
            CarLifeBehaviour otherCarLife = collision.gameObject.GetComponent<CarLifeBehaviour>();

            if (otherCarLife != null)
            {
                otherCarLife.TakeDamage(previousSpeed / 4f);
                score += (int)(previousSpeed / 2f);
                OnIncreaseScore?.Invoke(score);
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
        public void Win()
        {
            OnWin?.Invoke(score);
        }
    }
}