using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public class CarLifeBehaviour : MonoBehaviour
    {
        [Header("HP")]
        [SerializeField] private int maxHealth = 100;

        [Header("Car parts")]
        [SerializeField] private Rigidbody[] parts = null;

        [Header("Feedbacks")]
        [SerializeField] private ParticleSystem smokeParticles;
        [SerializeField] private ParticleSystem fireParticles;
        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private ParticleSystem destroyParticles;
        [SerializeField] private ParticleSystem lava;
        [SerializeField] private ParticleSystem redSmoke;
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioSource fire;
        [SerializeField] private List<AudioClip> clips;
        [SerializeField] private LayerMask backCar;

        public List<Transform> childs;
        public List<Renderer> renderers;


        private int currentHealth;
        private int score;
        private float previousTime;
        private float speed;
        private float previousSpeed;
        private bool alive = true;

        private Vector3 previousPosition;

        public event Action OnZeroHealth;
        public event Action<int> OnPlayerLose;
        public event Action<int> OnWin;
        public event Action<int> OnTakeDamage;
        public event Action<int> OnIncreaseScore;
        public event Action<float> OnSpeedChange;

        private void Start()
        {

            score = 0;
            currentHealth = maxHealth;
            InitVelocityData();
            FindAllChilds(transform);
            FindAllRenderers();
            ChangeRenderersColors(1, 1, 1);

        }

        private void TakeDamage(float damage, CarLifeBehaviour other = null, bool sameDirection = false)
        {

            if (sameDirection)
            {
                if (other.GetSpeed() > GetSpeed())
                {
                    currentHealth -= (int)damage;
                    OnTakeDamage?.Invoke(currentHealth);
                }
                else
                {
                    currentHealth -= (int)damage / 5;
                    OnTakeDamage?.Invoke(currentHealth);
                }

            }
            else
            {
                currentHealth -= (int)damage;
                OnTakeDamage?.Invoke(currentHealth);
            }





            Debug.Log(gameObject.name + " life: " + currentHealth);

            if (currentHealth <= 25f)
            {
                smokeParticles.Stop();
                fireParticles.Play();
            }
            else if (currentHealth <= 50f)
            {
                smokeParticles.Play();
            }

            if (currentHealth <= 0 && alive)
            {
                fireParticles.Stop();
                explosionParticles.Play();
                destroyParticles.Play();
                OnZeroHealth?.Invoke();
                OnPlayerLose?.Invoke(score);
                alive = false;
                DestroyCarParts();
                ChangeRenderersColors(0, 0, 0);
                redSmoke.Play();
                lava.Play();
                if (source != null)
                {
                    PlaySound("Explosion");
                }
                fire.Play();
                Destroy(this);
            }
        }

        private void InitVelocityData()
        {
            speed = 0f;
            previousPosition = transform.position;
            previousTime = 0f;
        }
        private void Update()
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.forward * 15, Color.red);
        }
        private void FixedUpdate()
        {
            UpdateVelocityData();

            if (Input.GetKeyDown(KeyCode.M)) DestroyCarParts();
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

        private void ToDamageOpponent(Collision collision, bool sameDirection = false)
        {
            CarLifeBehaviour otherCarLife = collision.gameObject.GetComponent<CarLifeBehaviour>();

            if (otherCarLife != null)
            {
                if (sameDirection)
                    otherCarLife.TakeDamage(previousSpeed / 10f, otherCarLife, true);

                else
                    otherCarLife.TakeDamage(previousSpeed / 10f);

                score += (int)(previousSpeed / 2f);
                OnIncreaseScore?.Invoke(score);

                if (otherCarLife.GetCurrentHealth() <= 0 && source != null)
                {
                    PlaySound("Explosion");
                }

            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Car"))
            {
                Vector3 collisionNormal = collision.contacts[0].normal;
                float dotProduct = Vector3.Dot(transform.forward, collisionNormal);
                float allowedAngle = 0.8f;
                if (source != null && alive)
                    PlaySound("Crash");

                if (dotProduct < -allowedAngle)
                {
                    ToDamageOpponent(collision);
                }

                else if (dotProduct > allowedAngle)
                {
                    Debug.Log("de frente");
                    ToDamageOpponent(collision, true);

                }
            }
        }

        private void DestroyCarParts()
        {
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].gameObject.transform.SetParent(null);
                parts[i].gameObject.transform.rotation = Random.rotation;

                parts[i].isKinematic = false;
                parts[i].useGravity = true;

                Vector3 explosionDirection = (parts[i].position - transform.position).normalized;
                parts[i].AddForce(explosionDirection * Random.Range(10, 20), ForceMode.VelocityChange);
                parts[i].AddTorque(Random.insideUnitSphere * Random.Range(10, 20), ForceMode.VelocityChange);
            }
        }

        public void Win()
        {
            OnWin?.Invoke(score);
        }

        private void FindAllChilds(Transform objTransform)
        {
            childs.Add(objTransform);

            if (objTransform.childCount > 0)
            {
                for (int i = 0; i < objTransform.childCount; i++)
                {
                    FindAllChilds(objTransform.GetChild(i));
                }
            }
        }
        private void FindAllRenderers()
        {
            foreach (var child in childs)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderers.Add(renderer);
                }
            }
        }

        private void ChangeRenderersColors(float r, float g, float b)
        {

            foreach (var renderer in renderers)
            {
                Material[] materials = renderer.sharedMaterials;
                Color newColor = new Color(r, g, b);


                foreach (var material in materials)
                {
                    if (material != null)
                    {
                        if (material.name != "OutlineShader" && material.name != "OutlineShader2")
                        {
                            material.color = newColor;
                        }
                        else
                        {
                            material.color = new Color(0, 0, 0);
                        }
                    }
                }

            }
        }
        private void PlaySound(string name)
        {
            int index = clips.FindIndex(i => i.name == name);
            source.clip = clips[index];
            source.Play();
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        public float GetSpeed()
        {
            return speed;
        }
    }
}